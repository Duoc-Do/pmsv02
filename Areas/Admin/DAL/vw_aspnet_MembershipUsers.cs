using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebApp.Areas.Admin.Services;

namespace WebApp.Areas.Admin.DAL
{
    public partial class vw_aspnet_MembershipUsers : SenVietListObject
    {
        public vw_aspnet_MembershipUsers(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "vw_aspnet_MembershipUsers";
            this._metaname = "vw_aspnet_MembershipUsers";
            this._keyfield = "UserId";
            this._storeNameI = @"sp_vw_aspnet_MembershipUsersI ";
            this._storeNameU = @"sp_vw_aspnet_MembershipUsersU ";
            this._storeNameD = @"sp_vw_aspnet_MembershipUsersD {0}";
            this.appajaxoption.ajaxoption.Add("title", "Thành viên");
            base.InitObject();
        }

        public List<Models.vw_aspnet_MembershipUsers> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.vw_aspnet_MembershipUsers);
            return model;
        }


        public Models.vw_aspnet_MembershipUsers GetById(Guid id)
        {
            var result = this._db.vw_aspnet_MembershipUsers.SingleOrDefault(m => m.UserId == id);
            result.Roles = this._db.aspnet_Roles.OrderBy(m=>m.RoleName).ToList();
            result.SelectedUserRoles = Roles.GetRolesForUser(result.UserName);
            return result;
        }
        public List<Models.aspnet_Roles> GetAllRoles()
        {
            return this._db.aspnet_Roles.OrderBy(m => m.RoleName).ToList();
        }

        public Models.vw_aspnet_MembershipUsers GetNew(Guid? id)
        {
            if (id != null) return GetById(id ?? Guid.Empty);

            var result = new Models.vw_aspnet_MembershipUsers();
            result.Roles = GetAllRoles();
            return result;
        }

        public Models.vw_aspnet_MembershipUsers GetEdit(Guid id)
        {
            return GetById(id);
        }

        public Models.vw_aspnet_MembershipUsers GetDelete(Guid id)
        {
            return GetById(id);
        }
        public int Insert(Models.vw_aspnet_MembershipUsers data)
        {
            try
            {
                this.Validate(data);

                var password = this._request["Password"];
                var confirmpassword = this._request["ConfirmPassword"];
                if (string.IsNullOrEmpty(password)) throw new InvalidOperationException("Phải nhập mật khẩu.");

                if (password != confirmpassword) throw new InvalidOperationException("Mật khẩu và xác nhận mật khẩu không khớp.");

                var aspnetusername = this._db.aspnet_Users.SingleOrDefault(m => m.UserName == data.UserName);

                if (aspnetusername != null) throw new InvalidOperationException("Tên thành viên đã tồn tại. Vui lòng đặt tên khác.");

                var aspnetuseremail = this._db.aspnet_Memberships.SingleOrDefault(m=>m.Email == data.Email);
                if (aspnetuseremail != null) throw new InvalidOperationException("Email đã tồn tại. Vui lòng đặt email khác.");

                MembershipCreateStatus createStatus;
                Membership.CreateUser(data.UserName, password, data.Email, null, null, data.IsApproved, null, out createStatus);
                if (createStatus!=MembershipCreateStatus.Success) throw new InvalidOperationException("Đã có lỗi trong quá trình xử lý. Xin vui lòng liên hệ với chúng tôi để được hỗ trợ. ");

                //cập nhật giá trị phụ
                var aspnetuser = this._db.aspnet_Users.SingleOrDefault(m => m.UserName == data.UserName);
                aspnetuser.MobileAlias = data.MobileAlias;
                aspnetuser.IsAnonymous = data.IsAnonymous;
                this._db.Entry(aspnetuser).State = System.Data.Entity.EntityState.Modified;

                var aspnetmember = this._db.aspnet_Memberships.SingleOrDefault(m => m.UserId == aspnetuser.UserId);

                aspnetmember.MobilePIN = data.MobilePIN;
                aspnetmember.IsLockedOut = data.IsLockedOut;
                aspnetmember.Comment = data.Comment;

                this._db.Entry(aspnetmember).State = System.Data.Entity.EntityState.Modified;

                this._db.SaveChanges();

                this._db.Database.ExecuteSqlCommand(string.Format("sp_SenUserSynUser '{0}'", data.UserName));

                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.vw_aspnet_MembershipUsers data)
        {
            try
            {
                this.Validate(data);

                //MembershipUser user;
                //Membership;
                var password = this._request["Password"];
                var confirmpassword = this._request["ConfirmPassword"];

                if (password != confirmpassword)
                {
                    throw new InvalidOperationException("Mật khẩu và xác nhận mật khẩu không khớp.");
                }

                var aspnetuser = this._db.aspnet_Users.SingleOrDefault(m => m.UserId == data.UserId);
                if (aspnetuser != null)
                {
                    var aspnetusername = this._db.aspnet_Users.SingleOrDefault(m => m.UserId != data.UserId && m.UserName == data.UserName);
                    if (aspnetusername != null) throw new InvalidOperationException("Tên thành viên đã tồn tại. Vui lòng đặt tên khác.");

                    var aspnetuseremail = this._db.aspnet_Memberships.SingleOrDefault(m => m.UserId != data.UserId && m.Email == data.Email);
                    if (aspnetuseremail != null) throw new InvalidOperationException("Email đã tồn tại. Vui lòng đặt email khác.");

                    aspnetuser.UserName = data.UserName;
                    aspnetuser.LoweredUserName = aspnetuser.UserName.ToLower();
                    aspnetuser.MobileAlias = data.MobileAlias;
                    aspnetuser.IsAnonymous = data.IsAnonymous;

                    this._db.Entry(aspnetuser).State = System.Data.Entity.EntityState.Modified;

                    var aspnetmember = this._db.aspnet_Memberships.SingleOrDefault(m => m.UserId == data.UserId);

                    aspnetmember.Email = data.Email;
                    aspnetmember.LoweredEmail = aspnetmember.Email.ToLower();
                    aspnetmember.MobilePIN = data.MobilePIN;

                    aspnetmember.IsLockedOut = data.IsLockedOut;
                    aspnetmember.IsApproved = data.IsApproved;

                    aspnetmember.Comment = data.Comment;

                    this._db.Entry(aspnetmember).State = System.Data.Entity.EntityState.Modified;

                    this._db.SaveChanges();

                    if (!string.IsNullOrEmpty(password))
                    {
                        //do để chế độ mã hóa mật khẩu theo HASH nên không thể lấy lại mật khẩu
                        //nên muốn thay đỗi mật khẩu phải reset và lấy mật khẩu reset để change
                        string oldpassword = Membership.GetUser(data.UserName).ResetPassword();
                        Membership.GetUser(data.UserName).ChangePassword(oldpassword, password);
                    }

                    var roles = Roles.GetRolesForUser(data.UserName);
                    if (roles.Count()>0) Roles.RemoveUserFromRoles(data.UserName,roles);
                    if (data.SelectedUserRoles!=null) Roles.AddUserToRoles(data.UserName, data.SelectedUserRoles);

                    #region cập nhật senuser
                    var senuser = this._db.SenUsers.Where(m => m.UserId == data.UserId).SingleOrDefault();
                    this.MapView2Table(data.SenUserView, senuser,"SenUser");
                    this._db.Entry(senuser).State = System.Data.Entity.EntityState.Modified;
                    this._db.SaveChanges();
                    #endregion

                }


                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(Guid id)
        {
            try
            {
                var _rec = GetById(id);
                Membership.DeleteUser(_rec.UserName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override byte[] ExportToExcel()
        {
            byte[] bytes = null;
            using (var stream = new MemoryStream())
            {
                Export.ExportToXlsx(stream, this.GetData(), this._metaobject.GetMetaTable());
                bytes = stream.ToArray();
            }
            return bytes;
        }
    }
}
