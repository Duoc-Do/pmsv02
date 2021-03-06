﻿
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
    [Table("AppDocumentTable")]
    public partial class AppDocumentTable
    {
        [Key]
        public long DocumentID { get; set; }
        public Nullable<int> AccountCreditID { get; set; }
        public Nullable<int> AccountDebitID { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public int CurrencyID { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public string Description { get; set; }
        public Nullable<int> DueDate { get; set; }
        public decimal ExchangeRate { get; set; }
        public Nullable<bool> IsFixPrice { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDateTime { get; set; }
        public Nullable<long> ParentID { get; set; }
        public Nullable<int> PostType { get; set; }
        public System.DateTime VoucherDate { get; set; }
        public int VoucherID { get; set; }
        public string VoucherNumber { get; set; }
        public Nullable<int> WarehouseID { get; set; }

    }
}
