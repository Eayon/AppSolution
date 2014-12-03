using System;
using System.Collections.Generic;
using App.Common;
using App.Models;
namespace App.IBLL
{
    public interface ISysLogBLL
    {
        List<SysLog> GetList(ref GridPager pager,string queryStr);
        SysLog GetById(string id);
    }
}
