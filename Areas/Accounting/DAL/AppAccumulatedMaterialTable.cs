
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class AppAccumulatedMaterialTable : SenVietListObject
    {
        public AppAccumulatedMaterialTable(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "AppAccumulatedMaterialTable";
            this._metaname = "AppAccumulatedMaterialView";
            this._keyfield = "AccumulatedMaterialID";
            base.InitObject();
        }

        public List<Models.AppAccumulatedMaterialView> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.AppAccumulatedMaterialViews);
            return model;
        }

        public Models.AppAccumulatedMaterialView GetById(int Id)
        {
            return this._db.AppAccumulatedMaterialViews.SingleOrDefault(m => m.AccumulatedMaterialID == Id);
        }
        public Models.AppAccumulatedMaterialTable GetById2(int Id)
        {
            return this._db.AppAccumulatedMaterialTables.SingleOrDefault(m => m.AccumulatedMaterialID == Id);
        }

        public Models.AppAccumulatedMaterialView GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.AppAccumulatedMaterialView() {IsActive=true,AccumulatedDate=DateTime.Today };
        }

        public Models.AppAccumulatedMaterialView GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.AppAccumulatedMaterialView GetDelete(int id)
        {
            return GetById(id);
        }
        public int Insert(Models.AppAccumulatedMaterialView data)
        {
            try
            {
                this.Validate(data);
                var _data = new Models.AppAccumulatedMaterialTable();
                this.MapView2Table(data, _data);
                this._db.AppAccumulatedMaterialTables.Add(_data);
                this._db.SaveChanges();
                return data.AccumulatedMaterialID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.AppAccumulatedMaterialView data)
        {
            try
            {
                this.Validate(data);
                var _data = this.GetById2(data.AccumulatedMaterialID);

                this.MapView2Table(data, _data);
                this._db.Entry(_data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                return data.AccumulatedMaterialID;
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