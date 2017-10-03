using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class SysColumn : SenVietListObject
    {
        public SysColumn(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "SysColumn";
            this._metaname = "SysColumn";
            this._keyfield = "Name";
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
        public Models.SysColumn GetById2(string Id)
        {
            return this._db.SysColumns.SingleOrDefault(m => m.Name == Id);
        }

        public Models.SysColumn GetNew(string id)
        {
            if (id != null) return GetById(id);
            return new Models.SysColumn();
        }

        public Models.SysColumn GetEdit(string id)
        {
            return GetById(id);
        }

        public Models.SysColumn GetDelete(string id)
        {
            return GetById(id);
        }
        public string Insert(Models.SysColumn data)
        {
            try
            {
                this.Validate(data);
                var _data = new Models.SysColumn();
                this.MapView2Table(data, _data);
                this._db.SysColumns.Add(_data);
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
                var _data = this.GetById2(data.Name);

                this.MapView2Table(data, _data);
                this._db.Entry(_data).State = System.Data.Entity.EntityState.Modified;
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
                var _rec = GetById2(Id);
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