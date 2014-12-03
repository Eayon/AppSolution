using System;
using System.Collections.Generic;
using App.Common;
using App.Models;
namespace App.IBLL
{
    public interface ISysExceptionBLL
    {
        List<SysException> GetList(ref GridPager pager,string queryStr);
        SysException GetById(string id);
    }
}
