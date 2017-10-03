using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Areas.Admin.Services;

namespace WebApp.Areas.Admin.DAL
{
    public partial class SenCompany : SenVietListObject
    {
        public SenCompany(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "SenCompany";
            this._metaname = "SenCompanyView";
            this._keyfield = "CompanyId";

            this.appajaxoption.ajaxoption.Add("title", "Doanh nghiệp");
            base.InitObject();
        }

        public List<Models.SenCompanyView> GetData() 
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.SenCompanyViews);
            return model;
        }

        public Models.SenCompany GetById(int Id)
        {
            return this._db.SenCompanys.SingleOrDefault(m => m.CompanyId == Id);
        }

        public Models.SenCompany GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.SenCompany();
        }

        public Models.SenCompany GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.SenCompany GetDelete(int id)
        {
            return GetById(id);
        }

        public int Insert(Models.SenCompany data)
        {
            try
            {
                this.Validate(data);
                data.CreateDate = DateTime.UtcNow;
                this._db.SenCompanys.Add(data);
                this._db.SaveChanges();

                var userservicenew = new Models.SenService() { CompanyId = data.CompanyId, UserId = data.UserId ?? Guid.Empty };
                this._db.SenServices.Add(userservicenew);
                this._db.SaveChanges();

                return data.CompanyId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateExpireDate(int id, DateTime ExpireDate)
        {
            var data = GetById(id);
            if (data != null)
            {
                data.ExpireDate = ExpireDate;
                this._db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();
            }
        }

        public int Update(Models.SenCompany data)
        {
            try
            {
                this.Validate(data);

                this._db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();
                return data.CompanyId;
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

                //var _rec2 = this._db.SenServices.SingleOrDefault(m=>m.CompanyId == _rec.CompanyId && m.UserId == _rec.UserId);
                //this._db.Entry(_rec2).State = System.Data.Entity.EntityState.Deleted;
                //this._db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override byte[] ExportToExcel()
        {
            byte[] bytes = null;
            using (var stream = new System.IO.MemoryStream())
            {
                Export.ExportToXlsx(stream, this.GetData(), this._metaobject.GetMetaTable());
                bytes = stream.ToArray();
            }
            return bytes;
        }
    }
}