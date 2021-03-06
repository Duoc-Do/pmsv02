﻿
//------------------------------------------------------------------------------
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
    [Table("AppVPO05View")]
    public partial class AppVPO05View
    {
        public AppVPO05View()
        {
            this.AppVPO05LineViews = new HashSet<AppVPO05LineView>();
            this.AppVPO05VATViews = new HashSet<AppVPO05VATView>();

            this.VoucherDate = DateTime.Today;
            this.IsoCode = Services.Voucher.GetIsoCode();
            this.PostType = 0; // Trang thai chung tu 0: post tat ca vao so cai
            this.VoucherID = Services.Voucher.GetVoucherID("PO05");

            this.CurrencyID = Services.Voucher.GetCurrencyIDByIsoCode(this.IsoCode);
            this.ExchangeRate = Services.Voucher.GetExchangeRate(this.CurrencyID, this.VoucherDate);
            this.VoucherNumber = Services.Voucher.GetVoucherNumber(this.VoucherID);

        }
        public virtual ICollection<AppVPO05LineView> AppVPO05LineViews { get; set; }
        public virtual ICollection<AppVPO05VATView> AppVPO05VATViews { get; set; }

        public virtual ICollection<long> AppVPO05LineViewz { get; set; }
        public virtual ICollection<long> AppVPO05VATViewz { get; set; }



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
        public Nullable<System.DateTime> ExMDate01 { get; set; }
        public Nullable<System.DateTime> ExMDate02 { get; set; }
        public Nullable<System.DateTime> ExMDate03 { get; set; }
        public Nullable<System.DateTime> ExMDate04 { get; set; }
        public Nullable<System.DateTime> ExMDate05 { get; set; }
        public Nullable<System.DateTime> ExMDate06 { get; set; }
        public Nullable<decimal> ExMNumeric01 { get; set; }
        public Nullable<decimal> ExMNumeric02 { get; set; }
        public Nullable<decimal> ExMNumeric03 { get; set; }
        public Nullable<decimal> ExMNumeric04 { get; set; }
        public Nullable<decimal> ExMNumeric05 { get; set; }
        public Nullable<decimal> ExMNumeric06 { get; set; }
        public Nullable<decimal> ExMNumeric07 { get; set; }
        public Nullable<decimal> ExMNumeric08 { get; set; }
        public Nullable<decimal> ExMNumeric09 { get; set; }
        public Nullable<decimal> ExMNumeric10 { get; set; }
        public Nullable<int> ExMObject01ID { get; set; }
        public Nullable<int> ExMObject02ID { get; set; }
        public Nullable<int> ExMObject03ID { get; set; }
        public Nullable<int> ExMObject04ID { get; set; }
        public Nullable<int> ExMObject05ID { get; set; }
        public Nullable<int> ExMObject06ID { get; set; }
        public Nullable<int> ExMObject07ID { get; set; }
        public Nullable<int> ExMObject08ID { get; set; }
        public Nullable<int> ExMObject09ID { get; set; }
        public Nullable<int> ExMObject10ID { get; set; }
        public Nullable<int> ExMObject11ID { get; set; }
        public Nullable<int> ExMObject12ID { get; set; }
        public Nullable<int> ExMObject13ID { get; set; }
        public Nullable<int> ExMObject14ID { get; set; }
        public Nullable<int> ExMObject15ID { get; set; }
        public Nullable<int> ExMObject16ID { get; set; }
        public Nullable<int> ExMObject17ID { get; set; }
        public Nullable<int> ExMObject18ID { get; set; }
        public Nullable<int> ExMObject19ID { get; set; }
        public Nullable<int> ExMObject20ID { get; set; }

        public string ExMString01 { get; set; }
        public string ExMString02 { get; set; }
        public string ExMString03 { get; set; }
        public string ExMString04 { get; set; }
        public string ExMString05 { get; set; }
        public string ExMString06 { get; set; }
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
        public decimal SumDuty { get; set; }
        public decimal SumDutyFC { get; set; }
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