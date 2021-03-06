//------------------------------------------------------------------------------
// <auto-generated>
// gen by lotusviet.vn
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApp.Areas.Accounting.Models
{

    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("RpCashFlowInDirect")]
    public partial class RpCashFlowInDirect
    {
        public Nullable<decimal> AmountBG { get; set; }
        public Nullable<decimal> AmountEnd { get; set; }
        public Nullable<decimal> AmountFCBG { get; set; }
        public Nullable<decimal> AmountFCEnd { get; set; }
        public string DisplayNumberCredit { get; set; }
        public string DisplayNumberDebit { get; set; }
        public string Explanation { get; set; }
        public string Formula { get; set; }
        public string FormulaOrder { get; set; }
        public Nullable<bool> IsBold { get; set; }
        public Nullable<bool> IsPrint { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int ReportID { get; set; }
        public Nullable<int> ReportOrder { get; set; }

    }
}