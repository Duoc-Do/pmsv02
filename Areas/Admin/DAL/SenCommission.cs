using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace WebApp.Areas.Admin.DAL
{
    public partial class SenCommission : SenVietListObject
    {
        public SenCommission(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "SenCommission";
            this._metaname = "SenCommission";
            this._keyfield = "CommissionId";
            this.appajaxoption.ajaxoption.Add("title", "Chi hoa hồng");
            base.InitObject();
        }

        public List<Models.SenCommission> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.SenCommissions);
            return model;
        }

        public Models.SenCommission GetById(int Id)
        {
            return this._db.SenCommissions.SingleOrDefault(m => m.CommissionId == Id);
        }
   

        public Models.SenCommission GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.SenCommission() {VoucherDate=DateTime.UtcNow };
        }

        public Models.SenCommission GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.SenCommission GetDelete(int id)
        {
            return GetById(id);
        }

        public int Insert(Models.SenCommission data)
        {
            try
            {
                if (data.Amount == 0) throw new Exception("Phải nhập tiền");


                this.Validate(data);
                data.CreateBy = Guid.Parse(Membership.GetUser().ProviderUserKey.ToString());
                data.CreateDate = DateTime.UtcNow;

                var _data= new Models.SenCommission();
                this.MapView2Table(data, _data);
                this._db.SenCommissions.Add(_data);
                this._db.SaveChanges();

                var senContractdataobject = new DAL.SenContract(this._request);
                senContractdataobject.UpdateCommission(data.ContractId);

                return _data.CommissionId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.SenCommission data)
        {
            try
            {
                this.Validate(data);
                var _data = this.GetById(data.CommissionId);
                this.MapView2Table(data, _data);
                this._db.Entry(_data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                var senContractdataobject = new DAL.SenContract(this._request);
                senContractdataobject.UpdateCommission(data.ContractId);

                //this.SenContractTransactionPost(data.CommissionId);
                return data.CommissionId;
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

                var senContractdataobject = new DAL.SenContract(this._request);
                senContractdataobject.UpdateCommission(_rec.ContractId);

                //this.SenContractTransactionPost(Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}