//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApp.Areas.Accounting.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("AppCustomerBalanceTable")]
    public partial class AppCustomerBalanceTable
    {
        [Key]
        public int CustomerBalanceID { get; set; }
        public System.DateTime BalanceDate { get; set; }
        public int CustomerID { get; set; }
        public int AccountID { get; set; }
        public Nullable<decimal> Debit { get; set; }
        public Nullable<decimal> Credit { get; set; }
        public Nullable<decimal> DebitFC { get; set; }
        public Nullable<decimal> CreditFC { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDateTime { get; set; }
    
        //public virtual AppAccountTable AppAccountTable { get; set; }
        //public virtual AppCustomerTable AppCustomerTable { get; set; }
    }
}