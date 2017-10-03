
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class GapJournalHarvest : SenVietListObject
    {
        public GapJournalHarvest(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "GapJournalHarvest";
            this._metaname = "GapJournalHarvest";
            this._keyfield = "JournalHarvestId";
            base.InitObject();
        }

        public List<Models.GapJournalHarvest> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.GapJournalHarvests);
            return model;
        }

        public Models.GapJournalHarvest GetById(int Id)
        {
            return this._db.GapJournalHarvests.SingleOrDefault(m => m.JournalHarvestId == Id);
        }
        public Models.GapJournalHarvest GetById2(int Id)
        {
            return this._db.GapJournalHarvests.SingleOrDefault(m => m.JournalHarvestId == Id);
        }

        public Models.GapJournalHarvest GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.GapJournalHarvest() {JournalHarvestDate=DateTime.Now };
        }

        public Models.GapJournalHarvest GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.GapJournalHarvest GetDelete(int id)
        {
            return GetById(id);
        }
        public int Insert(Models.GapJournalHarvest data)
        {
            try
            {
                this.Validate(data);
                var _data = new Models.GapJournalHarvest();
                this.MapView2Table(data, _data);
                this._db.GapJournalHarvests.Add(_data);
                this._db.SaveChanges();
                return data.JournalHarvestId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.GapJournalHarvest data)
        {
            try
            {
                this.Validate(data);
                var _data = this.GetById2(data.JournalHarvestId);

                this.MapView2Table(data, _data);
                this._db.Entry(_data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                return data.JournalHarvestId;
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