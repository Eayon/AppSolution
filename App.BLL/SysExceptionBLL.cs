using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using App.IDAL;
using App.Common;
using App.Models.Sys;
using App.Models;
using App.IBLL;
namespace App.BLL
{
    public class SysExceptionBLL: ISysExceptionBLL
    {
        [Dependency]
        public ISysExceptionRepository exceptionRepository { get; set; }
 
       
        public List<SysException> GetList(ref GridPager pager, string queryStr)
        {
            DBContainer db = new DBContainer();
            List<SysException> query = null;
            IQueryable<SysException> list = exceptionRepository.GetList(db);
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                list = list.Where(a => a.Message.Contains(queryStr));
                pager.totalRows = list.Count();
            }
            else
            {
                pager.totalRows = list.Count();
            }

            if (pager.order == "desc")
            {
                query = list.OrderByDescending(c => c.CreateTime).Skip((pager.page - 1) * pager.rows).Take(pager.rows).ToList();
            }
            else
            {
                query = list.OrderBy(c => c.CreateTime).Skip((pager.page - 1) * pager.rows).Take(pager.rows).ToList();
            }

            return query;
        }
        public SysException GetById(string id)
        {
            return exceptionRepository.GetById(id);
        }
    }
}
