﻿
//------------------------------------------------------------------------------
// <auto-generated>
// gen by lotusviet.vn
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApp.Areas.Admin.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("SenMessageTemplate")]
    public partial class SenMessageTemplate
    {
        [Key]
        public int MessageTemplateId { get; set; }
        public string BccEmailAddresses { get; set; }
        public string Body { get; set; }
        public int EmailAccountId { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }

    }
}