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
    [Table("AppVIN03View")]
    public partial class AppVIN03View
    {
        public AppVIN03View()
        {
            this.AppVIN03LineViews = new HashSet<AppVIN03LineView>();
            //this.AppVIN03VATViews = new HashSet<AppVIN03VATView>();

            this.VoucherDate = DateTime.Today;
            this.IsoCode = Services.Voucher.GetIsoCode();
            this.PostType = 0; // Trang thai chung tu 0: post tat ca vao so cai
            this.VoucherID = Services.Voucher.GetVoucherID("IN03");

            this.CurrencyID = Services.Voucher.GetCurrencyIDByIsoCode(this.IsoCode);
            this.ExchangeRate = Services.Voucher.GetExchangeRate(this.CurrencyID, this.VoucherDate);
            this.VoucherNumber = Services.Voucher.GetVoucherNumber(this.VoucherID);

        }
        public virtual ICollection<AppVIN03LineView> AppVIN03LineViews { get; set; }
        //public virtual ICollection<AppVIN03VATView> AppVIN03VATViews { get; set; }

        public virtual ICollection<long> AppVIN03LineViewz { get; set; }
        public virtual ICollection<long> AppVIN03VATViewz { get; set; }


        [Key]
        public long DocumentID { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public int CurrencyID { get; set; }
        public string CustomerCode { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string Description { get; set; }
        public decimal ExchangeRate { get; set; }
        public Nullable<bool> IsFixPrice { get; set; }
        public string IsoCode { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDateTime { get; set; }
        public Nullable<long> ParentID { get; set; }
        public Nullable<int> PostType { get; set; }
        public Nullable<decimal> SumAmount { get; set; }
        public Nullable<decimal> SumAmountFC { get; set; }
        public Nullable<decimal> SumQuantity0 { get; set; }
        public string VoucherCode { get; set; }
        public System.DateTime VoucherDate { get; set; }
        public int VoucherID { get; set; }
        public string VoucherName { get; set; }
        public string VoucherNumber { get; set; }

    }
}
