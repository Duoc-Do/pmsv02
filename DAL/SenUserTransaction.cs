using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebApp.Services;

namespace WebApp.DAL
{
    public partial class SenUserTransaction : SenVietListObject
    {
        public SenUserTransaction(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "SenUserTransaction";
            this._metaname = "SenUserTransaction";
            this._keyfield = "TransactionId";
            this._storeNameI = @"sp_SenUserTransactionI @VoucherCode,@VoucherId";
            this._storeNameU = @"sp_SenUserTransactionU ";
            this._storeNameD = @"sp_SenUserTransactionD {0}";
            base.InitObject();
        }

        public Models.SenUserTransaction GetLastInfo()
        {
            Guid userid = Guid.Parse(Membership.GetUser().ProviderUserKey.ToString());
            return this._db.SenUserTransactions.Where(m => m.UserId == userid).OrderByDescending(m => m.VoucherDate).ThenByDescending(m => m.TransactionType).ThenByDescending(m => m.VoucherId).FirstOrDefault();
        }

        public decimal GetBalance()
        {
            var result = this.GetLastInfo();
            decimal _Amount = 0;
            if (result != null) _Amount = result.Balance;
            return _Amount;
        }

        public List<Models.SenUserTransaction> GetData()
        {
            var model = Services.Helpers.GridHelper.GetResults(this._request, this._metaname, this._db.SenUserTransactions);
            return model;
        }
        public List<Models.SenUserTransaction> GetDataByUser() 
        {
            Guid userid = Guid.Parse(Membership.GetUser().ProviderUserKey.ToString());
            return GetDataByUser(userid);
        }
        public List<Models.SenUserTransaction> GetDataByUser(Guid userid)
        {
            string search = "";
            if (this._request.Form["Paging.Search"] != null) search = this._request.Form["Paging.Search"];
            var MetaObject = Services.GlobalMeta.GetMetaObject(this._metaname);
            MetaObject.Paging.Search = search;

            var datefrom = DateTime.Today;
            var dateto = DateTime.Today;
            WebApp.Services.Helpers.Uti.Date.SetDateRange(this._request, out datefrom, out dateto);


            var query = this._db.SenUserTransactions.Where(m => m.UserId == userid && m.VoucherDate>=datefrom && m.VoucherDate<=dateto)
                .OrderByDescending(m => m.VoucherDate)
                .ThenByDescending(m => m.TransactionType)
                .ThenByDescending(m => m.VoucherId)
                .AsQueryable<Models.SenUserTransaction>();
            if (!string.IsNullOrEmpty(search)) query = query.Where(m => (m.Description.Contains(search) || m.VoucherDate.ToString().Contains(search))).AsQueryable<Models.SenUserTransaction>();
            var model = Services.Helpers.GridHelper.GetResults(this._request, this._metaname, query);
            return model;
        }

        public Models.SenUserTransaction GetById(int Id)
        {
            return this._db.SenUserTransactions.SingleOrDefault(m => m.TransactionId == Id); 
        }

        public Models.SenUserTransaction GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.SenUserTransaction();
        }

        public Models.SenUserTransaction GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.SenUserTransaction GetDelete(int id)
        {
            return GetById(id);
        }
        public int Insert(Models.SenUserTransaction data)
        {
            try
            {
                this.Validate(data);

                //data.CreatedBy = GlobalVariant.GetAppUser().UserID;
                //data.CreatedDateTime = DateTime.Now;

                SqlParameter[] parameters = ExConvert.Data2SqlParam(data, this._metaobject, this._paramnameoutput).ToArray();
                this._db.Database.ExecuteSqlCommand(this._storeNameI, parameters);

                return (int)parameters.GetValueSqlParam(this._paramnameoutput);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.SenUserTransaction data)
        {
            try
            {
                this.Validate(data);

                //data.ModifiedBy = GlobalVariant.GetAppUser().UserID;
                //data.ModifiedDateTime = DateTime.Now;

                SqlParameter pOriginal = ExConvert.ParseSqlParam(data, this._metaobject.GetMetaByColumnName(this._paramnameoutput), this._paramnameupdate);
                var parameters = ExConvert.Data2SqlParam(data, this._metaobject, pOriginal).ToArray();
                this._db.Database.ExecuteSqlCommand(this._storeNameU, parameters);

                return data.TransactionId;
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
                this._db.Database.ExecuteSqlCommand(this._storeNameD, Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}