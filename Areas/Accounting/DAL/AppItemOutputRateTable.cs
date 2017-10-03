using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class AppItemOutputRateTable : SenVietListObject
    {
        public AppItemOutputRateTable(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "AppItemOutputRateTable";
            this._metaname = "AppItemOutputRateView";
            this._keyfield = "ItemOutputRateID";
            base.InitObject();
        }

        public List<Models.AppItemOutputRateView> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.AppItemOutputRateViews);
            return model;
        }

        public Models.AppItemOutputRateView GetById(int Id)
        {
            return this._db.AppItemOutputRateViews.SingleOrDefault(m => m.ItemOutputRateID == Id);
        }

        public Models.AppItemOutputRateView GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.AppItemOutputRateView() {IsActive=true };
        }

        public Models.AppItemOutputRateView GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.AppItemOutputRateView GetDelete(int id)
        {
            return GetById(id);
        }
        public int Insert(Models.AppItemOutputRateView data)
        {
            try
            {
                this.Validate(data);
                var _data = new Models.AppItemOutputRateTable();
                MapView2Table(data, _data);
                this._db.AppItemOutputRateTables.Add(_data);
                this._db.SaveChanges();
                return _data.ItemOutputRateID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.AppItemOutputRateView data)
        {
            try
            {
                this.Validate(data);
                var _data = new Models.AppItemOutputRateTable();
                MapView2Table(data, _data);
                this._db.Entry(_data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                return data.ItemOutputRateID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Models.AppItemOutputRateTable GetById2(int Id)
        {
            return this._db.AppItemOutputRateTables.SingleOrDefault(m => m.ItemOutputRateID == Id);
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