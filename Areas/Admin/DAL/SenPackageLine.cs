
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.DAL
{
    public partial class SenPackageLine : SenVietListObject
    {
        public SenPackageLine(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "SenPackageLine";
            this._metaname = "SenPackageLineView";
            this._keyfield = "PackageLineId";
            this.appajaxoption.ajaxoption.Add("title", "Chi tiết gói sản phẩm");
            base.InitObject();
        }

        public List<Models.SenPackageLineView> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.SenPackageLineViews);
            return model;
        }

        public Models.SenPackageLine GetById(int Id)
        {
            return this._db.SenPackageLines.SingleOrDefault(m => m.PackageLineId == Id);
        }

        public Models.SenPackageLine GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.SenPackageLine();
        }

        public Models.SenPackageLine GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.SenPackageLine GetDelete(int id)
        {
            return GetById(id);
        }
        public int Insert(Models.SenPackageLine data)
        {
            try
            {
                this.Validate(data);

                this._db.SenPackageLines.Add(data);
                this._db.SaveChanges();
                return data.PackageLineId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.SenPackageLine data)
        {
            try
            {
                this.Validate(data);

                this._db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                return data.PackageLineId;
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