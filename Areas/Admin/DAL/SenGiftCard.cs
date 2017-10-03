using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.DAL
{
    public partial class SenGiftCard : SenVietListObject
    {
        public SenGiftCard(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "SenGiftCard";
            this._metaname = "SenGiftCard";
            this._keyfield = "Id";

            this.appajaxoption.ajaxoption.Add("title", "thẻ quà tặng");
            base.InitObject();
        }

        public List<Models.SenGiftCard> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.SenGiftCards);
            return model;
        }

        public Models.SenGiftCard GetById(int Id)
        {
            return this._db.SenGiftCards.SingleOrDefault(m => m.Id == Id);
        }

        public Models.SenGiftCard GetByCouponCode(string CouponCode)
        {
            return this._db.SenGiftCards.SingleOrDefault(m => m.GiftCardCouponCode == CouponCode);
        }

        public Models.SenGiftCard GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.SenGiftCard() {CreatedOnUtc=DateTime.Now };
        }

        public Models.SenGiftCard GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.SenGiftCard GetDelete(int id)
        {
            return GetById(id);
        }
        public int Insert(Models.SenGiftCard data)
        {
            try
            {
                this.Validate(data);

                this._db.SenGiftCards.Add(data);
                this._db.SaveChanges();
                return data.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.SenGiftCard data)
        {
            try
            {
                this.Validate(data);

                this._db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                return data.Id;
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