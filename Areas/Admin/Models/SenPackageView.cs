﻿
//------------------------------------------------------------------------------
// <auto-generated>
// gen by lotusviet.vn
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApp.Areas.Admin.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("SenPackageView")]
    public partial class SenPackageView
    {
        public int ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        [Key]
        public int PackageId { get; set; }

    }
}