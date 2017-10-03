using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class AppMaterialBudgetTable : SenVietListObject
    {
        public AppMaterialBudgetTable(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "AppMaterialBudgetTable";
            this._metaname = "AppMaterialBudgetView";
            this._keyfield = "MaterialBudgetID";
            base.InitObject();
        }

        public List<Models.AppMaterialBudgetView> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.AppMaterialBudgetViews);
            return model;
        }

        public Models.AppMaterialBudgetView GetById(int Id)
        {
            return this._db.AppMaterialBudgetViews.SingleOrDefault(m => m.MaterialBudgetID == Id);
        }
        public Models.AppMaterialBudgetTable GetById2(int Id)
        {
            return this._db.AppMaterialBudgetTables.SingleOrDefault(m => m.MaterialBudgetID == Id);
        }

        public Models.AppMaterialBudgetView GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.AppMaterialBudgetView() { IsActive = true, DateOfExecution = DateTime.Today };
        }

        public Models.AppMaterialBudgetView GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.AppMaterialBudgetView GetDelete(int id)
        {
            return GetById(id);
        }
        public int Insert(Models.AppMaterialBudgetView data)
        {
            try
            {
                this.Validate(data);
                var _data = new Models.AppMaterialBudgetTable();
                this.MapView2Table(data, _data);
                this._db.AppMaterialBudgetTables.Add(_data);
                this._db.SaveChanges();
                return data.MaterialBudgetID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.AppMaterialBudgetView data)
        {
            try
            {
                this.Validate(data);
                var _data = this.GetById2(data.MaterialBudgetID);

                this.MapView2Table(data, _data);
                this._db.Entry(_data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                return data.MaterialBudgetID;
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