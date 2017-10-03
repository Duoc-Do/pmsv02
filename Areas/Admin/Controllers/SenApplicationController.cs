using System;
using System.Web.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    public class SenApplicationController : AppAdminListController
    {
        DAL.SenApplication _dataobject;

        protected override IActionInvoker CreateActionInvoker()
        {
            _dataobject = new DAL.SenApplication(Request);
            this.InitData(_dataobject);
            return base.CreateActionInvoker();
        }

        public ActionResult AutoComplete()
        {
            DAL.SenApplication _dataobject = new DAL.SenApplication(Request);
            return Json(_dataobject.AutoComplete(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FieldChange()
        {
            DAL.SenApplication _dataobject = new DAL.SenApplication(Request);
            return Json(_dataobject.FieldChange(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return PartialView(this._indexview, _dataobject.GetData());
        }

        public ActionResult Create(int? ApplicationId)
        {
            //if (!_dataobject.sysbusinessrole.IsAdd) return PartialView(this._roleview);
            return PartialView(this._updateview, _dataobject.GetNew(ApplicationId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.SenApplication collection)
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

        public ActionResult Edit(int ApplicationId)
        {
            //if (!_dataobject.sysbusinessrole.IsEdit) return PartialView(this._roleview);
            return PartialView(this._updateview, _dataobject.GetEdit(ApplicationId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int ApplicationId, Models.SenApplication collection)
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

        public ActionResult Delete(int ApplicationId)
        {
            //if (!_dataobject.sysbusinessrole.IsDelete) return PartialView(this._roleview);
            return PartialView(this._deleteview, _dataobject.GetDelete(ApplicationId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int ApplicationId, FormCollection collection)
        {
            try
            {
                _dataobject.Delete(ApplicationId);
                return RedirectToAction(this.ActionReturn());
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(this._deleteview, _dataobject.GetDelete(ApplicationId));
            }
        }
    }
}