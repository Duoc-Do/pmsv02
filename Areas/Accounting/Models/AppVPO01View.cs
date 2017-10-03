﻿//------------------------------------------------------------------------------
// <auto-generated>
// gen by lotusviet.vn
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApp.Areas.Accounting.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("AppVPO01View")]
    public partial class AppVPO01View
    {
        public AppVPO01View()
        {
            this.AppVPO01LineViews = new HashSet<AppVPO01LineView>();
            this.AppVPO01VATViews = new HashSet<AppVPO01VATView>();

            this.VoucherDate = DateTime.Today;
            this.IsoCode = Services.Voucher.GetIsoCode();
            this.PostType = 0; // Trang thai chung tu 0: post tat ca vao so cai
            this.VoucherID = Services.Voucher.GetVoucherID("PO01");

            this.CurrencyID = Services.Voucher.GetCurrencyIDByIsoCode(this.IsoCode);
            this.ExchangeRate = Services.Voucher.GetExchangeRate(this.CurrencyID, this.VoucherDate);
            this.VoucherNumber = Services.Voucher.GetVoucherNumber(this.VoucherID);

        }
        public virtual ICollection<AppVPO01LineView> AppVPO01LineViews { get; set; }
        public virtual ICollection<AppVPO01VATView> AppVPO01VATViews { get; set; }

        public virtual ICollection<long> AppVPO01LineViewz { get; set; }
        public virtual ICollection<long> AppVPO01VATViewz { get; set; }


        [Key]
        public long DocumentID { get; set; }
        public Nullable<int> AccountCreditID { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public int CurrencyID { get; set; }
        public string CustomerCode { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string Description { get; set; }
        public string DisplayNumberCredit { get; set; }
        public Nullable<int> DueDate { get; set; }
        public decimal ExchangeRate { get; set; }
        public string IsoCode { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDateTime { get; set; }
        public Nullable<long> ParentID { get; set; }
        public Nullable<int> PostType { get; set; }
        public decimal SumAmount { get; set; }
        public decimal SumAmountCost { get; set; }
        public decimal SumAmountCostFC { get; set; }
        public decimal SumAmountFC { get; set; }
        public decimal SumAmountVAT { get; set; }
        public decimal SumAmountVATFC { get; set; }
        public decimal SumDiscount { get; set; }
        public decimal SumDiscountFC { get; set; }
        public decimal SumQuantity0 { get; set; }
        public Nullable<decimal> SumTotal { get; set; }
        public Nullable<decimal> SumTotalFC { get; set; }
        public string VoucherCode { get; set; }
        public System.DateTime VoucherDate { get; set; }
        public int VoucherID { get; set; }
        public string VoucherName { get; set; }
        public string VoucherNumber { get; set; }

    }
}