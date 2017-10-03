
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.DAL
{
    public partial class SenPrivateMessage : SenVietListObject
    {
        public SenPrivateMessage(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "SenPrivateMessage";
            this._metaname = "SenPrivateMessageView";
            this._keyfield = "PrivateMessageId";
            this.appajaxoption.ajaxoption.Add("title", "Tin nhắn");
            base.InitObject();
        }

        public List<Models.SenPrivateMessageView> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.SenPrivateMessageViews);
            return model;
        }

        public Models.SenPrivateMessage GetById(int Id)
        {
            return this._db.SenPrivateMessages.SingleOrDefault(m => m.PrivateMessageId == Id);
        }

        public Models.SenPrivateMessage GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.SenPrivateMessage();
        }

        public Models.SenPrivateMessage GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.SenPrivateMessage GetDelete(int id)
        {
            return GetById(id);
        }
        public int Insert(Models.SenPrivateMessage data)
        {
            try
            {
                this.Validate(data);
                data.CreatedOnUtc = DateTime.UtcNow;
                this._db.SenPrivateMessages.Add(data);
                this._db.SaveChanges();
                return data.PrivateMessageId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.SenPrivateMessage data)
        {
            try
            {
                this.Validate(data);

                this._db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                return data.PrivateMessageId;
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