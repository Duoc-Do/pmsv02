using System;
using System.Linq;
using System.Web.Mvc;
using WebApp.Services;

namespace WebApp.Areas.Admin.Controllers
{

    public class SenContractController : AppCRMListController
    {
        DAL.SenContract _dataobject;

        protected override IActionInvoker CreateActionInvoker()
        {
            _dataobject = new DAL.SenContract(Request);
            this.InitData(_dataobject);
            return base.CreateActionInvoker();
        }

        public ActionResult AutoComplete()
        {
            DAL.SenContract _dataobject = new DAL.SenContract(Request);
            return Json(_dataobject.AutoComplete(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FieldChange()
        {
            DAL.SenContract _dataobject = new DAL.SenContract(Request);
            return Json(_dataobject.FieldChange(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return PartialView(_dataobject.GetData());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContractNotification(int ContractId, int ContractType)
        {

            try
            {
                #region mail cho người dùng
                var contract = _dataobject.GetById(ContractId);
                var user = contract.Assign;
                //IWebHelper webhelper = new WebHelper(this.HttpContext);
                //var urlactive = Url.Action("RegisterActive").Remove(0, 1);
                //string RegisterActiveUrl = string.Format("{0}{1}?id={2}", webhelper.GetStoreLocation(), urlactive, user.UserId);
                string emailto = user.Email;

                // xử lí tạm cần dùng một mẫu tạm để gửi cho khách hàng
                #region thiết lập token
                //GlobalToken.setToken("Account.RegisterActiveURL", RegisterActiveUrl);
                GlobalToken.setToken("Account.Name", user.UserName);
                GlobalToken.setToken("SenContract.ContractNumber",contract.ContractNumber);
                #endregion

                string SenMessageTemplate_Name = "sencontract.notificationnew";
                if (ContractType == 1) SenMessageTemplate_Name = "sencontract.notificationupdate";
                if (ContractType == 2) SenMessageTemplate_Name = "sencontract.notificationend";

                var db = new WebApp.Models.SenContext();
                var messagetemplate = db.SenMessageTemplates.SingleOrDefault(m => m.Name == SenMessageTemplate_Name && m.IsActive == true);

                var queuedemail = new WebApp.Models.SenQueuedEmail();
                queuedemail.CreatedOnUtc = DateTime.UtcNow;
                queuedemail.EmailAccountId = messagetemplate.EmailAccountId;
                queuedemail.From = messagetemplate.SenEmailAccount.Email;
                queuedemail.FromName = messagetemplate.SenEmailAccount.DisplayName;
                queuedemail.Bcc = messagetemplate.BccEmailAddresses;
                queuedemail.Subject = GlobalToken.MappingToken(messagetemplate.Subject);
                queuedemail.To = emailto;
                queuedemail.ToName = user.UserName;
                queuedemail.Priority = 0;
                queuedemail.Body = GlobalToken.MappingToken(messagetemplate.Body);

                db.SenQueuedEmails.Add(queuedemail);
                db.SaveChanges();
                #endregion

            }
            catch (Exception ex)
            {
                return Json(new { ketqua = ex.Message });
            }

            return Json(new { ketqua = "Đã email Ok." });
        }

        public ActionResult Create(int? ContractId)
        {
            //if (!_dataobject.sysbusinessrole.IsAdd) return PartialView(this._roleview);
            return PartialView(this._updateview, _dataobject.GetNew(ContractId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.SenContract collection)
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

        public ActionResult Edit(int ContractId)
        {
            //if (!_dataobject.sysbusinessrole.IsEdit) return PartialView(this._roleview);
            return PartialView(this._updateview, _dataobject.GetEdit(ContractId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int ContractId, Models.SenContract collection)
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

        public ActionResult Delete(int ContractId)
        {
            //if (!_dataobject.sysbusinessrole.IsDelete) return PartialView(this._roleview);
            return PartialView(this._deleteview, _dataobject.GetDelete(ContractId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int ContractId, FormCollection collection)
        {
            try
            {
                _dataobject.Delete(ContractId);
                return RedirectToAction(this.ActionReturn());
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(this._deleteview, _dataobject.GetDelete(ContractId));
            }
        }
    }
}