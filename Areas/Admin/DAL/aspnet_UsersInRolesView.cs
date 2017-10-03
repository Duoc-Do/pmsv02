using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace WebApp.Areas.Admin.DAL
{
    public partial class aspnet_UsersInRolesView : SenVietListObject
    {
        public aspnet_UsersInRolesView(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "aspnet_UsersInRolesView";
            this._metaname = "aspnet_UsersInRolesView";
            this._keyfield = "UserId,RoleId";
            this.appajaxoption.ajaxoption.Add("title", "Phân quyền thành viên");
            base.InitObject();
        }

        public List<Models.aspnet_UsersInRolesView> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.aspnet_UsersInRolesViews);
            return model;
        }

        public Models.aspnet_UsersInRolesView GetById(Guid UserId,Guid RoleId)
        {
            return this._db.aspnet_UsersInRolesViews.SingleOrDefault(m => m.UserId == UserId && m.RoleId == RoleId);
        }

        public Models.aspnet_UsersInRoles GetById2(Guid UserId, Guid RoleId)
        {
            return this._db.aspnet_UsersInRoles.SingleOrDefault(m => m.UserId == UserId && m.RoleId == RoleId);
        }


        public Models.aspnet_UsersInRolesView GetNew(Guid? UserId, Guid? RoleId)
        {
            return new Models.aspnet_UsersInRolesView();
        }

        public Models.aspnet_UsersInRolesView GetEdit(Guid UserId, Guid RoleId)
        {
            return GetById(UserId,RoleId);
        }

        public Models.aspnet_UsersInRolesView GetDelete(Guid UserId, Guid RoleId)
        {
            return GetById(UserId, RoleId);
        }
        public int Insert(Models.aspnet_UsersInRolesView data)
        {
            try
            {
                this.Validate(data);

                if (Roles.IsUserInRole(data.UserName, data.RoleName)) throw new InvalidOperationException("Phân quyền đã tồn tại.");

                var adddata = new Models.aspnet_UsersInRoles();
                adddata.RoleId = data.RoleId;
                adddata.UserId = data.UserId;
                this._db.aspnet_UsersInRoles.Add(adddata);
                this._db.SaveChanges();
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.aspnet_UsersInRolesView data)
        {
            try
            {
                //this.Validate(data);
                //var updatedata = GetById2(data.UserId, data.RoleId);

                ////this._db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                //this._db.SaveChanges();
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(Guid UserId, Guid RoleId)
        {
            try
            {
                var _rec = GetById2(UserId, RoleId);
                this._db.Entry(_rec).State = System.Data.Entity.EntityState.Deleted;
                this._db.SaveChanges();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}