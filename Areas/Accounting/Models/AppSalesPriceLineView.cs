
//------------------------------------------------------------------------------
// <auto-generated>
// gen by lotusviet.vn
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApp.Areas.Accounting.Models
{

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("AppSalesPriceLineView")]
    public partial class AppSalesPriceLineView
    {
        public string ItemCode { get; set; }
        public string ItemGroupCode { get; set; }
        public Nullable<int> ItemGroupID { get; set; }
        public string ItemGroupName { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public Nullable<decimal> QuantityFrom { get; set; }
        public Nullable<decimal> QuantityTo { get; set; }
        public string SalesPriceGroupCode { get; set; }
        public int SalesPriceGroupID { get; set; }
        public string SalesPriceGroupName { get; set; }
        [Key]
        public long SalesPriceLineID { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> UnitPriceFC { get; set; }

    }
}