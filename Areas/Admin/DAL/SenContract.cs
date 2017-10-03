using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebApp.Areas.Admin.Services;

namespace WebApp.Areas.Admin.DAL
{
    public partial class SenContract : SenVietListObject
    {
        public SenContract(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "SenContract";
            this._metaname = "SenContract";
            this._keyfield = "ContractId";
            //this._storeNameI = @"sp_SenContractI ";
            //this._storeNameU = @"sp_SenContractU ";
            //this._storeNameD = @"sp_SenContractD {0}";
            this.appajaxoption.ajaxoption.Add("title", "Hợp đồng");

            base.InitObject();
        }

        public List<Models.SenContract> GetDataByUser(dynamic viewbag )
        {
            Guid userid = Guid.Parse(Membership.GetUser().ProviderUserKey.ToString());
            return GetDataByUser(userid,viewbag);
        }

        public List<Models.SenContract> GetDataByUser(Guid userid ,dynamic viewbag)
        {
            string search = "";
            if (this._request.Form["Paging.Search"] != null) search = this._request.Form["Paging.Search"];
            var MetaObject = WebApp.Areas.Admin.Services.GlobalMeta.GetMetaObject(this._metaname);
            MetaObject.Paging.Search = search;


            var query = this._db.SenContracts.Where(m => m.AssignedTo == userid).AsQueryable<Models.SenContract>();
            viewbag.SumPayment = query.Select(m => m.Payment).DefaultIfEmpty().Sum();
            viewbag.SumCommission = query.Select(m => m.Commission).DefaultIfEmpty().Sum();
            viewbag.SumCommissionReceived = query.Select(m => m.CommissionReceived).DefaultIfEmpty().Sum();
            viewbag.SumCommissionPayment = query.Select(m => m.CommissionPayment).DefaultIfEmpty().Sum();

            if (!string.IsNullOrEmpty(search)) query = query.Where(m => (m.SenCustomer.Name.Contains(search) || m.ContractNumber.Contains(search))).AsQueryable<Models.SenContract>();

            if (this._request.Form["pcontractstatus"] != null)
            {
                if (!string.IsNullOrEmpty(this._request.Form["pcontractstatus"]))
                {
                    int status = int.Parse(this._request.Form["pcontractstatus"]);
                    query = query.Where(m => m.Status == status).AsQueryable<Models.SenContract>();
                }
            }

            if (this._request.Form["pcommissionpayment"] != null)
            {
                if (!string.IsNullOrEmpty(this._request.Form["pcommissionpayment"]))
                {
                    if (this._request.Form["pcommissionpayment"]=="on")
                    {
                        query = query.Where(m => m.CommissionPayment >0).AsQueryable<Models.SenContract>();
                    }
                }
            }



            var model = Services.GridHelper.GetResults(this._request, this._metaname, query);
            return model;
        }

        public List<Models.SenContract> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.SenContracts);
            return model;
        }

        public Models.SenContract GetById(int Id)
        {
            return this._db.SenContracts.SingleOrDefault(m => m.ContractId == Id);
        }

        public Models.SenContract ViewByUser(int Id)
        {
            Guid userid = Guid.Parse(Membership.GetUser().ProviderUserKey.ToString());

            return this._db.SenContracts.SingleOrDefault(m => m.ContractId == Id && m.AssignedTo == userid);
        }

        public Models.SenContract GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.SenContract() { ContractDate = DateTime.Now };
        }

        public Models.SenContract GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.SenContract GetDelete(int id)
        {
            return GetById(id);
        }

        public int Insert(Models.SenContract data)
        {
            try
            {
                this.Validate(data);

                this._db.SenContracts.Add(data);
                this._db.SaveChanges();
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.SenContract data)
        {
            try
            {
                this.Validate(data);

                this._db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                return data.ContractId;
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

        internal void UpdateCash(int Id)
        {
            //cập nhật lại thông tin giá trị trên hợp đồng
            var model = this.GetById(Id);
            decimal sumcash = model.SenContractCashs.Select(m => m.Amount).DefaultIfEmpty().Sum();
            model.Payment = sumcash;
            model.Commission = (model.CommissionPercentage * model.Payment) / 100;

            model.CommissionPayment = model.Commission - model.CommissionReceived;
            this._db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            this._db.SaveChanges();
            //throw new NotImplementedException();
        }

        internal void UpdateCommission(int Id)
        {
            //cập nhật lại thông tin giá trị trên hợp đồng
            var model = this.GetById(Id);
            decimal sumcommission = model.SenCommissions.Select(m => m.Amount).DefaultIfEmpty().Sum();
            model.CommissionReceived = sumcommission;
            model.CommissionPayment = model.Commission - model.CommissionReceived;
            this._db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            this._db.SaveChanges();
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