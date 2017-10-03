using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.DAL
{
    public partial class SenQueuedEmail : SenVietListObject
    {
        public SenQueuedEmail(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "SenQueuedEmail";
            this._metaname = "SenQueuedEmail";
            this._keyfield = "QueuedEmailId";

            this.appajaxoption.ajaxoption.Add("title", "Yêu cầu gửi email");

            base.InitObject();
        }

        public List<Models.SenQueuedEmail> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.SenQueuedEmails);
            return model;
        }

        public Models.SenQueuedEmail GetById(int Id)
        {
            return this._db.SenQueuedEmails.SingleOrDefault(m => m.QueuedEmailId == Id);
        }

        public Models.SenQueuedEmail GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.SenQueuedEmail();
        }

        public Models.SenQueuedEmail GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.SenQueuedEmail GetDelete(int id)
        {
            return GetById(id);
        }
        public int Insert(Models.SenQueuedEmail data)
        {
            try
            {
                this.Validate(data);

                this._db.SenQueuedEmails.Add(data);
                this._db.SaveChanges();
                return data.QueuedEmailId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.SenQueuedEmail data)
        {
            try
            {
                this.Validate(data);

                this._db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                return data.QueuedEmailId;
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