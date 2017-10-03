
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
    [Table("AppItemBalanceTable")]
    public partial class AppItemBalanceTable
    {
        public int AccountID { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountFC { get; set; }
        public System.DateTime BalanceDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        [Key]
        public int ItemBalanceID { get; set; }
        public int ItemID { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDateTime { get; set; }
        public decimal Quantity { get; set; }
        public int WarehouseID { get; set; }

    }
}