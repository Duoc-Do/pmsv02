using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.DAL
{
    public partial class aspnet_Roles : SenVietListObject
    {
        public aspnet_Roles(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "aspnet_Roles";
            this._metaname = "aspnet_Roles";
            this._keyfield = "RoleId";
            this._storeNameI = @"sp_aspnet_RolesI ";
            this._storeNameU = @"sp_aspnet_RolesU ";
            this._storeNameD = @"sp_aspnet_RolesD {0}";
            this.appajaxoption.ajaxoption.Add("title", "Quyền thành viên");
            base.InitObject();
        }

        public List<Models.aspnet_Roles> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.aspnet_Roles);
            return model;
        }

        public Models.aspnet_Roles GetById(Guid Id)
        {
            return this._db.aspnet_Roles.SingleOrDefault(m => m.RoleId == Id);
        }

        public Models.aspnet_Roles GetNew(Guid? id)
        {
            if (id != null) return GetById(id ?? Guid.Empty);
            return new Models.aspnet_Roles();
        }

        public Models.aspnet_Roles GetEdit(Guid id)
        {
            return GetById(id);
        }

        public Models.aspnet_Roles GetDelete(Guid id)
        {
            return GetById(id);
        }
        public int Insert(Models.aspnet_Roles data)
        {
            try
            {

                var appobject = this._db.aspnet_Applications.Where(m => m.ApplicationName == System.Web.Security.Membership.ApplicationName).SingleOrDefault();
                this.Validate(data);
                data.RoleId = Guid.NewGuid();
                if (appobject!=null)
                {
                    data.ApplicationId = appobject.ApplicationId;
                }
                data.LoweredRoleName = data.RoleName.ToLower();

                this._db.aspnet_Roles.Add(data);
                this._db.SaveChanges();
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.aspnet_Roles data)
        {
            try
            {
                this.Validate(data);
                data.LoweredRoleName = data.RoleName.ToLower();
                this._db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(Guid Id)
        {
            try
            {
                var _rec = GetById(Id);
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
