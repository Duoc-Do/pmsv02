﻿
//------------------------------------------------------------------------------
// <auto-generated>
// gen by lotusviet.vn
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApp.Areas.Admin.Models
{

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("aspnet_Roles")]
    public partial class aspnet_Roles
    {
        [Key]
        public Guid RoleId { get; set; }
        public Guid ApplicationId { get; set; }
        public string Description { get; set; }
        public string LoweredRoleName { get; set; }
        public string RoleName { get; set; }

    }
}
