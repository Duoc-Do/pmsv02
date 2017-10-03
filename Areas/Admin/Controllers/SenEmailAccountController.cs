using System;
using System.Web.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    public class SenEmailAccountController : AppAdminListController
    {
        DAL.SenEmailAccount _dataobject;

        protected override IActionInvoker CreateActionInvoker()
        {
            _dataobject = new DAL.SenEmailAccount(Request);
            this.InitData(_dataobject);
            return base.CreateActionInvoker();
        }

        public ActionResult AutoComplete()
        {
            DAL.SenEmailAccount _dataobject = new DAL.SenEmailAccount(Request);
            return Json(_dataobject.AutoComplete(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FieldChange()
        {
            DAL.SenEmailAccount _dataobject = new DAL.SenEmailAccount(Request);
            return Json(_dataobject.FieldChange(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return PartialView(this._indexview, _dataobject.GetData());
        }

        public ActionResult Create(int? EmailAccountId)
        {
            return PartialView(this._updateview, _dataobject.GetNew(EmailAccountId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.SenEmailAccount collection)
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

        public ActionResult Edit(int EmailAccountId)
        {
            return PartialView(this._updateview, _dataobject.GetEdit(EmailAccountId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int EmailAccountId, Models.SenEmailAccount collection)
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

        public ActionResult Delete(int EmailAccountId)
        {
            return PartialView(this._deleteview, _dataobject.GetDelete(EmailAccountId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int EmailAccountId, FormCollection collection)
        {
            try
            {
                _dataobject.Delete(EmailAccountId);
                return RedirectToAction(this.ActionReturn());
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(this._deleteview, _dataobject.GetDelete(EmailAccountId));
            }
        }
    }
}