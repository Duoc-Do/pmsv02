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
    [Table("aspnet_Users")]
    public partial class aspnet_Users
    {
        [Key]
        public Guid UserId { get; set; }
        public Guid ApplicationId { get; set; }
        public bool IsAnonymous { get; set; }
        public System.DateTime LastActivityDate { get; set; }
        public string LoweredUserName { get; set; }
        public string MobileAlias { get; set; }
        public string UserName { get; set; }

    }
}