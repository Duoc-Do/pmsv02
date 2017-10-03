﻿//------------------------------------------------------------------------------
// <auto-generated>
// gen by lotusviet.vn
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApp.Models
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("SenUser")]
    public partial class SenUser
    {
        public SenUser()
        {
            this.SenUserProfiles = new HashSet<SenUserProfile>();
        }
        [Key]
        public int SenUserId { get; set; }
        [DisplayName("Địa chỉ")]
        public string Address { get; set; }
        public Nullable<decimal> AmountBalance { get; set; }
        [DisplayName("Hình đại diện")]
        public string Avatar { get; set; }

        [DisplayName("Tên đầy đủ")]
        public string FullName { get; set; }
        public Nullable<System.DateTime> LastPayment { get; set; }
        [DisplayName("Điện thoại")]
        public string Telephone { get; set; }
        public Guid UserId { get; set; }
        public virtual UserProfile UserProfile { get; set; }

        public virtual ICollection<SenUserProfile> SenUserProfiles { get; set; }

    }
}