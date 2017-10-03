using System;
using System.Linq;
using System.Web.Mvc;
using WebApp.Services;
using WebApp.Models;

namespace WebApp.Areas.Admin.Controllers
{
    public class SenGiftCardController : AppAdminListController
    {
        DAL.SenGiftCard _dataobject;

        protected override IActionInvoker CreateActionInvoker()
        {
            _dataobject = new DAL.SenGiftCard(Request);
            this.InitData(_dataobject);
            return base.CreateActionInvoker();
        }

        public ActionResult AutoComplete()
        {
            DAL.SenGiftCard _dataobject = new DAL.SenGiftCard(Request);
            return Json(_dataobject.AutoComplete(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FieldChange()
        {
            DAL.SenGiftCard _dataobject = new DAL.SenGiftCard(Request);
            return Json(_dataobject.FieldChange(), JsonRequestBehavior.AllowGet);
        }

        //Notification
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Notification(int Id)
        {

            try
            {
                #region mail cho người dùng
                var giftcard = _dataobject.GetById(Id);

                if (giftcard == null) throw new Exception("phải chọn thẻ");
                if (giftcard.RecipientName == null) throw new Exception("phải nhập tên người nhận");
                if (giftcard.RecipientEmail == null) throw new Exception("phải nhập email người nhận");


                //IWebHelper webhelper = new WebHelper(this.HttpContext);
                //var urlactive = Url.Action("RegisterActive").Remove(0, 1);
                //string RegisterActiveUrl = string.Format("{0}{1}?id={2}", webhelper.GetStoreLocation(), urlactive, user.UserId);
                string emailto = giftcard.RecipientEmail;

                // xử lí tạm cần dùng một mẫu tạm để gửi cho khách hàng
                #region thiết lập token

                GlobalToken.setToken("SenGiftCard.RecipientEmail", giftcard.RecipientEmail);
                GlobalToken.setToken("SenGiftCard.RecipientName", giftcard.RecipientName);

                GlobalToken.setToken("SenGiftCard.SenderEmail", giftcard.SenderEmail);
                GlobalToken.setToken("SenGiftCard.SenderName", giftcard.SenderName);

                GlobalToken.setToken("SenGiftCard.Message", giftcard.Message);
                GlobalToken.setToken("SenGiftCard.GiftCardCouponCode", giftcard.GiftCardCouponCode);
                GlobalToken.setToken("SenGiftCard.Amount", ExConvert.Data2String(giftcard.Amount, "numeric", "n0", ""));

                #endregion

                string SenMessageTemplate_Name = "sengiftcard.notification";


                var db = new SenContext();
                var messagetemplate = db.SenMessageTemplates.SingleOrDefault(m => m.Name == SenMessageTemplate_Name && m.IsActive == true);

                var queuedemail = new WebApp.Models.SenQueuedEmail();
                queuedemail.CreatedOnUtc = DateTime.UtcNow;
                queuedemail.EmailAccountId = messagetemplate.EmailAccountId;
                queuedemail.From = messagetemplate.SenEmailAccount.Email;
                queuedemail.FromName = messagetemplate.SenEmailAccount.DisplayName;
                queuedemail.Bcc = messagetemplate.BccEmailAddresses;
                queuedemail.Subject = GlobalToken.MappingToken(messagetemplate.Subject);
                queuedemail.To = emailto;
                queuedemail.ToName = giftcard.RecipientName;
                queuedemail.Priority = 0;
                queuedemail.Body = GlobalToken.MappingToken(messagetemplate.Body);

                db.SenQueuedEmails.Add(queuedemail);
                db.SaveChanges();
                #endregion

                giftcard.IsRecipientNotified = true;
                _dataobject.Update(giftcard);
            }
            catch (Exception ex)
            {
                return Json(new { ketqua = ex.Message });
            }

            

            return Json(new { ketqua = "Đã email Ok." });
        }


        public ActionResult Index()
        {
            ViewBag.Title = "Thẻ quà tặng";
            return PartialView(_dataobject.GetData());
        }

        public ActionResult Create(int? Id)
        {
            return PartialView(this._updateview, _dataobject.GetNew(Id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Models.SenGiftCard collection)
        {
            try
            {
                int outputId = _dataobject.Insert(collection);
                return RedirectToAction(this.ActionReturn());
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(this._updateview, collection);
            }
        }

        public ActionResult Edit(int Id)
        {
            return PartialView(this._updateview, _dataobject.GetEdit(Id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int Id, Models.SenGiftCard collection)
        {
            try
            {

                int outputId = _dataobject.Update(collection);
                return RedirectToAction(this.ActionReturn());
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(this._updateview, collection);
            }
        }

        public ActionResult Delete(int Id)
        {
            return PartialView(this._deleteview, _dataobject.GetDelete(Id));
        }

        public ActionResult GenerateCouponCode()
        {
            return Json(new {CouponCode = Guid.NewGuid().ToString().Substring(0,13)} , JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int Id, FormCollection collection)
        {
            try
            {
                _dataobject.Delete(Id);
                return RedirectToAction(this.ActionReturn());
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(this._deleteview, _dataobject.GetDelete(Id));
            }
        }
    }
}