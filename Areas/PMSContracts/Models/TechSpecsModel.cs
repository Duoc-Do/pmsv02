namespace WebApp.Areas.PMSContracts.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("CONTRACTS_CONDITIONS_TECHSPECS")]
    public partial class TechSpecsModel
    {
        [Key]
        public long TechSpecID { get; set; }

        [StringLength(100)]
        public string TechSpecCode { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(30)]
        public string CreationBy { get; set; }

        [StringLength(30)]
        public string ModifiedBy { get; set; }

        [StringLength(2)]
        public string OriginalLanguage { get; set; }

        public DateTime? DateOfModifier { get; set; }

        public long? FileId { get; set; }

        public DateTime? DateOfCreation { get; set; }

        public int? ContractID { get; set; }
    }
}
