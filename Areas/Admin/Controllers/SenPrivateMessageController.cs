using System;
using System.Web.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    public class SenPrivateMessageController : AppAdminListController
    {
        DAL.SenPrivateMessage _dataobject;

        protected override IActionInvoker CreateActionInvoker()
        {
            _dataobject = new DAL.SenPrivateMessage(Request);
            this.InitData(_dataobject);
            return base.CreateActionInvoker();
        }

        public ActionResult AutoComplete()
        {
            DAL.SenPrivateMessage _dataobject = new DAL.SenPrivateMessage(Request);
            return Json(_dataobject.AutoComplete(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FieldChange()
        {
            DAL.SenPrivateMessage _dataobject = new DAL.SenPrivateMessage(Request);
            return Json(_dataobject.FieldChange(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return PartialView(_dataobject.GetData());
        }

        public ActionResult Create(int? PrivateMessageId)
        {
            return PartialView(this._updateview, _dataobject.GetNew(PrivateMessageId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.SenPrivateMessage collection)
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

        public ActionResult Edit(int PrivateMessageId)
        {
            return PartialView(this._updateview, _dataobject.GetEdit(PrivateMessageId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int PrivateMessageId, Models.SenPrivateMessage collection)
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

        public ActionResult Delete(int PrivateMessageId)
        {
            return PartialView(this._deleteview, _dataobject.GetDelete(PrivateMessageId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int PrivateMessageId, FormCollection collection)
        {
            try
            {
                _dataobject.Delete(PrivateMessageId);
                return RedirectToAction(this.ActionReturn());
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(this._deleteview, _dataobject.GetDelete(PrivateMessageId));
            }
        }
    }
}