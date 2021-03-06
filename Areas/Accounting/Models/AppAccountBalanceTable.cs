
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
    [Table("AppAccountBalanceTable")]
    public partial class AppAccountBalanceTable
    {
        [Key]
        public int AccountBalanceID { get; set; }
        public int AccountID { get; set; }
        public System.DateTime BalanceDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public decimal Credit { get; set; }
        public decimal CreditFC { get; set; }
        public decimal Debit { get; set; }
        public decimal DebitFC { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDateTime { get; set; }
    }
}