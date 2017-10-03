using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.DAL
{
    public partial class SysColumn : SenVietListObject
    {
        public SysColumn(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "SysColumn";
            this._metaname = "SysColumn";
            this._keyfield = "Name";
            this.appajaxoption.ajaxoption.Add("title", "Mô tả cột");
            base.InitObject();
        }

        public List<Models.SysColumn> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.SysColumns);
            return model;
        }

        public Models.SysColumn GetById(string Id)
        {
            return this._db.SysColumns.SingleOrDefault(m => m.Name == Id);
        }

        public Models.SysColumn GetNew(string Id)
        {
            if (!string.IsNullOrEmpty(Id)) return GetById(Id);
            return new Models.SysColumn();
        }

        public Models.SysColumn GetEdit(string Id)
        {
            return GetById(Id);
        }

        public Models.SysColumn GetDelete(string Id)
        {
            return GetById(Id);
        }
        public string Insert(Models.SysColumn data)
        {
            try
            {
                this.Validate(data);

                this._db.SysColumns.Add(data);
                this._db.SaveChanges();
                return data.Name;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string Update(Models.SysColumn data)
        {
            try
            {
                this.Validate(data);

                this._db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                return data.Name;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(string Id)
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