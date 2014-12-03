using App.Common;
using App.IBLL;
using App.Models;
using App.Models.Sys;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Admin.Controllers
{
    public class AccountController : Controller
    {
        [Dependency]
        public IAccountBLL accountBLL { get; set; }

        //
        // GET: /Account/

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Login(string UserName, string Password, string Code)
        {
            //INSERT INTO [SysUser] ([Id],[UserName],[Password],[TrueName],[Card],[MobileNumber],[PhoneNumber],[QQ],[EmailAddress],[OtherContact],[Province],[City],[Village],[Address],[State],[CreateTime],[CreatePerson],[Sex],[Birthday],[JoinDate],[Marital],[Political],[Nationality],[Native],[School],[Professional],[Degree],[DepId],[PosId],[Expertise],[JobState],[Photo],[Attach]) values ('admin','admin','01-92-02-3A-7B-BD-73-25-05-16-F0-69-DF-18-B5-00','系统管理员',NULL,NULL,'06638888888','324345345','ymnets@sina.com','MSN：ymnets','440000','440100','440101','小小村落',1,'11 18 2012  3:40PM','admin','男','05 18 1900 12:00AM','01  1 2013 12:00AM','未婚','中国共产党','中国','广东','美国哈佛大学','计算机工程','小学','20000','20001','勤劳向学,为人友善,乐于助人','在职',NULL,NULL)
            if (Session["Code"] == null)
                return Json(JsonHandler.CreateMessage(0, "请重新刷新验证码"), JsonRequestBehavior.AllowGet);

            if (Session["Code"].ToString().ToLower() != Code.ToLower())
                return Json(JsonHandler.CreateMessage(0, "验证码错误"), JsonRequestBehavior.AllowGet);
            SysUser user = accountBLL.Login(UserName, ValueConvert.MD5(Password));
            if (user == null)
            {
                return Json(JsonHandler.CreateMessage(0, "用户名或密码错误"), JsonRequestBehavior.AllowGet);
            }
            else if (!Convert.ToBoolean(user.State))//被禁用
            {
                return Json(JsonHandler.CreateMessage(0, "账户被系统禁用"), JsonRequestBehavior.AllowGet);
            }

            AccountModel account = new AccountModel();
            account.Id = user.Id;
            account.TrueName = user.TrueName;
            Session["Account"] = account;

            return Json(JsonHandler.CreateMessage(1, ""), JsonRequestBehavior.AllowGet);
        }

        #region 验证码
        private int letterWidth = 15;//单个字体的宽度范围
        private int letterHeight = 27;//单个字体的高度范围
        private int letterCount = 4;//验证码位数
        private char[] chars = "1234567890".ToCharArray();
        private string[] fonts = { "Arial", "Georgia" };
        /// <summary>
        /// 产生波形滤镜效果
        /// </summary>
        private const double PI = 3.1415926535897932384626433832795;
        private const double PI2 = 6.283185307179586476925286766559;

        public ActionResult ValidateCode()
        {
            string str_ValidateCode = GetRandomNumberString(letterCount);
            Session["Code"] = str_ValidateCode;
            CreateImage(str_ValidateCode);
            return View();
        }

        public void CreateImage(string checkCode)
        {
            int int_ImageWidth = checkCode.Length * letterWidth + 5;
            Random newRandom = new Random();
            Bitmap image = new Bitmap(int_ImageWidth, letterHeight);
            Graphics g = Graphics.FromImage(image);
            //生成随机生成器
            Random random = new Random();
            //白色背景
            g.Clear(Color.White);
            //画图片的背景噪音线
            for (int i = 0; i < 10; i++)
            {
                int x1 = random.Next(image.Width);
                int x2 = random.Next(image.Width);
                int y1 = random.Next(image.Height);
                int y2 = random.Next(image.Height);

                g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
            }

            //画图片的前景噪音点
            for (int i = 0; i < 10; i++)
            {
                int x = random.Next(image.Width);
                int y = random.Next(image.Height);

                image.SetPixel(x, y, Color.FromArgb(random.Next()));
            }
            //随机字体和颜色的验证码字符

            int findex;
            for (int int_index = 0; int_index < checkCode.Length; int_index++)
            {
                findex = newRandom.Next(fonts.Length - 1);
                string str_char = checkCode.Substring(int_index, 1);
                Brush newBrush = new SolidBrush(GetRandomColor());
                Point thePos = new Point(int_index * letterWidth + 1 + newRandom.Next(3), 1 + newRandom.Next(3));//5+1+a+s+p+x
                g.DrawString(str_char, new Font(fonts[findex], 14, FontStyle.Bold), newBrush, thePos);
            }
            //灰色边框
            g.DrawRectangle(new Pen(Color.LightGray, 1), 0, 0, int_ImageWidth - 1, (letterHeight - 1));
            //图片扭曲
            //image = TwistImage(image, true, 3, 4);
            //将生成的图片发回客户端
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);
            Response.ClearContent(); //需要输出图象信息 要修改HTTP头 
            Response.ContentType = "image/Png";
            Response.BinaryWrite(ms.ToArray());
            g.Dispose();
            image.Dispose();
        }
        /// <summary>
        /// 正弦曲线Wave扭曲图片
        /// </summary>
        /// <param name="srcBmp">图片路径</param>
        /// <param name="bXDir">如果扭曲则选择为True</param>
        /// <param name="nMultValue">波形的幅度倍数，越大扭曲的程度越高，一般为3</param>
        /// <param name="dPhase">波形的起始相位，取值区间[0-2*PI)</param>
        /// <returns></returns>
        public System.Drawing.Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            System.Drawing.Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);

            // 将位图背景填充为白色
            System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(destBmp);

            graph.FillRectangle(new SolidBrush(System.Drawing.Color.White), 0, 0, destBmp.Width, destBmp.Height);

            graph.Dispose();

            double dBaseAxisLen = bXDir ? (double)destBmp.Height : (double)destBmp.Width;

            for (int i = 0; i < destBmp.Width; i++)
            {
                for (int j = 0; j < destBmp.Height; j++)
                {
                    double dx = 0;

                    dx = bXDir ? (PI2 * (double)j) / dBaseAxisLen : (PI2 * (double)i) / dBaseAxisLen;

                    dx += dPhase;

                    double dy = Math.Sin(dx);

                    // 取得当前点的颜色
                    int nOldX = 0, nOldY = 0;

                    nOldX = bXDir ? i + (int)(dy * dMultValue) : i;

                    nOldY = bXDir ? j : j + (int)(dy * dMultValue);

                    System.Drawing.Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }
            return destBmp;
        }
        public Color GetRandomColor()
        {
            Random RandomNum_First = new Random((int)DateTime.Now.Ticks);
            System.Threading.Thread.Sleep(RandomNum_First.Next(50));
            Random RandomNum_Sencond = new Random((int)DateTime.Now.Ticks);
            int int_Red = RandomNum_First.Next(210);
            int int_Green = RandomNum_Sencond.Next(180);
            int int_Blue = (int_Red + int_Green > 300) ? 0 : 400 - int_Red - int_Green;
            int_Blue = (int_Blue > 255) ? 255 : int_Blue;
            return Color.FromArgb(int_Red, int_Green, int_Blue);
        }
        //  生成随机数字字符串
        public string GetRandomNumberString(int int_NumberLength)
        {
            Random random = new Random();
            string validateCode = string.Empty;
            for (int i = 0; i < int_NumberLength; i++)
                validateCode += chars[random.Next(0, chars.Length)].ToString();
            return validateCode;
        }
        #endregion
    }
}
