using System;
using System.Web.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    public class SenUserCashController : AppAdminListController
    {
        DAL.SenUserCash _dataobject;

        protected override IActionInvoker CreateActionInvoker()
        {
            _dataobject = new DAL.SenUserCash(Request);
            this.InitData(_dataobject);
            return base.CreateActionInvoker();
        }

        public ActionResult AutoComplete()
        {
            DAL.SenUserCash _dataobject = new DAL.SenUserCash(Request);
            return Json(_dataobject.AutoComplete(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FieldChange()
        {
            DAL.SenUserCash _dataobject = new DAL.SenUserCash(Request);
            return Json(_dataobject.FieldChange(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return PartialView(_dataobject.GetData());
        }

        public ActionResult Create(int? UserCashId)
        {
            return PartialView(this._updateview, _dataobject.GetNew(UserCashId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.SenUserCashView collection)
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

        public ActionResult Edit(int UserCashId)
        {
            return PartialView(this._updateview, _dataobject.GetEdit(UserCashId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int UserCashId, Models.SenUserCashView collection)
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

        public ActionResult Delete(int UserCashId)
        {
            return PartialView(this._deleteview, _dataobject.GetDelete(UserCashId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int UserCashId, FormCollection collection)
        {
            try
            {
                _dataobject.Delete(UserCashId);
                return RedirectToAction(this.ActionReturn());
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(this._deleteview, _dataobject.GetDelete(UserCashId));
            }
        }
    }
}