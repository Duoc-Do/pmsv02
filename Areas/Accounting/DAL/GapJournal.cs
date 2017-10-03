
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class GapJournal : SenVietListObject
    {
        public GapJournal(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "GapJournal";
            this._metaname = "GapJournal";
            this._keyfield = "JournalId";
            base.InitObject();
        }

        public List<Models.GapJournal> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.GapJournals);
            return model;
        }

        public Models.GapJournal GetById(int Id)
        {
            return this._db.GapJournals.SingleOrDefault(m => m.JournalId == Id);
        }
        public Models.GapJournal GetById2(int Id)
        {
            return this._db.GapJournals.SingleOrDefault(m => m.JournalId == Id);
        }

        public Models.GapJournal GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.GapJournal() {JournalDate=DateTime.Now, StatusId=1 };
        }

        public Models.GapJournal GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.GapJournal GetDelete(int id)
        {
            return GetById(id);
        }
        public int Insert(Models.GapJournal data)
        {
            try
            {
                this.Validate(data);
                var _data = new Models.GapJournal();
                this.MapView2Table(data, _data);
                this._db.GapJournals.Add(_data);
                this._db.SaveChanges();
                return _data.JournalId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.GapJournal data)
        {
            try
            {
                this.Validate(data);
                var _data = this.GetById2(data.JournalId);

                this.MapView2Table(data, _data);
                this._db.Entry(_data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                return data.JournalId;
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