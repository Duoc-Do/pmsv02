
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
    [Table("AppConstructionBalanceTable")]
    public partial class AppConstructionBalanceTable
    {
        [Key]
        public int ConstructionBalanceID { get; set; }
        public int AccountID { get; set; }
        public System.DateTime BalanceDate { get; set; }
        public int ConstructionID { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public Nullable<decimal> Credit { get; set; }
        public Nullable<decimal> CreditFC { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<decimal> Debit { get; set; }
        public Nullable<decimal> DebitFC { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDateTime { get; set; }

    }
}