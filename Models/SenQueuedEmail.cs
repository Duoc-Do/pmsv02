﻿//------------------------------------------------------------------------------
// <auto-generated>
// gen by lotusviet.vn
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApp.Models
{

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("SenQueuedEmail")]
    public partial class SenQueuedEmail
    {
        [Key]
        public int QueuedEmailId { get; set; }
        public string Bcc { get; set; }
        public string Body { get; set; }
        public string CC { get; set; }
        public System.DateTime CreatedOnUtc { get; set; }
        public int EmailAccountId { get; set; }
        public string From { get; set; }
        public string FromName { get; set; }
        public int Priority { get; set; }
        public Nullable<System.DateTime> SentOnUtc { get; set; }
        public int SentTries { get; set; }
        public string Subject { get; set; }
        public string To { get; set; }
        public string ToName { get; set; }
    }
}