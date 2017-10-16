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

        public string TechSpecCode { get; set; }

       
        public string Description { get; set; }

       
        public string CreationBy { get; set; }

        
        public string ModifiedBy { get; set; }

       
        public string OriginalLanguage { get; set; }

        public DateTime? DateOfModifier { get; set; }

        public long? FileId { get; set; }

        public DateTime? DateOfCreation { get; set; }

        public long? ContractID { get; set; }
    }
}
