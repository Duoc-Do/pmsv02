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

    [Table("SysReport")]
    public partial class SysReport
    {
        [Key]
        public int SysReportID { get; set; }
        public string BusinessCode { get; set; }
        public string TableName { get; set; }
        public string ReportName { get; set; }
        public string ReportPath { get; set; }
        public string Des { get; set; }
        public string Title { get; set; }
        public Nullable<bool> IsDefault { get; set; }
        public string ProcedureName { get; set; }
    
        //public virtual SysBusiness SysBusiness { get; set; }
    }
}
