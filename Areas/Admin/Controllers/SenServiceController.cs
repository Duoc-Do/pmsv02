
using System;
using System.Linq;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace WebApp.Areas.Admin.Controllers
{
    public class SenServiceController : AppAdminListController
    {
        DAL.SenService _dataobject;

        protected override IActionInvoker CreateActionInvoker()
        {
            _dataobject = new DAL.SenService(Request);
            this.InitData(_dataobject);
            return base.CreateActionInvoker();
        }

        public ActionResult AutoComplete()
        {
            DAL.SenService _dataobject = new DAL.SenService(Request);
            return Json(_dataobject.AutoComplete(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FieldChange()
        {
            DAL.SenService _dataobject = new DAL.SenService(Request);
            return Json(_dataobject.FieldChange(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return PartialView(this._indexview, _dataobject.GetData());
        }

        public ActionResult Create(int? ServiceId)
        {
            return PartialView(this._updateview, _dataobject.GetNew(ServiceId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.SenServiceView collection)
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

        public ActionResult Edit(int ServiceId)
        {
            return PartialView(this._updateview, _dataobject.GetEdit(ServiceId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int ServiceId, Models.SenServiceView collection)
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

        public ActionResult Delete(int ServiceId)
        {
            return PartialView(this._deleteview, _dataobject.GetDelete(ServiceId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int ServiceId, FormCollection collection)
        {
            try
            {
                _dataobject.Delete(ServiceId);
                return RedirectToAction(this.ActionReturn());
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(this._deleteview, _dataobject.GetDelete(ServiceId));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Remove(int ServiceId)
        {
            try
            {
                _dataobject.Delete(ServiceId);
            }
            catch (Exception ex)
            {
                return Json(new { ketqua = "0", message = ex.Message });
            }

            return Json(new { ketqua = "1" });
        }


        public ActionResult Add(int? CompanyId)
        {
            var company = _dataobject._db.SenCompanys.Where(m => m.CompanyId == CompanyId).SingleOrDefault();
            return PartialView(this._updateview, new Models.SenServiceView() { CompanyId = CompanyId.Value, CompanyName = company.Name });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Models.SenServiceView collection)
        {
            try
            {
                int outputId = _dataobject.Insert(collection);
                return Json(new { ketqua = "1" });
            }
            catch (Exception ex)
            {
                return Json(new { ketqua = "0", message = ex.Message });
            }
        }
    }
}