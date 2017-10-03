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
    [Table("AppVSO06VATView")]
    public partial class AppVSO06VATView
    {
        public AppVSO06VATView()
        {
            this.AmountVAT = 0;
            this.AmountVATFC = 0;
            this.DocumentVATType = 2; // 1: mua vao
        }

        [Key]
        public long DocumentVATID { get; set; }
        public Nullable<int> AccountCreditLineID { get; set; }
        public Nullable<int> AccountDebitLineID { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> AmountFC { get; set; }
        public Nullable<decimal> AmountVAT { get; set; }
        public Nullable<decimal> AmountVATFC { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerName { get; set; }
        public string DescriptionVAT { get; set; }
        public string DisplayNumberLineCredit { get; set; }
        public string DisplayNumberLineDebit { get; set; }
        public long DocumentID { get; set; }
        public int DocumentVATType { get; set; }
        public Nullable<System.DateTime> ExDate01 { get; set; }
        public Nullable<System.DateTime> ExDate02 { get; set; }
        public Nullable<System.DateTime> ExDate03 { get; set; }
        public Nullable<System.DateTime> ExDate04 { get; set; }
        public Nullable<System.DateTime> ExDate05 { get; set; }
        public Nullable<System.DateTime> ExDate06 { get; set; }
        public Nullable<decimal> ExNumeric01 { get; set; }
        public Nullable<decimal> ExNumeric02 { get; set; }
        public Nullable<decimal> ExNumeric03 { get; set; }
        public Nullable<decimal> ExNumeric04 { get; set; }
        public Nullable<decimal> ExNumeric05 { get; set; }
        public Nullable<decimal> ExNumeric06 { get; set; }
        public Nullable<decimal> ExNumeric07 { get; set; }
        public Nullable<decimal> ExNumeric08 { get; set; }
        public Nullable<decimal> ExNumeric09 { get; set; }
        public Nullable<decimal> ExNumeric10 { get; set; }
        public string ExObject01Code { get; set; }
        public Nullable<int> ExObject01ID { get; set; }
        public string ExObject01Name { get; set; }
        public string ExObject02Code { get; set; }
        public Nullable<int> ExObject02ID { get; set; }
        public string ExObject02Name { get; set; }
        public string ExObject03Code { get; set; }
        public Nullable<int> ExObject03ID { get; set; }
        public string ExObject03Name { get; set; }
        public string ExObject04Code { get; set; }
        public Nullable<int> ExObject04ID { get; set; }
        public string ExObject04Name { get; set; }
        public string ExObject05Code { get; set; }
        public Nullable<int> ExObject05ID { get; set; }
        public string ExObject05Name { get; set; }
        public string ExObject06Code { get; set; }
        public Nullable<int> ExObject06ID { get; set; }
        public string ExObject06Name { get; set; }
        public string ExObject07Code { get; set; }
        public Nullable<int> ExObject07ID { get; set; }

        public string ExObject07Name { get; set; }
        public string ExObject08Code { get; set; }
        public Nullable<int> ExObject08ID { get; set; }
        public string ExObject08Name { get; set; }
        public string ExObject09Code { get; set; }
        public Nullable<int> ExObject09ID { get; set; }
        public string ExObject09Name { get; set; }
        public string ExObject10Code { get; set; }
        public Nullable<int> ExObject10ID { get; set; }
        public string ExObject10Name { get; set; }
        public string ExObject11Code { get; set; }
        public Nullable<int> ExObject11ID { get; set; }
        public string ExObject11Name { get; set; }
        public string ExObject12Code { get; set; }
        public Nullable<int> ExObject12ID { get; set; }
        public string ExObject12Name { get; set; }
        public string ExObject13Code { get; set; }
        public Nullable<int> ExObject13ID { get; set; }
        public string ExObject13Name { get; set; }
        public string ExObject14Code { get; set; }
        public Nullable<int> ExObject14ID { get; set; }
        public string ExObject14Name { get; set; }
        public string ExObject15Code { get; set; }
        public Nullable<int> ExObject15ID { get; set; }
        public string ExObject15Name { get; set; }
        public string ExObject16Code { get; set; }
        public Nullable<int> ExObject16ID { get; set; }
        public string ExObject16Name { get; set; }
        public string ExObject17Code { get; set; }
        public Nullable<int> ExObject17ID { get; set; }
        public string ExObject17Name { get; set; }
        public string ExObject18Code { get; set; }
        public Nullable<int> ExObject18ID { get; set; }
        public string ExObject18Name { get; set; }
        public string ExObject19Code { get; set; }
        public Nullable<int> ExObject19ID { get; set; }
        public string ExObject19Name { get; set; }
        public string ExObject20Code { get; set; }
        public Nullable<int> ExObject20ID { get; set; }
        public string ExObject20Name { get; set; }
        public string ExString01 { get; set; }
        public string ExString02 { get; set; }
        public string ExString03 { get; set; }
        public string ExString04 { get; set; }
        public string ExString05 { get; set; }
        public string ExString06 { get; set; }
        public string ItemName { get; set; }
        public Nullable<decimal> Percentage { get; set; }
        public string ProductCode { get; set; }
        public Nullable<int> ProductID { get; set; }
        public string ProductName { get; set; }
        public string SalesTaxCode { get; set; }
        public Nullable<int> SalesTaxID { get; set; }
        public string SalesTaxName { get; set; }
        public string TaxCode { get; set; }
        public Nullable<System.DateTime> VATDate { get; set; }
        public string VATNumber { get; set; }
        public string VATSerial { get; set; }
        public string VATTemplate { get; set; }
    }
}
