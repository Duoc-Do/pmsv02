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
    [Table("SenService")]
    public partial class SenService
    {
        [Key]
        public int ServiceId { get; set; }
        public int CompanyId { get; set; }
        public Guid UserId { get; set; }

        //public virtual SenCompany SenCompany { get; set; }
        //public virtual aspnet_Users User { get; set; }

    }
}
