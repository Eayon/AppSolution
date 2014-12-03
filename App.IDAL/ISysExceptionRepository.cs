using System;
using App.Models;
using System.Linq;
namespace App.IDAL
{
    public interface ISysExceptionRepository
    {
        int Create(SysException entity);
        IQueryable<SysException> GetList(DBContainer db);
        SysException GetById(string id);
    }
}
