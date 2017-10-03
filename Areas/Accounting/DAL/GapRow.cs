using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Areas.Accounting.Services;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class GapRow : SenVietListObject
    {
        public GapRow(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "GapRow";
            this._metaname = "GapRow";
            this._keyfield = "RowId";
            base.InitObject();
        }

        public List<Models.GapRow> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.GapRows);
            return model;
        }

        public Models.GapRow GetById(int Id)
        {
            return this._db.GapRows.SingleOrDefault(m => m.RowId == Id);
        }
        public Models.GapRow GetById2(int Id)
        {
            return this._db.GapRows.SingleOrDefault(m => m.RowId == Id);
        }

        public Models.GapRow GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.GapRow() { IsActive=true};
        }

        public Models.GapRow GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.GapRow GetDelete(int id)
        {
            return GetById(id);
        }
        public int Insert(Models.GapRow data)
        {
            try
            {
                this.Validate(data);
                data.CreatedBy = GlobalVariant.GetAppUser().UserID;
                data.CreatedDateTime = DateTime.Now;
                var _data = new Models.GapRow();
                this.MapView2Table(data, _data);
                this._db.GapRows.Add(_data);
                this._db.SaveChanges();
                return data.RowId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.GapRow data)
        {
            try
            {
                this.Validate(data);
                data.ModifiedBy = GlobalVariant.GetAppUser().UserID;
                data.ModifiedDateTime = DateTime.Now;
                var _data = this.GetById2(data.RowId);

                this.MapView2Table(data, _data);
                this._db.Entry(_data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                return data.RowId;
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