//------------------------------------------------------------------------------
// <auto-generated>
// gen by lotusviet.vn
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApp.Areas.Accounting.Models
{

    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("RpSalesLinePrice")]
    public partial class RpSalesLinePrice
    {
        public string ItemCode { get; set; }
        public string ItemGroupCode { get; set; }
        public string ItemGroupName { get; set; }
        public string ItemName { get; set; }
        public Nullable<decimal> Quantity1 { get; set; }
        public Nullable<decimal> Quantity2 { get; set; }
        public Nullable<decimal> Quantity3 { get; set; }
        public string SalesPriceGroupCode { get; set; }
        public string SalesPriceGroupName { get; set; }
        public Nullable<decimal> UnitPrice1 { get; set; }
        public Nullable<decimal> UnitPrice2 { get; set; }
        public Nullable<decimal> UnitPrice3 { get; set; }
        public string UOMCode { get; set; }

    }
}