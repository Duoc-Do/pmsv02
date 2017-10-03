using System;
using System.Web.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    public class SenQueuedEmailController : AppAdminListController
    {
        DAL.SenQueuedEmail _dataobject;

        protected override IActionInvoker CreateActionInvoker()
        {
            _dataobject = new DAL.SenQueuedEmail(Request);
            this.InitData(_dataobject);
            return base.CreateActionInvoker();
        }

        public ActionResult AutoComplete()
        {
            DAL.SenQueuedEmail _dataobject = new DAL.SenQueuedEmail(Request);
            return Json(_dataobject.AutoComplete(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FieldChange()
        {
            DAL.SenQueuedEmail _dataobject = new DAL.SenQueuedEmail(Request);
            return Json(_dataobject.FieldChange(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return PartialView(_dataobject.GetData());
        }
                [ValidateInput(false)]//Không kiểm tra nội dung là thẻ html và script
        public ActionResult Create(int? QueuedEmailId)
        {
            return PartialView(this._updateview, _dataobject.GetNew(QueuedEmailId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
                [ValidateInput(false)]//Không kiểm tra nội dung là thẻ html và script
        public ActionResult Create(Models.SenQueuedEmail collection)
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

        public ActionResult Edit(int QueuedEmailId)
        {
            return PartialView(this._updateview, _dataobject.GetEdit(QueuedEmailId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]//Không kiểm tra nội dung là thẻ html và script
        public ActionResult Edit(int QueuedEmailId, Models.SenQueuedEmail collection)
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

        public ActionResult Delete(int QueuedEmailId)
        {
            return PartialView(this._deleteview, _dataobject.GetDelete(QueuedEmailId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int QueuedEmailId, FormCollection collection)
        {
            try
            {
                _dataobject.Delete(QueuedEmailId);
                return RedirectToAction(this.ActionReturn());
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(this._deleteview, _dataobject.GetDelete(QueuedEmailId));
            }
        }
    }
}