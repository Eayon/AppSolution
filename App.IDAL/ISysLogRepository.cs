using System;
using App.Models;
using System.Linq;
namespace App.IDAL
{
    public interface ISysLogRepository
    {
        int Create(SysLog entity);
        void Delete(DBContainer db, string[] deleteCollection);
        IQueryable<SysLog> GetList(DBContainer db);
        SysLog GetById(string id);
    }
}
