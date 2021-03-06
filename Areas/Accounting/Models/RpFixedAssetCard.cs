//------------------------------------------------------------------------------
// <auto-generated>
// gen by lotusviet.vn
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApp.Areas.Accounting.Models
{

    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("RpFixedAssetCard")]
    public partial class RpFixedAssetCard
    {
        public Nullable<int> AccountCreditID { get; set; }
        public Nullable<int> AccountDebitID { get; set; }
        public Nullable<int> AccountID { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public string DepartmentCode { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string DescriptionDecrease { get; set; }
        public string DescriptionSuspension { get; set; }
        public string DisplayNumber { get; set; }
        public string DisplayNumberCredit { get; set; }
        public string DisplayNumberDebit { get; set; }
        public string FixedAssetCode { get; set; }
        public string FixedAssetGroupCode { get; set; }
        public Nullable<int> FixedAssetGroupID { get; set; }
        public string FixedAssetGroupName { get; set; }
        public int FixedAssetID { get; set; }
        public string FixedAssetNumber { get; set; }
        public string FixedAssetTypeCode { get; set; }
        public Nullable<int> FixedAssetTypeID { get; set; }
        public string FixedAssetTypeName { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string MadeIn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDateTime { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public Nullable<int> PeriodOfDepreciation { get; set; }
        public string Power { get; set; }
        public Nullable<System.DateTime> SuspensionDate { get; set; }
        public Nullable<System.DateTime> VoucherDate { get; set; }
        public Nullable<System.DateTime> VoucherDateDecrease { get; set; }
        public string VoucherNumber { get; set; }
        public string VoucherNumberDecrease { get; set; }
        public Nullable<int> YearProduction { get; set; }
        public Nullable<int> YearUse { get; set; }

    }
}