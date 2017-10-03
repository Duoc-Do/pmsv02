
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
    [Table("GapJournalHarvest")]
    public partial class GapJournalHarvest
    {
        [Key]
        public int JournalHarvestId { get; set; }
        public System.DateTime JournalHarvestDate { get; set; }
        public int JournalId { get; set; }
        public string Note { get; set; }
        public decimal Quantity { get; set; }

        public Nullable<int> UOMID { get; set; }

        public Nullable<System.DateTime> RefIsolationDate { get; set; }
        public Nullable<int> RefIsolationDay { get; set; }

        public virtual AppUnitOfMeasureTable AppUnitOfMeasureTable { get; set; }
    }
}