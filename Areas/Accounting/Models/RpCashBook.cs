//------------------------------------------------------------------------------
// <auto-generated>
// gen by lotusviet.vn
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApp.Areas.Accounting.Models
{

    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("RpCashBook")]
    public partial class RpCashBook
    {
        public Nullable<int> AccountID { get; set; }
        public string AccountName { get; set; }
        public Nullable<decimal> Credit { get; set; }
        public Nullable<decimal> CreditFC { get; set; }
        public Nullable<decimal> CreditFCSBG { get; set; }
        public Nullable<decimal> CreditFCSEnd { get; set; }
        public Nullable<decimal> CreditSBG { get; set; }
        public Nullable<decimal> CreditSEnd { get; set; }
        public string CustomerCode { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public string CustomerName { get; set; }
        public Nullable<decimal> Debit { get; set; }
        public Nullable<decimal> DebitFC { get; set; }
        public Nullable<decimal> DebitFCSBG { get; set; }
        public Nullable<decimal> DebitFCSEnd { get; set; }
        public Nullable<decimal> DebitSBG { get; set; }
        public Nullable<decimal> DebitSEnd { get; set; }
        public string Description { get; set; }
        public string DisplayNumber { get; set; }
        public Nullable<long> DocumentID { get; set; }
        public string VoucherCode { get; set; }
        public Nullable<System.DateTime> VoucherDate { get; set; }
        public string VoucherName { get; set; }
        public string VoucherNumber { get; set; }

    }
}