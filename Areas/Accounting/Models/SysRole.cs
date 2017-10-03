//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApp.Areas.Accounting.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SysRole")]
    public partial class SysRole
    {
        public SysRole()
        {
            this.SysBusinessRoles = new HashSet<SysBusinessRole>();
            this.SysMenuRoles = new HashSet<SysMenuRole>();
        }
        [Key]
        public int RoleID { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public bool IsAdmin { get; set; }
        public string Note { get; set; }

        public virtual ICollection<SysBusinessRole> SysBusinessRoles { get; set; }
        public virtual ICollection<SysMenuRole> SysMenuRoles { get; set; }
    }
}
