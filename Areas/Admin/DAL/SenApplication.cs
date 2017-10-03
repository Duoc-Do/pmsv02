using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.DAL
{
    public partial class SenApplication : SenVietListObject
    {
        public SenApplication(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "SenApplication";
            this._metaname = "SenApplication";
            this._keyfield = "ApplicationId";
            this._storeNameI = @"sp_SenApplicationI ";
            this._storeNameU = @"sp_SenApplicationU ";
            this._storeNameD = @"sp_SenApplicationD {0}";
            this.appajaxoption.ajaxoption.Add("title", "Ứng dụng");
            base.InitObject();
        }

        public List<Models.SenApplication> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.SenApplications);
            return model;
        }

        public Models.SenApplication GetById(int Id)
        {
            return this._db.SenApplications.SingleOrDefault(m => m.ApplicationId == Id);
        }

        public Models.SenApplication GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.SenApplication();
        }

        public Models.SenApplication GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.SenApplication GetDelete(int id)
        {
            return GetById(id);
        }
        public int Insert(Models.SenApplication data)
        {
            try
            {
                this.Validate(data);

                this._db.SenApplications.Add(data);
                this._db.SaveChanges();
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.SenApplication data)
        {
            try
            {
                this.Validate(data);

                this._db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                return data.ApplicationId;
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
                var _rec = GetById(Id);
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