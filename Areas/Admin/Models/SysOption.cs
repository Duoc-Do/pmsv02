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
    [Table("SysOption")]
    public partial class SysOption
    {
        [Key]
        public int SysOptionID { get; set; }
        public string Des { get; set; }
        public string SysOptionDefault { get; set; }
        public string SysOptionName { get; set; }
        public string SysOptionValue { get; set; }
        public string Type { get; set; }

    }
}