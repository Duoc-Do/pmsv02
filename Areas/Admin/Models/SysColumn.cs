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
    [Table("SysColumn")]
    public partial class SysColumn
    {
        [Key]
        public string Name { get; set; }
        public Nullable<bool> AllowDBNull { get; set; }
        public string CultureInfo { get; set; }
        public string DefaultValue { get; set; }
        public string Des { get; set; }
        public string FormatType { get; set; }
        public string FormatValue { get; set; }
        public Nullable<bool> ReadOnly { get; set; }
        public bool UType { get; set; }
        public Nullable<int> WidthDefault { get; set; }

    }
}