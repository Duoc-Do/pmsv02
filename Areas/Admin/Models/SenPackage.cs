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
    [Table("SenPackage")]
    public partial class SenPackage
    {
        [Key]
        public int PackageId { get; set; }
        public int ApplicationId { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }

    }
}