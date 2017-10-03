
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Areas.Accounting.Services;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class GapJournalCare : SenVietListObject
    {
        public GapJournalCare(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "GapJournalCare";
            this._metaname = "GapJournalCare";
            this._keyfield = "JournalCareId";
            base.InitObject();
        }

        public List<Models.GapJournalCare> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.GapJournalCares);
            return model;
        }

        public Models.GapJournalCare GetById(int Id)
        {
            return this._db.GapJournalCares.SingleOrDefault(m => m.JournalCareId == Id);
        }
        public Models.GapJournalCare GetById2(int Id)
        {
            return this._db.GapJournalCares.SingleOrDefault(m => m.JournalCareId == Id);
        }

        public Models.GapJournalCare GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.GapJournalCare() { JournalCareDate = DateTime.Now };
        }

        public Models.GapJournalCare GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.GapJournalCare GetDelete(int id)
        {
            return GetById(id);
        }
        public int Insert(Models.GapJournalCare data)
        {
            try
            {
                this.Validate(data);
                BeginUpdate(data);
                var _data = new Models.GapJournalCare();
                this.MapView2Table(data, _data);
                this._db.GapJournalCares.Add(_data);
                this._db.SaveChanges();
                EndUpdate(_data.JournalCareId);
                return _data.JournalCareId;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void BeginUpdate(Models.GapJournalCare data)
        {
            if (data.ItemID != null)
            {
                var measurerate = Services.Voucher.GetMeasureRate(data.UOMID,data.ItemID);
                data.Quantity = Decimal.Round((Decimal)data.Quantity0 * (Decimal)measurerate, int.Parse(GlobalVariant.GetSysOption()["RoundQuantity"].ToString()));
            }
            //this._db.SaveChanges();
        }
        public void EndUpdate(int id)
        {

            var _data = this.GetById2(id);
            var gapjournal = this._db.GapJournals.SingleOrDefault(m => m.JournalId == _data.JournalId);
            if (gapjournal != null)
            {
                var dalitem = new DAL.AppItemTable(this._request);
                gapjournal.IsolationDate = null;
                foreach (var item in gapjournal.GapJournalCares)
                {
                    if (gapjournal.IsolationDate == null)
                    {
                        gapjournal.IsolationDate = item.JournalCareDate;
                        gapjournal.IsolationDay = dalitem.GetIsolationDay(item.ItemID??0);
                    }
                    else
                    {

                        int i= dalitem.GetIsolationDay(item.ItemID ?? 0);
                        if (i > 0)
                        {
                            var daynumber1 = gapjournal.IsolationDateEnd();
                            var daynumber2 = item.JournalCareDate.AddDays(i);
                            if (daynumber2 > daynumber1)
                            {
                                gapjournal.IsolationDate = item.JournalCareDate;
                                gapjournal.IsolationDay = i;
                            }
                        }
                    }
                }
                this._db.Entry(gapjournal).State = System.Data.Entity.EntityState.Modified;
            }
            this._db.SaveChanges();
        }
        public int Update(Models.GapJournalCare data)
        {
            try
            {
                this.Validate(data);
                BeginUpdate(data);
                var _data = this.GetById2(data.JournalCareId);
                this.MapView2Table(data, _data);
                this._db.Entry(_data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();
                EndUpdate(_data.JournalCareId);
                return _data.JournalCareId;
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