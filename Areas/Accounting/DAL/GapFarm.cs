using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Areas.Accounting.Services;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class GapFarm : SenVietListObject
    {
        public GapFarm(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "GapFarm";
            this._metaname = "GapFarm";
            this._keyfield = "FarmId";
            base.InitObject();
        }

        public List<Models.GapFarm> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.GapFarms);
            return model;
        }

        public Models.GapFarm GetById(int Id)
        {
            return this._db.GapFarms.SingleOrDefault(m => m.FarmId == Id);
        }
        public Models.GapFarm GetById2(int Id)
        {
            return this._db.GapFarms.SingleOrDefault(m => m.FarmId == Id);
        }

        public Models.GapFarm GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.GapFarm() { IsActive = true};
        }

        public Models.GapFarm GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.GapFarm GetDelete(int id)
        {
            return GetById(id);
        }
        public int Insert(Models.GapFarm data)
        {
            try
            {
                this.Validate(data);

                data.CreatedBy = GlobalVariant.GetAppUser().UserID;
                data.CreatedDateTime = DateTime.Now;

                var _data = new Models.GapFarm();
                this.MapView2Table(data, _data);



                this._db.GapFarms.Add(_data);
                this._db.SaveChanges();
                return data.FarmId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.GapFarm data)
        {
            try
            {
                this.Validate(data);

                data.ModifiedBy = GlobalVariant.GetAppUser().UserID;
                data.ModifiedDateTime = DateTime.Now;

                var _data = this.GetById2(data.FarmId);


                this.MapView2Table(data, _data);
                this._db.Entry(_data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                return data.FarmId;
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