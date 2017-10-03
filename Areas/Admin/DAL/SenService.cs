
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.DAL
{
    public partial class SenService : SenVietListObject
    {
        public SenService(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "SenService";
            this._metaname = "SenServiceView";
            this._keyfield = "ServiceId";
            this.appajaxoption.ajaxoption.Add("title", "Dịch vụ");
            base.InitObject();
        }

        public List<Models.SenServiceView> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.SenServiceViews);
            return model;
        }

        public Models.SenServiceView GetById(int Id)
        {
            return this._db.SenServiceViews.SingleOrDefault(m => m.ServiceId == Id);
        }

        public Models.SenService GetById2(int Id)
        {
            return this._db.SenServices.SingleOrDefault(m => m.ServiceId == Id);
        }

        public Models.SenServiceView GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.SenServiceView();
        }

        public Models.SenServiceView GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.SenServiceView GetDelete(int id)
        {
            return GetById(id);
        }
        public int Insert(Models.SenServiceView data)
        {
            try
            {
                this.Validate(data);
                var _data = new Models.SenService();
                MapView2Table(data, _data);
                this._db.SenServices.Add(_data);
                this._db.SaveChanges();
                return _data.ServiceId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.SenServiceView data)
        {
            try
            {
                this.Validate(data);
                var _data = this.GetById2(data.ServiceId);
                MapView2Table(data, _data);
                this._db.Entry(_data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                return _data.ServiceId;
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