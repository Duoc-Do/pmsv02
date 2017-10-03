
using System;
using System.Web.Mvc;

namespace WebApp.Areas.Accounting.Controllers
{
    public class AppVIN03ViewController : AppAccountingVoucherController
    {
        DAL.AppVIN03View _dataobject;

        protected override IActionInvoker CreateActionInvoker()
        {
            _dataobject = new DAL.AppVIN03View(Request);
            this.InitData(_dataobject);
            return base.CreateActionInvoker();
        }
        public ActionResult AutoComplete()
        {
            return Json(_dataobject.AutoComplete(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult FieldChange()
        {
            return Json(_dataobject.FieldChange(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Print(long id, int sysreportid, string reporttype = "PDF")
        {
            return Services.Report.Print.PrintVoucher<Models.AppVIN03PrintView>(this, id, sysreportid, reporttype, new System.Collections.Hashtable());
        }
        public ActionResult Index()
        {
            return PartialView(this._indexview, _dataobject.GetData());
        }
        public ActionResult Create(long? id)
        {
            if (!_dataobject.sysbusinessrole.IsAdd) return PartialView(this._roleview);
            return PartialView(this._updateview, _dataobject.GetNew(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.AppVIN03View collection)
        {
            // TODO: Add insert logic here
            try
            {
                long outputId = _dataobject.Insert(collection);
                return RedirectToAction(this.ActionReturn());
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(this._updateview, collection);
            }

        }
        public ActionResult Edit(long id)
        {
            if (!_dataobject.sysbusinessrole.IsEdit) return PartialView(this._roleview);
            return PartialView(this._updateview, _dataobject.GetEdit(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(long id, Models.AppVIN03View collection)
        {
            // TODO: Add insert logic here
            try
            {
                long outputId = _dataobject.Update(collection);
                return RedirectToAction(this.ActionReturn());
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(this._updateview, collection);
            }
        }

        public ActionResult Delete(long id)
        {
            if (!_dataobject.sysbusinessrole.IsDelete) return PartialView(this._roleview);
            return PartialView(this._deleteview, _dataobject.GetDelete(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(long id, FormCollection collection)
        {
            try
            {
                _dataobject.Delete(id);
                return RedirectToAction(this.ActionReturn());
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(this._deleteview, _dataobject.GetDelete(id));
            }
        }
    }
}


//----- Phần Update
//@Html.svHiddenFor(model => model.VoucherID)//@Html.svHiddenFor(model => model.ParentID)//@Html.svHiddenFor(model => model.DocumentID)//@Html.svHiddenFor(model => model.CustomerID)//@Html.svHiddenFor(model => model.CurrencyID)//@Html.bsEditorFor(model => model.VoucherNumber)//@Html.bsEditorFor(model => model.VoucherName)//@Html.bsEditorFor(model => model.VoucherDate)//@Html.bsEditorFor(model => model.VoucherCode)//@Html.bsEditorFor(model => model.SumQuantity0)//@Html.bsEditorFor(model => model.SumAmountFC)//@Html.bsEditorFor(model => model.SumAmount)//@Html.bsEditorFor(model => model.PostType)//@Html.bsEditorFor(model => model.IsoCode)//@Html.bsEditorFor(model => model.IsFixPrice)//@Html.bsEditorFor(model => model.ExchangeRate)//@Html.bsEditorFor(model => model.Description)//@Html.bsEditorFor(model => model.CustomerName)//@Html.bsEditorFor(model => model.CustomerCode)//@Html.bsEditorFor(model => model.Contact)//@Html.bsEditorFor(model => model.Address)
