using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace WebApp.Models
{
    public class SenContext : DbContext
    {
        public SenContext()
            : base("DefaultConnection")
        {
        }

        //#region các bảng hệ thống của asp.net
        //public DbSet<aspnet_Roles> aspnet_Roles { get; set; }
        //#endregion
        //#region các bảng hệ thống của senviệt
        //public DbSet<SysTableDetail> SysTableDetails { get; set; }
        //public DbSet<SysTableDetailView> SysTableDetailViews { get; set; }
        //#endregion
        public DbSet<Language> Languages { get; set; }

        public DbSet<SenUserProfile> SenUserProfiles { get; set; }

        public DbSet<SenChatMessage> SenChatMessages { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<SenPackage> SenPackages { get; set; }
        public DbSet<SenPackageLine> SenPackageLines { get; set; }

        public DbSet<SenCompany> SenCompanys { get; set; }
        public DbSet<SenCompanyView> SenCompanyViews { get; set; }

        public DbSet<SenApplication> SenApplications { get; set; }
        public DbSet<SenService> SenServices { get; set; }
        public DbSet<SenServiceView> SenServiceViews { get; set; }

        public DbSet<SenScheduleTask> SenScheduleTasks { get; set; }
        public DbSet<SenLog> SenLogs { get; set; }

        public DbSet<SenQueuedEmail> SenQueuedEmails { get; set; }

        public DbSet<SenEmailAccount> SenEmailAccounts { get; set; }

        public DbSet<SenMessageTemplate> SenMessageTemplates { get; set; }

        public DbSet<SenPrivateMessage> SenPrivateMessages { get; set; }

        public DbSet<SysTableDetail> SysTableDetails { get; set; }
        public DbSet<SysTableDetailView> SysTableDetailViews { get; set; }

        public DbSet<SysOption> SysOptions { get; set; }
        public DbSet<SenUser> SenUsers { get; set; }


        public DbSet<SenUserTransaction> SenUserTransactions { get; set; }

    }

    [Table("aspnet_Users")]
    public class UserProfile
    {
        [Key]
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        //public virtual SenUser SenUser { get; set; }
    }

    [Table("SenService")]
    public class SenService
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ServiceId { get; set; }

        public Guid UserId { get; set; }
        public int CompanyId { get; set; }

        public virtual UserProfile UserProfile { get; set; }
        public virtual SenCompany SenCompany { get; set; }


    }

    [Table("SenApplication")]
    public class SenApplication
    {
        public SenApplication()
        {
            this.SenPackages = new HashSet<SenPackage>();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ApplicationId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<SenPackage> SenPackages { get; set; }
    }




    [Table("SenPackage")]
    public partial class SenPackage
    {
        [Key]
        public int PackageId { get; set; }
        public int ApplicationId { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public virtual SenApplication SenApplication { get; set; }
    }

    [Table("SenPackageLine")]
    public partial class SenPackageLine
    {
        [Key]
        public int PackageLineId { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public int PackageId { get; set; }
        public string Value { get; set; }

        public virtual SenPackage SenPackage { get; set; }
    }

    [Table("SenCompany")]
    public partial class SenCompany
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Phải nhập {0}.")]
        [DisplayName("Địa chỉ")]
        public string Address { get; set; }

        [DisplayName("Ghi chú")]
        public string Comment { get; set; }
        public string ConnectionString { get; set; }

        [DisplayName("Ngày đăng ký")]
        public Nullable<System.DateTime> CreateDate { get; set; }

        [DisplayName("Hạn sử dụng")]
        public Nullable<System.DateTime> ExpireDate { get; set; }

        [DisplayName("Đăng nhập sau cùng")]
        public Nullable<System.DateTime> LastLoginDate { get; set; }

        [DisplayName("Mô tả")]
        public string Description { get; set; }

        public string Email { get; set; }

        [DisplayName("Fax")]
        public string FaxNumber { get; set; }

        [DisplayName("Duyệt")]
        public Nullable<bool> IsApproved { get; set; }

        [DisplayName("Bị khóa")]
        public Nullable<bool> IsLockedOut { get; set; }

        [DisplayName("Logo")]
        public string Logo { get; set; }

        [Required(ErrorMessage = "Phải nhập {0}.")]
        [DisplayName("Tên")]
        public string Name { get; set; }

        [DisplayName("Gói ứng dụng")]
        public Nullable<int> PackageId { get; set; }

        [Required(ErrorMessage = "Phải nhập {0}.")]
        [DisplayName("Mã số thuế")]
        public string TaxCode { get; set; }

        [Required(ErrorMessage = "Phải nhập {0}.")]
        [DisplayName("Điện thoại")]
        public string Telephone { get; set; }
        public Nullable<Guid> UserId { get; set; }
        public string Website { get; set; }

        public virtual UserProfile UserProfile { get; set; }
        public virtual SenPackage SenPackage { get; set; }

    }
}