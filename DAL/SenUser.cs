using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace WebApp.DAL
{
    public partial class SenUser : SenVietListObject
    {
        public SenUser() : base(null) { }
        public SenUser(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "SenUser";
            this._metaname = "SenUser";
            this._keyfield = "UserId";
            base.InitObject();
        }

        public List<Models.SenUser> GetData()
        {
            var model = Services.Helpers.GridHelper.GetResults(this._request, this._metaname, this._db.SenUsers);
            return model;
        }

        public Models.SenUser GetById(Guid Id)
        {
            return this._db.SenUsers.SingleOrDefault(m => m.UserId == Id);
        }

        public Models.SenUser GetNew(Guid? id)
        {
            if (id != null) return GetById(id ?? Guid.Empty);
            return new Models.SenUser();
        }

        public Models.SenUser GetSenUser()
        {
            return GetEdit(Guid.Parse(Membership.GetUser().ProviderUserKey.ToString()));
        }

        public void AddCash(decimal amount)
        { 

        }

        public Models.SenUser GetEdit(Guid id)
        {
            var model = GetById(id);
            if (model == null)
            {
                model = new Models.SenUser();
            }

            return model;
        }

        public Models.SenUser GetDelete(Guid id)
        {
            return GetById(id);
        }

        public Guid Insert(Models.SenUser data)
        {
            try
            {
                this.Validate(data);
                data.UserId = Guid.Parse(Membership.GetUser().ProviderUserKey.ToString());
                this._db.SenUsers.Add(data);
                this._db.SaveChanges();
                return data.UserId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool IsExist(Guid id)
        {
            return this._db.SenUsers.Any(m => m.UserId.Equals(id));
        }

        public Guid Update(Models.SenUser data)
        {
            try
            {
                if (data.UserId == Guid.Empty)
                {
                    data.UserId = Guid.Parse(Membership.GetUser().ProviderUserKey.ToString());
                }
                this.Validate(data);

                if (this.IsExist(data.UserId))
                {
                    this._db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                    this._db.SaveChanges();
                    return data.UserId;
                }
                else
                {
                    return this.Insert(data);
                }

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
