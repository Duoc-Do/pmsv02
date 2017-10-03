
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class GapSeed : SenVietListObject
    {
        public GapSeed(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "GapSeed";
            this._metaname = "GapSeed";
            this._keyfield = "SeedId";
            base.InitObject();
        }

        public List<Models.GapSeed> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.GapSeeds);
            return model;
        }

        public Models.GapSeed GetById(int Id)
        {
            return this._db.GapSeeds.SingleOrDefault(m => m.SeedId == Id);
        }
        public Models.GapSeed GetById2(int Id)
        {
            return this._db.GapSeeds.SingleOrDefault(m => m.SeedId == Id);
        }

        public Models.GapSeed GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.GapSeed() { IsActive=true};
        }

        public Models.GapSeed GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.GapSeed GetDelete(int id)
        {
            return GetById(id);
        }
        public int Insert(Models.GapSeed data)
        {
            try
            {
                this.Validate(data);
                var _data = new Models.GapSeed();
                this.MapView2Table(data, _data);
                this._db.GapSeeds.Add(_data);
                this._db.SaveChanges();
                return data.SeedId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.GapSeed data)
        {
            try
            {
                this.Validate(data);
                var _data = this.GetById2(data.SeedId);

                this.MapView2Table(data, _data);
                this._db.Entry(_data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                return data.SeedId;
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