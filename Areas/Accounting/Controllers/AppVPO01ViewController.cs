using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;

namespace WebApp.Areas.Accounting.Controllers
{
    public class AppVPO01ViewController : AppAccountingVoucherController
    {
        DAL.AppVPO01View _dataobject;

        protected override IActionInvoker CreateActionInvoker()
        {
            _dataobject = new DAL.AppVPO01View(Request);
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
            return Services.Report.Print.PrintVoucher<Models.AppVPO01PrintView>(this, id, sysreportid, reporttype, new System.Collections.Hashtable());
        }
        public ActionResult Index()
        {
            return PartialView(_dataobject.GetData());
            //return PartialView(this._indexview, _dataobject.GetData());
        }

        public ActionResult CreateBy(int createtype)
        {
            if (!_dataobject.sysbusinessrole.IsAdd) return PartialView(this._roleview);
            return PartialView(createtype);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBy(FormCollection collection)
        {
            var DateFrom = DateTime.Parse(collection["DateFrom"].ToString());
            var DateTo = DateTime.Parse(collection["DateTo"].ToString());
            var VoucherNumber = collection["VoucherNumber"];
            var datavoucher = _dataobject._db.AppDocumentTables.Where(m => m.VoucherDate >= DateFrom && m.VoucherDate <= DateTo && m.VoucherNumber == VoucherNumber).FirstOrDefault();
            long DocumentID = 0;
            if (datavoucher != null) { DocumentID = datavoucher.DocumentID; }

            // TODO: Add insert logic here
            return RedirectToAction("Create", new { action_return = "Index", documentid = DocumentID });
        }


        public ActionResult Create(long? id)
        {
            if (!_dataobject.sysbusinessrole.IsAdd) return PartialView(this._roleview);
            if (Request.Params["documentid"] != null)
            {
                long documentid = long.Parse(Request.Params["documentid"]);
                var dataline = _dataobject._db.Database.SqlQuery<Models.AppVPO01LineView>(string.Format("sp_AppVPO0101LineViewSByKey2 'ParentID={0}'", Request.Params["documentid"])).ToList();
                var datamaster = _dataobject._db.AppVPO06Views.Where(m => m.DocumentID == documentid).SingleOrDefault();

                var newobject = _dataobject.GetNew(id);
                if (datamaster != null)
                {

                    newobject.CustomerID = datamaster.CustomerID;
                    newobject.CustomerCode = datamaster.CustomerCode;
                    newobject.CustomerName = datamaster.CustomerName;

                    newobject.Address = datamaster.Address;
                    newobject.Contact = datamaster.Contact;
                    newobject.Description = datamaster.Description;

                    newobject.DisplayNumberCredit = datamaster.DisplayNumberCredit;
                    newobject.AccountCreditID = datamaster.AccountCreditID;

                    newobject.DueDate = datamaster.DueDate;

                    newobject.CurrencyID = datamaster.CurrencyID;
                    newobject.IsoCode = datamaster.IsoCode;
                    newobject.ExchangeRate = datamaster.ExchangeRate;

                    newobject.SumAmount = datamaster.SumAmount;
                    newobject.SumAmountFC = datamaster.SumAmountFC;

                    newobject.SumTotal = datamaster.SumTotal;
                    newobject.SumTotalFC = datamaster.SumTotalFC;
                }


                newobject.AppVPO01LineViews = dataline;
                return PartialView(this._updateview, newobject);
            }
            return PartialView(this._updateview, _dataobject.GetNew(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.AppVPO01View collection)
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
        public ActionResult Edit(long id, Models.AppVPO01View collection)
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

//@Html.svHiddenFor(model => model.VoucherID)
//@Html.svHiddenFor(model => model.ParentID)
//@Html.svHiddenFor(model => model.DocumentID)
//@Html.svHiddenFor(model => model.CustomerID)
//@Html.svHiddenFor(model => model.CurrencyID)
//@Html.svHiddenFor(model => model.AccountCreditID)
//@Html.bsEditorFor(model => model.VoucherNumber)
//@Html.bsEditorFor(model => model.VoucherName)
//@Html.bsEditorFor(model => model.VoucherDate)
//@Html.bsEditorFor(model => model.VoucherCode)
//@Html.bsEditorFor(model => model.SumTotalFC)
//@Html.bsEditorFor(model => model.SumTotal)
//@Html.bsEditorFor(model => model.SumQuantity0)
//@Html.bsEditorFor(model => model.SumDiscountFC)
//@Html.bsEditorFor(model => model.SumDiscount)
//@Html.bsEditorFor(model => model.SumAmountVATFC)
//@Html.bsEditorFor(model => model.SumAmountVAT)
//@Html.bsEditorFor(model => model.SumAmountFC)
//@Html.bsEditorFor(model => model.SumAmountCostFC)
//@Html.bsEditorFor(model => model.SumAmountCost)
//@Html.bsEditorFor(model => model.SumAmount)
//@Html.bsEditorFor(model => model.PostType)
//@Html.bsEditorFor(model => model.IsoCode)
//@Html.bsEditorFor(model => model.ExchangeRate)
//@Html.bsEditorFor(model => model.DueDate)
//@Html.bsEditorFor(model => model.DisplayNumberCredit)
//@Html.bsEditorFor(model => model.Description)
//@Html.bsEditorFor(model => model.CustomerName)
//@Html.bsEditorFor(model => model.CustomerCode)
//@Html.bsEditorFor(model => model.Contact)
//@Html.bsEditorFor(model => model.Address)
