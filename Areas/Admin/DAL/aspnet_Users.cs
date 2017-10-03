using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.DAL
{
    public partial class aspnet_Users : SenVietListObject
    {
        public aspnet_Users(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "aspnet_Users";
            this._metaname = "aspnet_Users";
            this._keyfield = "UserId";
            this._storeNameI = @"sp_aspnet_UsersI ";
            this._storeNameU = @"sp_aspnet_UsersU ";
            this._storeNameD = @"sp_aspnet_UsersD {0}";

            this.appajaxoption.ajaxoption.Add("title", "Thành viên");

            base.InitObject();
        }

        public List<Models.aspnet_Users> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.aspnet_Users);
            return model;
        }

        public Models.aspnet_Users GetById(Guid Id)
        {
            return this._db.aspnet_Users.SingleOrDefault(m => m.UserId == Id);
        }

        public Models.aspnet_Users GetNew(Guid? id)
        {
            if (id != null) return GetById(id ?? Guid.Empty);
            return new Models.aspnet_Users();
        }

        public Models.aspnet_Users GetEdit(Guid id)
        {
            return GetById(id);
        }

        public Models.aspnet_Users GetDelete(Guid id)
        {
            return GetById(id);
        }

        public int Insert(Models.aspnet_Users data)
        {
            try
            {
                var appobject = this._db.aspnet_Applications.Where(m => m.ApplicationName == System.Web.Security.Membership.ApplicationName).SingleOrDefault();
                this.Validate(data);
                data.UserId = Guid.NewGuid();
                if (appobject!=null)
                {
                    data.ApplicationId = appobject.ApplicationId;
                }

                this._db.aspnet_Users.Add(data);
                this._db.SaveChanges();

                this._db.Database.ExecuteSqlCommand(string.Format("sp_SenUserSynUser '{0}'", data.UserName));

                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.aspnet_Users data)
        {
            try
            {
                this.Validate(data);
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
