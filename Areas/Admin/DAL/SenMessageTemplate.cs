
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.DAL
{
    public partial class SenMessageTemplate : SenVietListObject
    {
        public SenMessageTemplate(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "SenMessageTemplate";
            this._metaname = "SenMessageTemplate";
            this._keyfield = "MessageTemplateId";
            this.appajaxoption.ajaxoption.Add("title", "Tin nhắn mẩu");
            base.InitObject();
        }

        public List<Models.SenMessageTemplate> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.SenMessageTemplates);
            return model;
        }

        public Models.SenMessageTemplate GetById(int Id)
        {
            return this._db.SenMessageTemplates.SingleOrDefault(m => m.MessageTemplateId == Id);
        }

        public Models.SenMessageTemplate GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.SenMessageTemplate();
        }

        public Models.SenMessageTemplate GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.SenMessageTemplate GetDelete(int id)
        {
            return GetById(id);
        }
        public int Insert(Models.SenMessageTemplate data)
        {
            try
            {
                this.Validate(data);

                this._db.SenMessageTemplates.Add(data);
                this._db.SaveChanges();
                return data.MessageTemplateId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.SenMessageTemplate data)
        {
            try
            {
                this.Validate(data);

                this._db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                return data.MessageTemplateId;
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