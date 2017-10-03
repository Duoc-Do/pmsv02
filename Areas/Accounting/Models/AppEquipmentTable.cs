﻿//------------------------------------------------------------------------------
// <auto-generated>
// gen by lotusviet.vn
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApp.Areas.Accounting.Models
{

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("AppEquipmentTable")]
    public partial class AppEquipmentTable
    {
        [Key]
        public int EquipmentID { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> AmountFC { get; set; }
        public string EquipmentName { get; set; }
        public int FixedAssetID { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public string UOMCode { get; set; }

    }
}