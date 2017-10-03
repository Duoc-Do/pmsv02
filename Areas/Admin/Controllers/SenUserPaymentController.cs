using System;
using System.Web.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    public class SenUserPaymentController : AppAdminListController
    {
        DAL.SenUserPayment _dataobject;

        protected override IActionInvoker CreateActionInvoker()
        {
            _dataobject = new DAL.SenUserPayment(Request);
            this.InitData(_dataobject);
            return base.CreateActionInvoker();
        }

        public ActionResult AutoComplete()
        {
            DAL.SenUserPayment _dataobject = new DAL.SenUserPayment(Request);
            return Json(_dataobject.AutoComplete(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FieldChange()
        {
            DAL.SenUserPayment _dataobject = new DAL.SenUserPayment(Request);
            return Json(_dataobject.FieldChange(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return PartialView(_dataobject.GetData());
        }

        public ActionResult Create(int? UserPaymentId)
        {
            return PartialView(this._updateview, _dataobject.GetNew(UserPaymentId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.SenUserPaymentView collection)
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

        public ActionResult Edit(int UserPaymentId)
        {
            return PartialView(this._updateview, _dataobject.GetEdit(UserPaymentId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int UserPaymentId, Models.SenUserPaymentView collection)
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

        public ActionResult Delete(int UserPaymentId)
        {
            return PartialView(this._deleteview, _dataobject.GetDelete(UserPaymentId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int UserPaymentId, FormCollection collection)
        {
            try
            {
                _dataobject.Delete(UserPaymentId);
                return RedirectToAction(this.ActionReturn());
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(this._deleteview, _dataobject.GetDelete(UserPaymentId));
            }
        }
    }
}