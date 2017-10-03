using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Areas.Admin.Services;

namespace WebApp.Areas.Admin.DAL
{
    public partial class SenCustomer : SenVietListObject
    {
        public SenCustomer(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "SenCustomer";
            this._metaname = "SenCustomer";
            this._keyfield = "CustomerId";
            //this._storeNameI = @"sp_SenCustomerI ";
            //this._storeNameU = @"sp_SenCustomerU ";
            //this._storeNameD = @"sp_SenCustomerD {0}";
            this.appajaxoption.ajaxoption.Add("title", "Khách hàng");
            base.InitObject();
        }

        public List<Models.SenCustomer> GetData()
        {
            //var query = this._db.SenCustomers.Where("it.Assign.UserName.Contains(\"AppAdmin\")").AsQueryable<Models.SenCustomer>();

            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.SenCustomers);
            return model; 
        }

        public Models.SenCustomer GetById(int Id)
        {
            return this._db.SenCustomers.SingleOrDefault(m => m.CustomerId == Id);
        }

        public Models.SenCustomer GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.SenCustomer() { IsActive=true}; 
        }

        public Models.SenCustomer GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.SenCustomer GetDelete(int id)
        {
            return GetById(id);
        }

        public int Insert(Models.SenCustomer data)
        {
            try
            {
                this.Validate(data);
                data.CreatedDateTime = DateTime.Now;
                this._db.SenCustomers.Add(data);
                this._db.SaveChanges();
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.SenCustomer data)
        {
            try
            {
                this.Validate(data);
                var _data = this.GetById(data.CustomerId);
                data.CreatedDateTime = _data.CreatedDateTime;
                data.ModifiedDateTime = DateTime.Now;

                this._db.Entry(_data).State = System.Data.Entity.EntityState.Detached;
                //this._db.SaveChanges();
                this._db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                return data.CustomerId;
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