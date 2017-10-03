using System;
using System.Web.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    public class SenMessageTemplateController : AppAdminListController
    {
        DAL.SenMessageTemplate _dataobject;

        protected override IActionInvoker CreateActionInvoker()
        {
            _dataobject = new DAL.SenMessageTemplate(Request);
            this.InitData(_dataobject);
            return base.CreateActionInvoker();
        }

        public ActionResult AutoComplete()
        {
            DAL.SenMessageTemplate _dataobject = new DAL.SenMessageTemplate(Request);
            return Json(_dataobject.AutoComplete(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FieldChange()
        {
            DAL.SenMessageTemplate _dataobject = new DAL.SenMessageTemplate(Request);
            return Json(_dataobject.FieldChange(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return PartialView(this._indexview, _dataobject.GetData());
        }

        public ActionResult Create(int? MessageTemplateId)
        {
            return PartialView(this._updateview, _dataobject.GetNew(MessageTemplateId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]//Không kiểm tra nội dung là thẻ html và script
        public ActionResult Create(Models.SenMessageTemplate collection)
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

        public ActionResult Edit(int MessageTemplateId)
        {
            return PartialView(this._updateview, _dataobject.GetEdit(MessageTemplateId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]//Không kiểm tra nội dung là thẻ html và script
        public ActionResult Edit(int MessageTemplateId, Models.SenMessageTemplate collection)
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

        public ActionResult Delete(int MessageTemplateId)
        {
            return PartialView(this._deleteview, _dataobject.GetDelete(MessageTemplateId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int MessageTemplateId, FormCollection collection)
        {
            try
            {
                _dataobject.Delete(MessageTemplateId);
                return RedirectToAction(this.ActionReturn());
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(this._deleteview, _dataobject.GetDelete(MessageTemplateId));
            }
        }
    }
}