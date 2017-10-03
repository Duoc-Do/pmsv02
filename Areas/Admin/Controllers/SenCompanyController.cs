using System;
using System.Linq;
using System.Web.Mvc;
using WebApp.Services;
using WebApp.Models;

namespace WebApp.Areas.Admin.Controllers
{
    public class SenCompanyController : AppAdminListController
    {
        DAL.SenCompany _dataobject;

        protected override IActionInvoker CreateActionInvoker()
        {
            _dataobject = new DAL.SenCompany(Request);
            this.InitData(_dataobject);
            return base.CreateActionInvoker();
        }

        public ActionResult AutoComplete()
        {
            DAL.SenCompany _dataobject = new DAL.SenCompany(Request);
            return Json(_dataobject.AutoComplete(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FieldChange()
        {
            DAL.SenCompany _dataobject = new DAL.SenCompany(Request);
            return Json(_dataobject.FieldChange(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Doanh nghiệp đăng ký";
            return PartialView(_dataobject.GetData());
        }

        public ActionResult Create(int? CompanyId)
        {
            //if (!_dataobject.sysbusinessrole.IsAdd) return PartialView(this._roleview);
            return PartialView(this._updateview, _dataobject.GetNew(CompanyId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Models.SenCompany collection)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approved(int CompanyId, bool IsApproved)
        {

            try
            {
                #region mail cho người dùng
                var company = _dataobject.GetById(CompanyId);
                var user = company.UserProfile;
                //IWebHelper webhelper = new WebHelper(this.HttpContext);
                //var urlactive = Url.Action("RegisterActive").Remove(0, 1);
                //string RegisterActiveUrl = string.Format("{0}{1}?id={2}", webhelper.GetStoreLocation(), urlactive, user.UserId);
                string emailto = user.Email;

                // xử lí tạm cần dùng một mẫu tạm để gửi cho khách hàng
                #region thiết lập token
                //GlobalToken.setToken("Account.RegisterActiveURL", RegisterActiveUrl);
                GlobalToken.setToken("Account.Name", user.UserName);
                GlobalToken.setToken("SenCompany.Name", company.Name);
                #endregion

                string SenMessageTemplate_Name = "sencompany.isapproved";
                if (!IsApproved) SenMessageTemplate_Name = "sencompany.notapproved";


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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDb(int CompanyId)
        {
            //tạo database ke toan mau.
            string bkname = Server.MapPath("~/DataTmp/WebAppAcc.bak");

            //try
            //{
            #region mail cho người dùng

            using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
            {
                //my work, including restore query to remote sql server
                _dataobject._db.Database.ExecuteSqlCommand(System.Data.Entity.TransactionalBehavior.DoNotEnsureTransaction, string.Format("ssp_CreateDb {0},'{1}'", CompanyId, bkname));

                ts.Complete();

            }

            #endregion

            //}
            //catch (Exception ex)
            //{
            //    return Json(new { ketqua = ex });
            //}

            return Json(new { ketqua = "Đã tạo database Ok." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SynCompanyName(int CompanyId)
        {


            try
            {
                var company = _dataobject.GetEdit(CompanyId);
                if (company == null) throw new Exception("Phải chọn công ty");
                if (string.IsNullOrEmpty(company.ConnectionString)) throw new Exception("Phải khai báo connection");

                var _db = new WebApp.Areas.Accounting.Models.WebAppAccEntities(company.ConnectionString);

                var strsql = string.Format("Update SysCompany Set EnumName=N'{0}' where EnumValue='CompanyAddress'", company.Address);
                strsql += string.Format(" Update SysCompany Set EnumName=N'{0}' where EnumValue='CompanyFax'", company.FaxNumber);
                strsql += string.Format(" Update SysCompany Set EnumName=N'{0}' where EnumValue='CompanyHomePage'", company.Website);
                strsql += string.Format(" Update SysCompany Set EnumName=N'{0}' where EnumValue='CompanyName'", company.Name);
                strsql += string.Format(" Update SysCompany Set EnumName=N'{0}' where EnumValue='CompanyPhone'", company.Telephone);
                strsql += string.Format(" Update SysCompany Set EnumName=N'{0}' where EnumValue='CompanyTaxCode'", company.TaxCode);
                _db.Database.ExecuteSqlCommand(strsql);


            }
            catch (Exception ex)
            {
                return Json(new { ketqua = ex });
            }

            return Json(new { ketqua = "Đã đồng bộ Ok." });
        }

        public ActionResult Edit(int CompanyId)
        {
            //if (!_dataobject.sysbusinessrole.IsEdit) return PartialView(this._roleview);
            return PartialView(this._updateview, _dataobject.GetEdit(CompanyId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int CompanyId, Models.SenCompany collection)
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

        public ActionResult Delete(int CompanyId)
        {
            //if (!_dataobject.sysbusinessrole.IsDelete) return PartialView(this._roleview);
            return PartialView(this._deleteview, _dataobject.GetDelete(CompanyId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int CompanyId, FormCollection collection)
        {
            try
            {
                _dataobject.Delete(CompanyId);
                return RedirectToAction(this.ActionReturn());
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(this._deleteview, _dataobject.GetDelete(CompanyId));
            }
        }
    }
}