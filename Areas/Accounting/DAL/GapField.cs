using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebApp.Areas.Accounting.Services;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class GapField : SenVietListObject
    {
        public GapField(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "GapField";
            this._metaname = "GapField";
            this._keyfield = "FieldId";
            base.InitObject();
        }

        public List<Models.GapField> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.GapFields);
            return model;
        }
        public List<Models.GapField> GetDataByFarm(int farmid)
        {
            var model = this._db.GapFields.Where(m => m.FarmId == farmid && m.IsActive == true).OrderBy(m => m.Order).OrderBy(m => m.FieldId).ToList();
            return model;
        }
        public Models.GapField GetById(int Id)
        {
            return this._db.GapFields.SingleOrDefault(m => m.FieldId == Id);
        }
        public Models.GapField GetById2(int Id)
        {
            return this._db.GapFields.SingleOrDefault(m => m.FieldId == Id);
        }

        public Models.GapField GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.GapField() {IsActive=true};
        }

        public Models.GapField GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.GapField GetDelete(int id)
        {
            return GetById(id);
        }
        public int Insert(Models.GapField data)
        {
            try
            {
                this.Validate(data);
                data.CreatedBy = GlobalVariant.GetAppUser().UserID;
                data.CreatedDateTime = DateTime.Now;
                var _data = new Models.GapField();
                this.MapView2Table(data, _data);
                this._db.GapFields.Add(_data);
                this._db.SaveChanges();
                return data.FieldId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.GapField data)
        {
            try
            {
                this.Validate(data);
                data.ModifiedBy = GlobalVariant.GetAppUser().UserID;
                data.ModifiedDateTime = DateTime.Now;
                var _data = this.GetById2(data.FieldId);

                this.MapView2Table(data, _data);
                this._db.Entry(_data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                return data.FieldId;
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