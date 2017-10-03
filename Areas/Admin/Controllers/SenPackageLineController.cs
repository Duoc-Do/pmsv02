
using System;
using System.Web.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    public class SenPackageLineController : AppAdminListController
    {
        DAL.SenPackageLine _dataobject;

        protected override IActionInvoker CreateActionInvoker()
        {
            _dataobject = new DAL.SenPackageLine(Request);
            this.InitData(_dataobject);
            return base.CreateActionInvoker();
        }

        public ActionResult AutoComplete()
        {
            DAL.SenPackageLine _dataobject = new DAL.SenPackageLine(Request);
            return Json(_dataobject.AutoComplete(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FieldChange()
        {
            DAL.SenPackageLine _dataobject = new DAL.SenPackageLine(Request);
            return Json(_dataobject.FieldChange(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return PartialView(this._indexview, _dataobject.GetData());
        }

        public ActionResult Create(int? PackageLineId)
        {
            return PartialView(this._updateview, _dataobject.GetNew(PackageLineId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.SenPackageLine collection)
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

        public ActionResult Edit(int PackageLineId)
        {
            return PartialView(this._updateview, _dataobject.GetEdit(PackageLineId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int PackageLineId, Models.SenPackageLine collection)
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

        public ActionResult Delete(int PackageLineId)
        {
            return PartialView(this._deleteview, _dataobject.GetDelete(PackageLineId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int PackageLineId, FormCollection collection)
        {
            try
            {
                _dataobject.Delete(PackageLineId);
                return RedirectToAction(this.ActionReturn());
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(this._deleteview, _dataobject.GetDelete(PackageLineId));
            }
        }
    }
}