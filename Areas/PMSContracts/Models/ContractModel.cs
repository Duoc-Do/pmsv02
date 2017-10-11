using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Areas.PMSContracts.Models
{
    [Table("CONTRACTS")]
    public class ContractModel
    {
        [Key]
        [Display(Name = "Contract ID")]
        public long ContractID { get; set; }

        [Display(Name = "IDERP")]
        [StringLength(10)]
        public string ContractIDERP { get; set; }

        [Display(Name = "CCE ID")]
        [StringLength(3)]
        public string CCE_ID { get; set; }

        [Display(Name = "Contract Code")]
        [StringLength(50)]
        public string ContractCode { get; set; }

        [Display(Name = "Contract Date")]
        [Column(TypeName = "date")]
        public DateTime? ContractDate { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Commencement Date")]
        public DateTime? CommencementDate { get; set; }

        [StringLength(255)]
        [Display(Name = "Description EN")]
        public string Description { get; set; }

        [StringLength(255)]
        [Display(Name = "Description VN")]
        public string Description_VN { get; set; }

        [Display(Name = "FC Amount")]
        public byte? ContractFormat { get; set; }

        [Column(TypeName = "money")]
        public decimal? FCAmount { get; set; }

        [Column(TypeName = "money")]
        [Display(Name = "LC Amount")]
        public decimal? LCAmount { get; set; }

        [Column(TypeName = "money")]
        [Display(Name = "FC Amount Extension")]
        public decimal? FCAmountExtension { get; set; }

        [Column(TypeName = "money")]
        [Display(Name = "LG Amount Extension")]
        public decimal? LCAmountExtension { get; set; }

        [Display(Name = "Client ID")]
        public int? ClientID { get; set; }

        [Display(Name = "Contractor ID")]
        public int? ContractorID { get; set; }

        [Display(Name = "Duration In Days")]
        public int? DurationInDays { get; set; }

        [Display(Name = "Extension In Days")]
        public int? ExtensionInDays { get; set; }
        [Display(Name ="Modify Date")]
        public DateTime? ModifyDate { get; set; }

        [StringLength(10)]
        [Display(Name = "Currency Code")]
        public string CurrencyCode { get; set; }

        [StringLength(2)]
        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "For Ordering")]
        public byte? ForOrdering { get; set; }

        [StringLength(30)]
        public string Type { get; set; }
        [Display(Name = "Project ID")]
        public long? proj_id { get; set; }
        [StringLength(1)]
        [Display(Name = "Project Status")]
        public string proj_status { get; set; }
        [ForeignKey("ClientID")]
        public virtual ClientModel ClientModels { get; set; }
    }
}