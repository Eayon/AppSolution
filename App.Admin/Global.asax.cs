using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using App.Admin;
using Microsoft.Practices.Unity;
using App.BLL.Core;

namespace App.Admin
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //启用压缩
            BundleTable.EnableOptimizations = true;
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            //注入 Ioc
            var container = new UnityContainer();
            DependencyRegisterType.Container_Sys(ref container);
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
        
  
        
        /// <summary>
        /// 全局的异常处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            
            string s = HttpContext.Current.Request.Url.ToString();
            HttpServerUtility server = HttpContext.Current.Server;
            if (server.GetLastError() != null)
            {
                Exception lastError = server.GetLastError();
                // 此处进行异常记录，可以记录到数据库或文本，也可以使用其他日志记录组件。
                ExceptionHander.WriteException(lastError);
                Application["LastError"] = lastError;
                int statusCode = HttpContext.Current.Response.StatusCode;
                string exceptionOperator = "/SysException/Error";
                try
                {
                    if (!String.IsNullOrEmpty(exceptionOperator))
                    {
                        exceptionOperator = new System.Web.UI.Control().ResolveUrl(exceptionOperator);
                        string url = string.Format("{0}?ErrorUrl={1}", exceptionOperator, server.UrlEncode(s));
                        string script = String.Format("<script language='javascript' type='text/javascript'>window.top.location='{0}';</script>", url);
                        Response.Write(script);
                        Response.End();
                    }
                }
                catch { }
            }
        }

    }
}