﻿//------------------------------------------------------------------------------
// <auto-generated>
// gen by lotusviet.vn
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApp.Areas.Admin.Models
{

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("SenUser")]
    public partial class SenUser
    {
        [Key]
        public int SenUserId { get; set; }
        public string Address { get; set; }
        public Nullable<decimal> AmountBalance { get; set; }
        public string Avatar { get; set; }
        public string FullName { get; set; }
        public Nullable<System.DateTime> LastPayment { get; set; }
        public string Telephone { get; set; }

        //[ForeignKey("Membership")]
        public Guid UserId { get; set; }

        //public virtual aspnet_Membership Membership { get; set; }
    }
}