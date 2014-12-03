using App.BLL.Core;
using App.IBLL;
using App.IDAL;
using App.Models.Sys;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL
{
    public class SysUserBLL : BaseBLL, ISysUserBLL
    {
        [Dependency]
        public ISysRightRepository sysRightRepository { get; set; }
        public List<permModel> GetPermission(string accountid, string controller)
        {
            return sysRightRepository.GetPermission(accountid, controller);
        }
    }
}
