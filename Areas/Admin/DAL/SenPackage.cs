using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.DAL
{
    public partial class SenPackage : SenVietListObject
    {
        public SenPackage(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "SenPackage";
            this._metaname = "SenPackageView";
            this._keyfield = "PackageId";

            this.appajaxoption.ajaxoption.Add("title", "Gói sản phẩm");
            base.InitObject();
        }

        public List<Models.SenPackageView> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.SenPackageViews);
            return model;
        }

        public Models.SenPackage GetById(int Id)
        {
            return this._db.SenPackages.SingleOrDefault(m => m.PackageId == Id);
        }

        public Models.SenPackageView GetById2(int Id)
        {
            return this._db.SenPackageViews.SingleOrDefault(m => m.PackageId == Id);
        }

        public Models.SenPackage GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.SenPackage();
        }

        public Models.SenPackage GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.SenPackage GetDelete(int id)
        {
            return GetById(id);
        }
        public int Insert(Models.SenPackage data)
        {
            try
            {
                this.Validate(data);

                this._db.SenPackages.Add(data);
                this._db.SaveChanges();
                return data.PackageId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.SenPackage data)
        {
            try
            {
                this.Validate(data);

                this._db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                return data.PackageId;
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