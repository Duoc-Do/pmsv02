using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.DAL
{
    public partial class SenProduct : SenVietListObject
    {
        public SenProduct(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "SenProduct";
            this._metaname = "SenProduct";
            this._keyfield = "ProductId";
            //this._storeNameI = @"sp_SenProductI ";
            //this._storeNameU = @"sp_SenProductU ";
            //this._storeNameD = @"sp_SenProductD {0}";
            this.appajaxoption.ajaxoption.Add("title", "Sản phẩm");
            base.InitObject();
        }

        public List<Models.SenProduct> GetData()
        {

            //var query = this._db.SenProducts.AsQueryable<Models.SenProduct>().Select("new (ProductId, Name)");
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.SenProducts);
            return model;
        }

        public Models.SenProduct GetById(int Id)
        {
            return this._db.SenProducts.SingleOrDefault(m => m.ProductId == Id);
        }

        public Models.SenProduct GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.SenProduct();
        }

        public Models.SenProduct GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.SenProduct GetDelete(int id)
        {
            return GetById(id);
        }


        public int Insert(Models.SenProduct data)
        {
            try
            {

                this.Validate(data);

                this._db.SenProducts.Add(data);
                this._db.SaveChanges();
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.SenProduct data)
        {
            try
            {
                this.Validate(data);

                this._db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                return data.ProductId;
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