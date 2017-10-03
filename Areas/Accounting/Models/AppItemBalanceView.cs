namespace WebApp.Areas.Accounting.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("AppItemBalanceView")]
    public partial class AppItemBalanceView
    {
        public AppItemBalanceView()
        {
            Quantity = 0;
            Amount = 0;
            AmountFC = 0;
            BalanceDate = DateTime.Today;
        }


        public Nullable<int> AccountID { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> AmountFC { get; set; }
        public Nullable<System.DateTime> BalanceDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public string DisplayNumber { get; set; }
        [Key]
        public int ItemBalanceID { get; set; }
        public string ItemCode { get; set; }
        public Nullable<int> ItemID { get; set; }
        public string ItemName { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDateTime { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public string WarehouseCode { get; set; }
        public Nullable<int> WarehouseID { get; set; }
        public string WarehouseName { get; set; }
    }
}