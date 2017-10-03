using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Areas.PMSContracts.Models
{
    [Table("CONTRACTS_CLIENTS")]
    public class ClientModel
    {
        [Key]
        public int ClientID { get; set; }

        [StringLength(50)]
        public string ResName { get; set; }

        public int? Territory { get; set; }

        [StringLength(50)]
        public string AccountNumber { get; set; }

        [StringLength(50)]
        public string Bank { get; set; }

        [StringLength(100)]
        public string CompanyAddress { get; set; }

        [StringLength(50)]
        public string DeskPhone { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(1)]
        public string Type { get; set; }

        [StringLength(100)]
        public string CompanyName { get; set; }

        [StringLength(50)]
        public string Fax { get; set; }

        [StringLength(50)]
        public string HandPhone { get; set; }

        public bool? Status { get; set; }

        public DateTime ModifiedDate { get; set; }


    }
}