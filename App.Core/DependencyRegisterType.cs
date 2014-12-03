using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.BLL;
using App.DAL;
using App.IBLL;
using App.IDAL;
using Microsoft.Practices.Unity;

namespace App.Admin
{
    public class DependencyRegisterType
    {
        //系统注入
        public static void Container_Sys(ref UnityContainer container)
        {
            container.RegisterType<ISysSampleBLL, SysSampleBLL>();//样例
            container.RegisterType<ISysSampleRepository, SysSampleRepository>();

            container.RegisterType<IHomeBLL, HomeBLL>();
            container.RegisterType<IHomeRepository, HomeRepository>();

            container.RegisterType<IHomeBLL, HomeBLL>();
            container.RegisterType<IHomeRepository, HomeRepository>();

            container.RegisterType<ISysLogBLL, SysLogBLL>();
            container.RegisterType<ISysLogRepository, SysLogRepository>();

            container.RegisterType<ISysExceptionBLL, SysExceptionBLL>();
            container.RegisterType<ISysExceptionRepository, SysExceptionRepository>();

            container.RegisterType<IAccountBLL, AccountBLL>();
            container.RegisterType<IAccountRepository, AccountRepository>();

            container.RegisterType<ISysModuleBLL, SysModuleBLL>();
            container.RegisterType<ISysModuleRepository, SysModuleRepository>();

            container.RegisterType<ISysModuleOperateBLL, SysModuleOperateBLL>();
            container.RegisterType<ISysModuleOperateRepository, SysModuleOperateRepository>();
        }
    }
}