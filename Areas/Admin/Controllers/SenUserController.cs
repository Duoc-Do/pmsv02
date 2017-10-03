using System;
using System.Web.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    public class SenUserController : AppAdminListController
    {
        DAL.SenUser _dataobject;

        protected override IActionInvoker CreateActionInvoker()
        {
            _dataobject = new DAL.SenUser(Request);
            this.InitData(_dataobject);
            return base.CreateActionInvoker();
        }

        public ActionResult AutoComplete()
        {
            DAL.SenUser _dataobject = new DAL.SenUser(Request);
            return Json(_dataobject.AutoComplete(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FieldChange()
        {
            DAL.SenUser _dataobject = new DAL.SenUser(Request);
            return Json(_dataobject.FieldChange(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return PartialView(this._indexview, _dataobject.GetData());
        }

        public ActionResult Create(int? SenUserId)
        {
            return PartialView(this._updateview, _dataobject.GetNew(SenUserId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.SenUserView collection)
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

        public ActionResult Edit(int SenUserId)
        {
            return PartialView(this._updateview, _dataobject.GetEdit(SenUserId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int SenUserId, Models.SenUserView collection)
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

        public ActionResult Delete(int SenUserId)
        {
            return PartialView(this._deleteview, _dataobject.GetDelete(SenUserId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int SenUserId, FormCollection collection)
        {
            try
            {
                _dataobject.Delete(SenUserId);
                return RedirectToAction(this.ActionReturn());
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(this._deleteview, _dataobject.GetDelete(SenUserId));
            }
        }
    }
}