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
    [Table("AppVSO06View")]
    public partial class AppVSO06View
    {
        public AppVSO06View()
        {
            this.AppVSO06LineViews = new HashSet<AppVSO06LineView>();
            this.AppVSO06VATViews = new HashSet<AppVSO06VATView>();

            this.VoucherDate = DateTime.Today;
            this.IsoCode = Services.Voucher.GetIsoCode();
            this.PostType = 0; // Trang thai chung tu 0: post tat ca vao so cai
            this.VoucherID = Services.Voucher.GetVoucherID("SO06");

            this.CurrencyID = Services.Voucher.GetCurrencyIDByIsoCode(this.IsoCode);
            this.ExchangeRate = Services.Voucher.GetExchangeRate(this.CurrencyID, this.VoucherDate);
            this.VoucherNumber = Services.Voucher.GetVoucherNumber(this.VoucherID);

        }
        public virtual ICollection<AppVSO06LineView> AppVSO06LineViews { get; set; }
        public virtual ICollection<AppVSO06VATView> AppVSO06VATViews { get; set; }

        public virtual ICollection<long> AppVSO06LineViewz { get; set; }
        public virtual ICollection<long> AppVSO06VATViewz { get; set; }


        [Key]
        public long DocumentID { get; set; }
        public Nullable<int> AccountDebitID { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public int CurrencyID { get; set; }
        public string CustomerCode { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string Description { get; set; }
        public string DisplayNumberDebit { get; set; }
        public Nullable<int> DueDate { get; set; }
        public decimal ExchangeRate { get; set; }
        public Nullable<bool> IsFixPrice { get; set; }
        public string IsoCode { get; set; }
        public string ItemBarCode { get; set; }
        public string ItemBarCode2 { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDateTime { get; set; }
        public string NameCreatedBy { get; set; }
        public string NameModifiedBy { get; set; }
        public Nullable<long> ParentID { get; set; }
        public Nullable<int> PostType { get; set; }
        public decimal SumAmount { get; set; }
        public decimal SumAmountFC { get; set; }
        public decimal SumAmountSell { get; set; }
        public decimal SumAmountSellFC { get; set; }
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
