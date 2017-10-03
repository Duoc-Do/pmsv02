
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.DAL
{
    public partial class SenEmailAccount : SenVietListObject
    {
        public SenEmailAccount(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "SenEmailAccount";
            this._metaname = "SenEmailAccount";
            this._keyfield = "EmailAccountId";
            this.appajaxoption.ajaxoption.Add("title", "Tài khoản email");
            base.InitObject();
        }

        public List<Models.SenEmailAccount> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.SenEmailAccounts);
            return model;
        }

        public Models.SenEmailAccount GetById(int Id)
        {
            return this._db.SenEmailAccounts.SingleOrDefault(m => m.EmailAccountId == Id);
        }

        public Models.SenEmailAccount GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.SenEmailAccount();
        }

        public Models.SenEmailAccount GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.SenEmailAccount GetDelete(int id)
        {
            return GetById(id);
        }
        public int Insert(Models.SenEmailAccount data)
        {
            try
            {
                this.Validate(data);

                this._db.SenEmailAccounts.Add(data);
                this._db.SaveChanges();
                return data.EmailAccountId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.SenEmailAccount data)
        {
            try
            {
                this.Validate(data);

                this._db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                return data.EmailAccountId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(int Id)
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