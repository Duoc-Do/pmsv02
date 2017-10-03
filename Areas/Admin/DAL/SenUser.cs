using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebApp.Areas.Admin.Services;

namespace WebApp.Areas.Admin.DAL
{
    public partial class SenUser : SenVietListObject
    {
        public SenUser() : base(null) { }
        public SenUser(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "SenUser";
            this._metaname = "SenUserView";
            this._keyfield = "SenUserId";
            this.appajaxoption.ajaxoption.Add("title", "Thành viên sen việt");
            base.InitObject();
        }

        public List<Models.SenUserView> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.SenUserViews);
            return model;
        }

        public Models.SenUserView GetById(int Id)
        {
            return this._db.SenUserViews.SingleOrDefault(m => m.SenUserId == Id);
        }

        public Models.SenUser GetById2(int Id)
        {
            return this._db.SenUsers.SingleOrDefault(m => m.SenUserId == Id);
        }

        public Models.SenUserView GetById(Guid Id)
        {
            return this._db.SenUserViews.SingleOrDefault(m => m.UserId == Id);
        }

        public Models.SenUser GetById2(Guid Id)
        {
            return this._db.SenUsers.SingleOrDefault(m => m.UserId == Id);
        }

        public Models.SenUserView GetNew(Guid? id)
        {
            if (id != null) return GetById(id ?? Guid.Empty);
            return new Models.SenUserView();
        }

        public Models.SenUserView GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.SenUserView();
        }

        public Models.SenUser GetSenUser()
        {
            return GetById2(Guid.Parse(Membership.GetUser().ProviderUserKey.ToString()));
        }

        public void UpdateCash(Guid userid)
        {

            var senusercash = this._db.SenUserCashs.Where(m => m.UserId == userid).OrderByDescending(m => m.VoucherDate).FirstOrDefault();
            var amountcash = this._db.SenUserCashs.Where(m => m.UserId == userid).Select(m => m.Amount).DefaultIfEmpty(0).Sum();
            var amountpay = this._db.SenUserPaymentViews.Where(m => m.UserId == userid).Select(m => m.Amount).DefaultIfEmpty(0).Sum();
            var amount = amountcash - amountpay;

            var user = this._db.aspnet_Users.Where(m => m.UserId == userid);
            if (user == null) throw new Exception("Người dùng không tồn tại");
            if (IsExist(userid))
            {
                var senuseredit = GetById2(userid);
                senuseredit.AmountBalance = amount;
                //senuseredit.LastPayment = DateTime.UtcNow;
                if (senusercash != null)
                {
                    senuseredit.LastPayment = senusercash.VoucherDate;
                }

                this._db.Entry(senuseredit).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();
            }
            else
            {
                var senuser = new Models.SenUser();
                senuser.UserId = userid;
                senuser.AmountBalance = amount;
                //senuser.LastPayment = DateTime.UtcNow;
                if (senusercash != null)
                {
                    senuser.LastPayment = senusercash.VoucherDate;
                }

                this._db.SenUsers.Add(senuser);
                this._db.SaveChanges();
            }


            //var senuser = GetById(userid);
            //senuser.AmountBalance = amountcash - amountpay;
            //if (senusercash != null)
            //{
            //    senuser.LastPayment = senusercash.VoucherDate;
            //}

            //this.Update(senuser);

        }

        public void UpdatePayment(Guid userid)
        {
            var amountcash = this._db.SenUserCashs.Where(m => m.UserId == userid).Select(m => m.Amount).DefaultIfEmpty(0).Sum();
            var amountpay = this._db.SenUserPaymentViews.Where(m => m.UserId == userid).Select(m => m.Amount).DefaultIfEmpty(0).Sum();

            var senuser = GetById(userid);
            senuser.AmountBalance = amountcash - amountpay;
            this.Update(senuser);

        }


        public void AddCash(Guid userid, decimal amount)
        {
            var user = this._db.aspnet_Users.Where(m => m.UserId == userid);
            if (user == null) throw new Exception("Người dùng không tồn tại");
            if (IsExist(userid))
            {
                var senuseredit = GetById2(userid);
                senuseredit.AmountBalance = amount;
                senuseredit.LastPayment = DateTime.UtcNow;
                this._db.Entry(senuseredit).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();
            }
            else
            {
                var senuser = new Models.SenUser();
                senuser.UserId = userid;
                senuser.AmountBalance = amount;
                senuser.LastPayment = DateTime.UtcNow;
                this._db.SenUsers.Add(senuser);
                this._db.SaveChanges();
            }

        }

        public Models.SenUserView GetEdit(Guid id)
        {
            var model = GetById(id);
            if (model == null)
            {
                model = new Models.SenUserView();
            }

            return model;
        }

        public Models.SenUser GetDelete(Guid id)
        {
            return GetById2(id);
        }

        public Models.SenUserView GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.SenUser GetDelete(int id)
        {
            return GetById2(id);
        }

        public int Insert(Models.SenUserView data)
        {
            try
            {
                this.Validate(data);
                var _data = new Models.SenUser();
                this.MapView2Table(data, _data);
                this._db.SenUsers.Add(_data);
                this._db.SaveChanges();
                return data.SenUserId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.SenUserView data)
        {
            try
            {
                this.Validate(data);

                var _data = this.GetById2(data.SenUserId);
                this.MapView2Table(data, _data);

                this._db.Entry(_data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                return data.SenUserId;
            }
            catch (Exception)
            {
                throw;
            }
        }



        public Guid InsertByUser(Models.SenUserView data)
        {
            try
            {
                this.Validate(data);
                data.UserId = Guid.Parse(Membership.GetUser().ProviderUserKey.ToString());

                var _data = new Models.SenUser();
                this.MapView2Table(data, _data);

                this._db.SenUsers.Add(_data);
                this._db.SaveChanges();
                return data.UserId;
            }
            catch (Exception)
            {
                throw;
            }
        }



        private bool IsExist(Guid id)
        {
            return this._db.SenUsers.Any(m => m.UserId.Equals(id));
        }

        public Guid UpdateByUser(Models.SenUserView data)
        {
            try
            {
                if (data.UserId == Guid.Empty)
                {
                    data.UserId = Guid.Parse(Membership.GetUser().ProviderUserKey.ToString());
                }
                this.Validate(data);

                if (this.IsExist(data.UserId))
                {
                    var _data = this.GetById2(data.UserId);
                    this.MapView2Table(data, _data);

                    this._db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                    this._db.SaveChanges();
                    return data.UserId;
                }
                else
                {
                    return this.InsertByUser(data);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(Guid Id)
        {
            try
            {
                var _rec = GetDelete(Id);
                this._db.Entry(_rec).State = System.Data.Entity.EntityState.Deleted;
                this._db.SaveChanges();
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
                var _rec = GetDelete(Id);
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
