using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class AppWorkTable : SenVietListObject
    {
        public AppWorkTable(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "AppWorkTable";
            this._metaname = "AppWorkTable";
            this._keyfield = "WorkID";
            base.InitObject();
        }

        public List<Models.AppWorkTable> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.AppWorkTables);
            return model;
        }

        public Models.AppWorkTable GetById(int Id)
        {
            return this._db.AppWorkTables.SingleOrDefault(m => m.WorkID == Id);
        }
        public Models.AppWorkTable GetById2(int Id)
        {
            return this._db.AppWorkTables.SingleOrDefault(m => m.WorkID == Id);
        }

        public Models.AppWorkTable GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.AppWorkTable() { IsActive=true};
        }

        public Models.AppWorkTable GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.AppWorkTable GetDelete(int id)
        {
            return GetById(id);
        }
        public int Insert(Models.AppWorkTable data)
        {
            try
            {
                this.Validate(data);
                var _data = new Models.AppWorkTable();
                this.MapView2Table(data, _data);
                this._db.AppWorkTables.Add(_data);
                this._db.SaveChanges();
                return data.WorkID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.AppWorkTable data)
        {
            try
            {
                this.Validate(data);
                var _data = this.GetById2(data.WorkID);

                this.MapView2Table(data, _data);
                this._db.Entry(_data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                return data.WorkID;
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