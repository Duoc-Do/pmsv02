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
    [Table("AppDocumentLineTable")]
    public partial class AppDocumentLineTable
    {
        [Key]
        public long DocumentLineID { get; set; }
        public Nullable<int> AccountCreditLine1ID { get; set; }
        public Nullable<int> AccountCreditLineID { get; set; }
        public Nullable<int> AccountDebitLine1ID { get; set; }
        public Nullable<int> AccountDebitLineID { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> AmountCost { get; set; }
        public Nullable<decimal> AmountCostFC { get; set; }
        public Nullable<decimal> AmountFC { get; set; }
        public Nullable<decimal> AmountSell { get; set; }
        public Nullable<decimal> AmountSellFC { get; set; }
        public Nullable<decimal> Credit { get; set; }
        public Nullable<decimal> CreditFC { get; set; }
        public Nullable<decimal> Debit { get; set; }
        public Nullable<decimal> DebitFC { get; set; }
        public string DescriptionLine { get; set; }
        public Nullable<decimal> DisCount { get; set; }
        public Nullable<decimal> DisCountFC { get; set; }
        public Nullable<decimal> DisCountPercentage { get; set; }
        public long DocumentID { get; set; }
        public int DocumentLineType { get; set; }
        public Nullable<decimal> Duty { get; set; }
        public Nullable<decimal> DutyFC { get; set; }
        public Nullable<decimal> DutyPercentage { get; set; }
        public Nullable<decimal> ExchangeRateLine { get; set; }
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
        public Nullable<int> ExObject01ID { get; set; }
        public Nullable<int> ExObject02ID { get; set; }
        public Nullable<int> ExObject03ID { get; set; }
        public Nullable<int> ExObject04ID { get; set; }
        public Nullable<int> ExObject05ID { get; set; }
        public Nullable<int> ExObject06ID { get; set; }
        public Nullable<int> ExObject07ID { get; set; }
        public Nullable<int> ExObject08ID { get; set; }
        public Nullable<int> ExObject09ID { get; set; }

        public Nullable<int> ExObject10ID { get; set; }
        public Nullable<int> ExObject11ID { get; set; }
        public Nullable<int> ExObject12ID { get; set; }
        public Nullable<int> ExObject13ID { get; set; }
        public Nullable<int> ExObject14ID { get; set; }
        public Nullable<int> ExObject15ID { get; set; }
        public Nullable<int> ExObject16ID { get; set; }
        public Nullable<int> ExObject17ID { get; set; }
        public Nullable<int> ExObject18ID { get; set; }
        public Nullable<int> ExObject19ID { get; set; }
        public Nullable<int> ExObject20ID { get; set; }
        public string ExString01 { get; set; }
        public string ExString02 { get; set; }
        public string ExString03 { get; set; }
        public string ExString04 { get; set; }
        public string ExString05 { get; set; }
        public string ExString06 { get; set; }
        public Nullable<int> ItemID { get; set; }
        public Nullable<decimal> MeasureRate { get; set; }
        public Nullable<long> ParentLineID { get; set; }
        public Nullable<int> ParentLineType { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> Quantity0 { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> UnitPriceFC { get; set; }
        public Nullable<decimal> UnitPriceSell { get; set; }
        public Nullable<decimal> UnitPriceSellFC { get; set; }
        public Nullable<int> UOMID { get; set; }
        public Nullable<int> WarehouseLineID { get; set; }
    }
}