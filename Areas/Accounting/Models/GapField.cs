
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
    [Table("GapField")]
    public partial class GapField
    {
        public GapField()
        {
            this.GapRows = new HashSet<GapRow>();
        }

        public virtual ICollection<GapRow> GapRows { get; set; }
        //public virtual GapFarm GapFarm { get; set; }
     
        //public virtual GapJournal GapJournal { get; set; }

        [Key]
        public int FieldId { get; set; }

        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public int FarmId { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDateTime { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public Nullable<int> Order { get; set; }

        public Nullable<int> RefJournalId { get; set; }
    }
}