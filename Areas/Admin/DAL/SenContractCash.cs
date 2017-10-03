using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace WebApp.Areas.Admin.DAL
{
    public partial class SenContractCash : SenVietListObject
    {
        public SenContractCash(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "SenContractCash";
            this._metaname = "SenContractCash";
            this._keyfield = "ContractCashId";
            this.appajaxoption.ajaxoption.Add("title", "Thu tiền hợp đồng");
            base.InitObject();
        }

        public List<Models.SenContractCash> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.SenContractCashs);
            return model;
        }

        public Models.SenContractCash GetById(int Id)
        {
            return this._db.SenContractCashs.SingleOrDefault(m => m.ContractCashId == Id);
        }
   

        public Models.SenContractCash GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.SenContractCash() {VoucherDate=DateTime.UtcNow };
        }

        public Models.SenContractCash GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.SenContractCash GetDelete(int id)
        {
            return GetById(id);
        }

        //public void SenContractTransactionPost(int id)
        //{
        //    //string storename = string.Format("{0} {1}", "sp_SenContractCashPost", id);
        //    //this._db.Database.ExecuteSqlCommand(storename);
        //}

        public int Insert(Models.SenContractCash data)
        {
            try
            {
                if (data.Amount == 0) throw new Exception("Phải nhập tiền");

                this.Validate(data);
                data.CreateBy = Guid.Parse(Membership.GetUser().ProviderUserKey.ToString());
                data.CreateDate = DateTime.UtcNow;

                var _data= new Models.SenContractCash();
                this.MapView2Table(data, _data);
                this._db.SenContractCashs.Add(_data);
                this._db.SaveChanges();

                var senContractdataobject = new DAL.SenContract(this._request);
                senContractdataobject.UpdateCash(data.ContractId);

                //this.SenContractTransactionPost(_data.ContractCashId);

                return _data.ContractCashId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.SenContractCash data)
        {
            try
            {
                this.Validate(data);
                var _data = this.GetById(data.ContractCashId);
                this.MapView2Table(data, _data);
                this._db.Entry(_data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                var senContractdataobject = new DAL.SenContract(this._request);
                senContractdataobject.UpdateCash(data.ContractId);

                //this.SenContractTransactionPost(data.ContractCashId);
                return data.ContractCashId;
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
                senContractdataobject.UpdateCash(_rec.ContractId);

                //this.SenContractTransactionPost(Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}