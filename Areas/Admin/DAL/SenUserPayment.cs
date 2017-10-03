using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebApp.Areas.Admin.Services;

namespace WebApp.Areas.Admin.DAL
{
    public partial class SenUserPayment : SenVietListObject
    {
        public SenUserPayment(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "SenUserPayment";
            this._metaname = "SenUserPaymentView";
            this._keyfield = "UserPaymentId";
            this.appajaxoption.ajaxoption.Add("title", "Thanh toán doanh nghiệp");
            base.InitObject();
        }

        public override object FieldChange()
        {
            string fieldname = this._request.Params["fieldname"].ToString();
            string keyword = this._request.Params["keyword"].ToString();

            object results = null;




            switch (fieldname)
            {
                case "CompanyName":
                    var CompanyName = this._db.SenCompanys.Where(m => m.Name == keyword).SingleOrDefault();
                    if (CompanyName != null)
                    {
                        var package = this._db.SenPackages.Where(m => m.PackageId == CompanyName.PackageId).SingleOrDefault();

                        decimal UnitPrice = 0;
                        if (package != null)
                        {
                            UnitPrice = package.UnitPrice;
                        }

                        results = (new { extrows = new { UnitPrice = UnitPrice }, rows = new { UserId = CompanyName.UserId, CompanyId = CompanyName.CompanyId } });
                    }
                    break;

                default:
                    results = Services.Data.GetByCode(fieldname, keyword);
                    break;
            }

            return results;
        }


        public List<Models.SenUserPaymentView> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.SenUserPaymentViews);
            return model;
        }

        public Models.SenUserPaymentView GetById(int Id)
        {
            return this._db.SenUserPaymentViews.SingleOrDefault(m => m.UserPaymentId == Id);
        }

        public Models.SenUserPayment GetById2(int Id)
        {
            return this._db.SenUserPayments.SingleOrDefault(m => m.UserPaymentId == Id);
        }


        public Models.SenUserPaymentView GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.SenUserPaymentView() { VoucherDate = DateTime.UtcNow };
        }

        public Models.SenUserPaymentView GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.SenUserPaymentView GetDelete(int id)
        {
            return GetById(id);
        }

        public int Insert(Models.SenUserPaymentView data)
        {
            try
            {
                this.Validate(data);
                data.CreateBy = Guid.Parse(Membership.GetUser().ProviderUserKey.ToString());
                data.CreateDate = DateTime.UtcNow;

                var _data = new Models.SenUserPayment();
                this.MapView2Table(data, _data);
                this._db.SenUserPayments.Add(_data);
                this._db.SaveChanges();

                var senuserpay = this.GetById(_data.UserPaymentId);
                var senuserdataobject = new DAL.SenUser(this._request);
                senuserdataobject.UpdateCash(senuserpay.UserId ?? Guid.Empty);

                this.SenUserTransactionPost(_data.UserPaymentId);

                this.SetExpireDate(data.CompanyId, data.Quantity);

                return _data.UserPaymentId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SenUserTransactionPost(int id)
        {
            string storename = string.Format("{0} {1}", "sp_SenUserPaymentPost", id);
            this._db.Database.ExecuteSqlCommand(storename);
        }

        public int Update(Models.SenUserPaymentView data)
        {
            try
            {
                this.Validate(data);
                var _data = this.GetById2(data.UserPaymentId);

                data.CreateDate = _data.CreateDate;
                data.CreateBy = _data.CreateBy;

                this.MapView2Table(data, _data);
                this._db.Entry(_data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                var senuserdataobject = new DAL.SenUser(this._request);
                senuserdataobject.UpdateCash(data.UserId ?? Guid.Empty);

                this.SenUserTransactionPost(data.UserPaymentId);
                return data.UserPaymentId;
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
                var _rec = GetById2(Id);
                var _rec2 = GetById(Id);

                this._db.Entry(_rec).State = System.Data.Entity.EntityState.Deleted;
                this._db.SaveChanges();

                var senuserdataobject = new DAL.SenUser(this._request);
                senuserdataobject.UpdateCash(_rec2.UserId ?? Guid.Empty);

                this.SenUserTransactionPost(Id);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool AmountLimit(decimal Amount)
        {
            Guid userid = Guid.Parse(Membership.GetUser().ProviderUserKey.ToString());
            var result = this._db.SenUserTransactions.Where(m => m.UserId == userid).OrderByDescending(m => m.VoucherDate).ThenByDescending(m => m.TransactionType).ThenByDescending(m => m.VoucherId).FirstOrDefault();
            decimal _Amount = 0;
            if (result != null) _Amount = result.Balance;
            return Amount > _Amount;
        }

        public void SetExpireDate(int companyid, decimal quantity)
        {
            var result = this._db.SenCompanys.Where(m => m.CompanyId == companyid).SingleOrDefault();
            if (result != null)
            {

                if (result.ExpireDate.GetValueOrDefault() < DateTime.Today)
                {
                    if (DateTime.Today.IsEndOfMonth())
                    {
                        result.ExpireDate = DateTime.Today.AddMonths(Convert.ToInt16(quantity)).EndOfMonth();
                    }
                    else
                    {
                        result.ExpireDate = DateTime.Today.AddMonths(Convert.ToInt16(quantity));
                    }
                }
                else
                {
                    if (result.ExpireDate.Value.IsEndOfMonth())
                    {
                        result.ExpireDate = result.ExpireDate.Value.AddMonths(Convert.ToInt16(quantity)).EndOfMonth();
                    }
                    else
                    {
                        result.ExpireDate = result.ExpireDate.Value.AddMonths(Convert.ToInt16(quantity));
                    }
                }
                _db.Entry(result).State = System.Data.Entity.EntityState.Modified;
                _db.SaveChanges();
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