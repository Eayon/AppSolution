//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace App.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SysRight
    {
        public SysRight()
        {
            this.SysRightOperate = new HashSet<SysRightOperate>();
        }
    
        public string Id { get; set; }
        public string ModuleId { get; set; }
        public string RoleId { get; set; }
        public bool Rightflag { get; set; }
    
        public virtual SysModule SysModule { get; set; }
        public virtual SysRole SysRole { get; set; }
        public virtual ICollection<SysRightOperate> SysRightOperate { get; set; }
    }
}
