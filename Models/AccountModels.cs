//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Globalization;
//using System.Web.Mvc;
//using System.Web.Security;
//using System.ComponentModel;

using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Security;
using System.ComponentModel;

namespace WebApp.Models
{

    #region Models

    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "Phải nhập {0}.")]
        //[DisplayName("Tên đăng nhập")]
        [DisplayName("Tên người dùng")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Phải nhập {0}.")]
        //[DisplayName("Password answer")]
        [DisplayName("Trả lời mật khẩu")]
        public string PasswordAnswer { get; set; }

    }

    //public class ProfileModel
    //{
    //    [Required(ErrorMessage = "Phải nhập {0}.")]
    //    [DisplayName("Tên đầy đủ")]
    //    public string FullName { get; set; }
    //    public Guid UserId { get; set; }
    //}


    public class ForgotPasswordModel
    {
        //[Required(ErrorMessage = "Phải nhập {0}.")]
        //[DisplayName("Tên đăng nhập")]
        [DisplayName("Tên người dùng")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Phải nhập {0}.")]
        [DataType(DataType.EmailAddress)]
        //[DisplayName("Email address")]
        [RegularExpression(@"^[a-z|0-9|][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$", ErrorMessage = "Địa chỉ email không hợp lệ.")]
        [DisplayName("Địa chỉ email")]
        public string Email { get; set; }
    }


    //public class RoleModel
    //{

    //    [Required(ErrorMessage = "Phải nhập {0}.")]
    //    [DisplayName("ID ứng dụng")]
    //    public Guid ApplicationId { get; set; }

    //    [Required(ErrorMessage = "Phải nhập {0}.")]
    //    [DisplayName("ID quyền")]
    //    public Guid RoleId { get; set; }

    //    [Required(ErrorMessage = "Phải nhập {0}.")]
    //    [DisplayName("Tên quyền")]
    //    public string RoleName { get; set; }

    //    [Required(ErrorMessage = "Phải nhập {0}.")]
    //    [DisplayName("Ghi chú")]
    //    public string Description { get; set; }
    //}


    //[PropertiesMustMatch("NewPassword", "ConfirmPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    [PropertiesMustMatch("NewPassword", "ConfirmPassword", ErrorMessage = "Mật khẩu mới và xác nhận mật khẩu mới không trùng khớp.")]
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Phải nhập {0}.")]
        [DataType(DataType.Password)]
        //[DisplayName("Current password")]
        [DisplayName("Mật khẩu hiện hành")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Phải nhập {0}.")]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        //[DisplayName("New password")]
        [DisplayName("Mật khẩu mới")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Phải nhập {0}.")]
        [DataType(DataType.Password)]
        //[DisplayName("Confirm new password")]
        [DisplayName("Xác nhận mật khẩu mới")]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel
    {
        //[Required(ErrorMessage = "Phải nhập {0}.")]
        [Required(ErrorMessage = "Phải nhập {0}.")]
        [DisplayName("Tên đăng nhập")]
        //[DisplayName("Tên đăng nhập")]
        public string UserName { get; set; }

        //[Required(ErrorMessage = "Phải nhập {0}.")]
        [Required(ErrorMessage = "Phải nhập {0}.")]
        [DataType(DataType.Password)]
        [DisplayName("Mật khẩu")]
        //[DisplayName("Mật khẩu")]
        public string Password { get; set; }

        [DisplayName("Ghi nhớ?")]
        //[DisplayName("Ghi nhớ?")]
        public bool RememberMe { get; set; }
    }

    [PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không trùng khớp.")]
    //[PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessage = "The password and confirmation password do not match.")]
    //[PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessage = "The password and confirmation password do not match.")]
    public class RegisterModel
    {
        //[Required(ErrorMessage = "Phải nhập {0}.")]
        //[DisplayName("Tên đăng nhập")]
        [DisplayName("Tên đăng nhập")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Phải nhập {0}.")]
        [DataType(DataType.EmailAddress)]
        //[RegularExpression(@"^[a-z|0-9|][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$", ErrorMessage = "The password and confirmation password do not match.")]
        [RegularExpression(@"^[a-z|0-9|][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$", ErrorMessage = "Địa chỉ email không hợp lệ.")]
        //[DisplayName("Email address")]
        [DisplayName("Địa chỉ email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phải nhập {0}.")]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        //[DisplayName("Mật khẩu")]
        [DisplayName("Mật khẩu")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Phải nhập {0}.")]
        [DataType(DataType.Password)]
        //[DisplayName("Confirm password")]
        [DisplayName("Xác nhận mật khẩu")]
        public string ConfirmPassword { get; set; }
    }
    #endregion

    #region Services
    // The FormsAuthentication type is sealed and contains static members, so it is difficult to
    // unit test code that calls its members. The interface and helper class below demonstrate
    // how to create an abstract wrapper around such a type in order to make the AccountController
    // code unit testable.

    public interface IMembershipService
    {
        int MinPasswordLength { get; }
        bool ValidateUser(string userName, string password);
        //bool Login(string providerName, string providerUserId,string createPersistentCookie);

        MembershipCreateStatus CreateUser(string userName, string password, string email);
        bool ChangePassword(string userName, string oldPassword, string newPassword);


    }

    public class AccountMembershipService : IMembershipService
    {
        private readonly MembershipProvider _provider;

        public AccountMembershipService()
            : this(null)
        {
        }

        public AccountMembershipService(MembershipProvider provider)
        {
            _provider = provider ?? Membership.Provider;
        }

        public int MinPasswordLength
        {
            get
            {
                return _provider.MinRequiredPasswordLength;
            }
        }

        public bool ValidateUser(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Giá trị không được trống.", "userName");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("Giá trị không được trống.", "password");
            return _provider.ValidateUser(userName, password);
        }

        public MembershipCreateStatus CreateUser(string userName, string password, string email)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Giá trị không được trống.", "userName");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("Giá trị không được trống.", "password");
            if (String.IsNullOrEmpty(email)) throw new ArgumentException("Giá trị không được trống.", "email");

            MembershipCreateStatus status;
            _provider.CreateUser(userName, password, email, null, null, true, null, out status);

            return status;
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Giá trị không được trống.", "userName");
            if (String.IsNullOrEmpty(oldPassword)) throw new ArgumentException("Giá trị không được trống.", "oldPassword");
            if (String.IsNullOrEmpty(newPassword)) throw new ArgumentException("Giá trị không được trống.", "newPassword");

            // The underlying ChangePassword() will throw an exception rather
            // than return false in certain failure scenarios.
            try
            {
                MembershipUser currentUser = _provider.GetUser(userName, true /* userIsOnline */);
                return currentUser.ChangePassword(oldPassword, newPassword);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (MembershipPasswordException)
            {
                return false;
            }
        }

    }

    public interface IFormsAuthenticationService
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignOut();
    }

    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Giá trị không được trống.", "userName");

            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);

        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
    #endregion

    #region Validation
    public static class AccountValidation
    {

        public static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    //return "Username already exists. Please enter a different user name.";
                    return "Tên người dùng đã có. Xin vui lòng nhập tên khác.";

                case MembershipCreateStatus.DuplicateEmail:
                    //return "A username for that e-mail address already exists. Please enter a different e-mail address.";
                    return "Có người đã sử dụng địa chỉ e-mail này rồi. Xin vui lòng nhập địa chỉ e-mail khác. ";

                case MembershipCreateStatus.InvalidPassword:
                    //return "The password provided is invalid. Please enter a valid password value.";
                    return "Mật khẩu cung cấp không phù hợp. Xin vui lòng nhập mật khẩu phù hợp. ";

                case MembershipCreateStatus.InvalidEmail:
                    //return "The e-mail address provided is invalid. Please check the value and try again.";
                    return "Địa chỉ e-mail cung cấp không phù hợp. Xin vui lòng kiểm tra giá trị và thử lại.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    //return "The user name provided is invalid. Please check the value and try again.";
                    return "Tên người dùng cung cấp không phù hợp. Xin vui lòng kiểm tra giá trị và thử lại.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class PropertiesMustMatchAttribute : ValidationAttribute
    {
        //private const string _defaultErrorMessage = "'{0}' and '{1}' do not match.";
        private const string _defaultErrorMessage = "'{0}' và '{1}' không trùng khớp.";
        private readonly object _typeId = new object();

        public PropertiesMustMatchAttribute(string originalProperty, string confirmProperty)
            : base(_defaultErrorMessage)
        {
            OriginalProperty = originalProperty;
            ConfirmProperty = confirmProperty;
        }

        public string ConfirmProperty { get; private set; }
        public string OriginalProperty { get; private set; }

        public override object TypeId
        {
            get
            {
                return _typeId;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                OriginalProperty, ConfirmProperty);
        }

        public override bool IsValid(object value)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
            object originalValue = properties.Find(OriginalProperty, true /* ignoreCase */).GetValue(value);
            object confirmValue = properties.Find(ConfirmProperty, true /* ignoreCase */).GetValue(value);
            return Object.Equals(originalValue, confirmValue);
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidatePasswordLengthAttribute : ValidationAttribute
    {
        private const string _defaultErrorMessage = "'{0}' dài ít nhất {1} ký tự.";
        private readonly int _minCharacters = Membership.Provider.MinRequiredPasswordLength;

        public ValidatePasswordLengthAttribute()
            : base(_defaultErrorMessage)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                name, _minCharacters);
        }

        public override bool IsValid(object value)
        {
            string valueAsString = value as string;
            return (valueAsString != null && valueAsString.Length >= _minCharacters);
        }
    }
    #endregion

    #region oauth
    //public class UsersContext : DbContext
    //{
    //    public UsersContext()
    //        : base("DefaultConnection")
    //    {
    //    }

    //    public DbSet<UserProfile> UserProfiles { get; set; }
    //}

    //[Table("UserProfile")]
    //public class UserProfile
    //{
    //    [Key]
    //    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    //    public int UserId { get; set; }
    //    public string UserName { get; set; }
    //}

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "Tên người dùng")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        //[Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không trùng khớp.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Display(Name = "Ghi nhớ?")]
        public bool RememberMe { get; set; }
    }

    //public class RegisterModel
    //{
    //    [Required]
    //    [Display(Name = "Tên đăng nhập")]
    //    public string UserName { get; set; }

    //    [Required]
    //    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    //    [DataType(DataType.Password)]
    //    [Display(Name = "Mật khẩu")]
    //    public string Password { get; set; }

    //    [DataType(DataType.Password)]
    //    [Display(Name = "Confirm password")]
    //    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    //    public string ConfirmPassword { get; set; }
    //}

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
    #endregion
}





//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Data.Entity;
//using System.Globalization;
//using System.Web.Security;

//namespace WebApp.Models
//{
//    public class UsersContext : DbContext
//    {
//        public UsersContext()
//            : base("DefaultConnection")
//        {
//        }

//        public DbSet<UserProfile> UserProfiles { get; set; }
//        //public DbSet<Membership> Memberships { get; set; }
//        //public DbSet<Role> Roles { get; set; }
//        //public DbSet<OAuthMembership> OAuthMemberships { get; set; }

//    }


//    //[Table("webpages_Membership")]
//    //public class Membership
//    //{
//    //    public Membership()
//    //    {
//    //        Roles = new List<Role>();
//    //        OAuthMemberships = new List<OAuthMembership>();
//    //    }

//    //    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
//    //    public int UserId { get; set; }
//    //    public DateTime? CreateDate { get; set; }
//    //    [StringLength(128)]
//    //    public string ConfirmationToken { get; set; }
//    //    public bool? IsConfirmed { get; set; }
//    //    public DateTime? LastPasswordFailureDate { get; set; }
//    //    public int PasswordFailuresSinceLastSuccess { get; set; }
//    //    [Required, StringLength(128)]
//    //    public string Password { get; set; }
//    //    public DateTime? PasswordChangedDate { get; set; }
//    //    [Required, StringLength(128)]
//    //    public string PasswordSalt { get; set; }
//    //    [StringLength(128)]
//    //    public string PasswordVerificationToken { get; set; }
//    //    public DateTime? PasswordVerificationTokenExpirationDate { get; set; }

//    //    public ICollection<Role> Roles { get; set; }

//    //    [ForeignKey("UserId")]
//    //    public ICollection<OAuthMembership> OAuthMemberships { get; set; }
//    //}

//    //[Table("webpages_OAuthMembership")]
//    //public class OAuthMembership
//    //{
//    //    [Key, Column(Order = 0), StringLength(30)]
//    //    public string Provider { get; set; }

//    //    [Key, Column(Order = 1), StringLength(100)]
//    //    public string ProviderUserId { get; set; }

//    //    public int UserId { get; set; }

//    //    [Column("UserId"), InverseProperty("OAuthMemberships")]
//    //    public Membership User { get; set; }
//    //}
//    //[Table("webpages_Roles")]
//    //public class Role
//    //{
//    //    public Role()
//    //    {
//    //        Members = new List<Membership>();
//    //    }

//    //    [Key]
//    //    public int RoleId { get; set; }
//    //    [StringLength(256)]
//    //    public string RoleName { get; set; }

//    //    public ICollection<Membership> Members { get; set; }
//    //}

//    [Table("UserProfile")]
//    public class UserProfile
//    {
//        [Key]
//        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
//        public int UserId { get; set; }
//        public string UserName { get; set; }
//    }

//    public class RegisterExternalLoginModel
//    {
//        [Required]
//        [Display(Name = "Tên người dùng")]
//        public string UserName { get; set; }

//        public string ExternalLoginData { get; set; }
//    }

//    public class LocalPasswordModel
//    {
//        [Required]
//        [DataType(DataType.Password)]
//        [Display(Name = "Mật khẩu hiện hành")]
//        public string OldPassword { get; set; }

//        [Required]
//        [StringLength(100, ErrorMessage = "{0} phải ít nhất {2} ký tự.", MinimumLength = 3)]
//        [DataType(DataType.Password)]
//        [Display(Name = "Mật khẩu mới")]
//        public string NewPassword { get; set; }

//        [DataType(DataType.Password)]
//        [Display(Name = "Xác nhận mật khẩu mới")]
//        [Compare("NewPassword", ErrorMessage = "Mật khẩu mới và xác nhận mật khẩu không khớp.")]
//        public string ConfirmPassword { get; set; }
//    }

//    public class LoginModel
//    {
//        [Required]
//        [Display(Name = "Tên người dùng")]
//        public string UserName { get; set; }

//        [Required]
//        [DataType(DataType.Password)]
//        [Display(Name = "Mật khẩu")]
//        public string Password { get; set; }

//        [Display(Name = "Duy trì đăng nhập?")]
//        public bool RememberMe { get; set; }
//    }

//    public class RegisterModel
//    {
//        [Required]
//        [Display(Name = "Tên người dùng")]
//        public string UserName { get; set; }

//        [Required]
//        [StringLength(100, ErrorMessage = "{0} phải ít nhất {2} ký tự.", MinimumLength = 3)]
//        [DataType(DataType.Password)]
//        [Display(Name = "Mật khẩu")]
//        public string Password { get; set; }

//        [DataType(DataType.Password)]
//        [Display(Name = "Xác nhận mật khẩu")]
//        [Compare("Password", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không khớp.")]
//        public string ConfirmPassword { get; set; }
//    }

//    public class ExternalLogin
//    {
//        public string Provider { get; set; }
//        public string ProviderDisplayName { get; set; }
//        public string ProviderUserId { get; set; }
//    }


//}




