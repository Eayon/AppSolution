﻿using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.IDAL
{
    public interface ISysModuleOperateRepository
    {
        IQueryable<SysModuleOperate> GetList(DBContainer db);
        int Create(SysModuleOperate entity);
        int Delete(string id);
        SysModuleOperate GetById(string id);
        bool IsExist(string id);
    }
}
