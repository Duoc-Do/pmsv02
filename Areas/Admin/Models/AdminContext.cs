using System.Data.Entity;

namespace WebApp.Areas.Admin.Models
{
    public class AdminContext : DbContext
    {
        public AdminContext()
            : base("DefaultConnection")
        {
        }

        #region các bảng hệ thống của asp.net
        public DbSet<aspnet_Roles> aspnet_Roles { get; set; }
        public DbSet<aspnet_Applications> aspnet_Applications { get; set; }
        public DbSet<aspnet_Users> aspnet_Users { get; set; }
        public DbSet<aspnet_Membership> aspnet_Memberships { get; set; }
        public DbSet<aspnet_UsersInRoles> aspnet_UsersInRoles { get; set; }

        public DbSet<vw_aspnet_MembershipUsers> vw_aspnet_MembershipUsers { get; set; }
        #endregion


        #region các bảng hệ thống của sen việt
        public DbSet<SenCommission> SenCommissions { get; set; }
        public DbSet<SenContractCash> SenContractCashs { get; set; }

        public DbSet<SenChatMessage> SenChatMessages { get; set; }

        public DbSet<SenProduct> SenProducts { get; set; }
        public DbSet<SenCustomer> SenCustomers { get; set; }
        public DbSet<SenContract> SenContracts { get; set; }

        public DbSet<SenGiftCard> SenGiftCards { get; set; }

        public DbSet<SysColumn> SysColumns { get; set; }
        public DbSet<SysTable> SysTables { get; set; }
        public DbSet<SysTableDetail> SysTableDetails { get; set; }
        public DbSet<SysTableDetailView> SysTableDetailViews { get; set; }
        public DbSet<SysOption> SysOptions { get; set; }

        public DbSet<aspnet_UsersInRolesView> aspnet_UsersInRolesViews { get; set; }
        public DbSet<SenApplication> SenApplications { get; set; }
        public DbSet<SenCompany> SenCompanys { get; set; }
        public DbSet<SenCompanyView> SenCompanyViews { get; set; }

        public DbSet<SenService> SenServices { get; set; }
        public DbSet<SenServiceView> SenServiceViews { get; set; }
        public DbSet<SenScheduleTask> SenScheduleTasks { get; set; }
        public DbSet<SenEmailAccount> SenEmailAccounts { get; set; }

        public DbSet<SenQueuedEmail> SenQueuedEmails { get; set; }
        public DbSet<SenLog> SenLogs { get; set; }
        public DbSet<SenMessageTemplate> SenMessageTemplates { get; set; }

        public DbSet<SenPackage> SenPackages { get; set; }
        public DbSet<SenPackageView> SenPackageViews { get; set; }

        public DbSet<SenPackageLine> SenPackageLines { get; set; }
        public DbSet<SenPackageLineView> SenPackageLineViews { get; set; }


        public DbSet<SenPrivateMessage> SenPrivateMessages { get; set; }

        public DbSet<SenPrivateMessageView> SenPrivateMessageViews { get; set; }
        public DbSet<SenUser> SenUsers { get; set; }
        public DbSet<SenUserView> SenUserViews { get; set; }

        public DbSet<SenUserCash> SenUserCashs { get; set; }
        public DbSet<SenUserCashView> SenUserCashViews { get; set; }

        public DbSet<SenUserPayment> SenUserPayments { get; set; }
        public DbSet<SenUserPaymentView> SenUserPaymentViews { get; set; }

        public DbSet<SenUserTransaction> SenUserTransactions { get; set; }
        #endregion


    }
}