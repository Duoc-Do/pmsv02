using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebApp.Areas.Admin.Services;

namespace WebApp.Areas.Admin.DAL
{
    public partial class SenUserCash : SenVietListObject
    {
        public SenUserCash(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "SenUserCash";
            this._metaname = "SenUserCashView";
            this._keyfield = "UserCashId";
            this.appajaxoption.ajaxoption.Add("title", "Thành viên nạp tiền");
            base.InitObject();
        }

        public List<Models.SenUserCashView> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.SenUserCashViews);
            return model;
        }

        public Models.SenUserCashView GetById(int Id)
        {
            return this._db.SenUserCashViews.SingleOrDefault(m => m.UserCashId == Id);
        }

        public Models.SenUserCash GetById2(int Id)
        {
            return this._db.SenUserCashs.SingleOrDefault(m => m.UserCashId == Id);
        }

        public Models.SenUserCashView GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.SenUserCashView() {VoucherDate=DateTime.UtcNow };
        }

        public Models.SenUserCashView GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.SenUserCashView GetDelete(int id)
        {
            return GetById(id);
        }

        public void SenUserTransactionPost(int id)
        {
            string storename = string.Format("{0} {1}", "sp_SenUserCashPost", id);
            this._db.Database.ExecuteSqlCommand(storename);
        }

        public int InsertGiftCard(Models.SenGiftCard giftcard)
        {
            var cash = this.GetNew(null);

            Guid userid = Guid.Parse(Membership.GetUser().ProviderUserKey.ToString());

            cash.Amount = giftcard.Amount;
            cash.CashType = 1;
            cash.Description = string.Format("Khuyến mại: {0}", giftcard.GiftCardCouponCode);
            cash.UserId = userid;
            cash.UserName = Membership.GetUser().UserName;

            return this.Insert(cash);

        }

        public int Insert(Models.SenUserCashView data)
        {
            try
            {

                this.Validate(data);
                data.CreateBy = Guid.Parse(Membership.GetUser().ProviderUserKey.ToString());
                data.CreateDate = DateTime.UtcNow;

                var _data= new Models.SenUserCash();
                this.MapView2Table(data, _data);
                this._db.SenUserCashs.Add(_data);
                this._db.SaveChanges();

                var senuserdataobject = new DAL.SenUser(this._request);
                senuserdataobject.UpdateCash(data.UserId??Guid.Empty);

                this.SenUserTransactionPost(_data.UserCashId);

                return _data.UserCashId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.SenUserCashView data)
        {
            try
            {
                this.Validate(data);
                var _data = this.GetById2(data.UserCashId);
                this.MapView2Table(data, _data);
                this._db.Entry(_data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                var senuserdataobject = new DAL.SenUser(this._request);
                senuserdataobject.UpdateCash(data.UserId ?? Guid.Empty);

                this.SenUserTransactionPost(data.UserCashId);
                return data.UserCashId;
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