using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Areas.Admin.Services;

namespace WebApp.Areas.Admin.DAL
{
    public partial class SenLog : SenVietListObject
    {
        public SenLog(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "SenLog";
            this._metaname = "SenLog";
            this._keyfield = "LogId";
            this.appajaxoption.ajaxoption.Add("title", "Nhật ký");
            base.InitObject();
        }

        public List<Models.SenLog> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.SenLogs);
            return model;
        }

        public Models.SenLog GetById(int Id)
        {
            return this._db.SenLogs.SingleOrDefault(m => m.LogId == Id);
        }

        public Models.SenLog GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.SenLog();
        }

        public Models.SenLog GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.SenLog GetDelete(int id)
        {
            return GetById(id);
        }
        public int Insert(Models.SenLog data)
        {
            try
            {
                this.Validate(data);

                this._db.SenLogs.Add(data);
                this._db.SaveChanges();
                return data.LogId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.SenLog data)
        {
            try
            {
                this.Validate(data);

                this._db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                return data.LogId;
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


        public override byte[] ExportToExcel()
        {
            byte[] bytes = null;
            using (var stream = new System.IO.MemoryStream())
            {
                Export.ExportToXlsx(stream, this.GetData(), this._metaobject.GetMetaTable());
                bytes = stream.ToArray();
            }
            return bytes;
        }
    }
}