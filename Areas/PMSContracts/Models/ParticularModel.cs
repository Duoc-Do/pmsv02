namespace WebApp.Areas.PMSContracts.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.ComponentModel.DataAnnotations;

    [Table("CONTRACTS_CONDITIONS_PARTICULAR")]
    public partial class ParticularModel : DbContext
    {
        [Key]
        public long ParticularId { get; set; }

        public long? GeneralId { get; set; }

        public long? ParentId { get; set; }

        public string ClauseCode { get; set; }


        public string ClauseContent { get; set; }


        public string OriginalLanguage { get; set; }

        public DateTime? CreationDate { get; set; }


        public string Type { get; set; }
    }
}
