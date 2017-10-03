using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;

namespace WebApp.Areas.Accounting.Controllers
{
    public class AppVCA02ViewController : AppAccountingVoucherController
    {
        DAL.AppVCA02View _dataobject;

        protected override IActionInvoker CreateActionInvoker()
        {
            _dataobject = new DAL.AppVCA02View(Request);
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
            return Services.Report.Print.PrintVoucher<Models.AppVCA02PrintView>(this, id, sysreportid, reporttype, new System.Collections.Hashtable());
        }
        public ActionResult Index()
        {
            return PartialView(this._indexview, _dataobject.GetData());
        }
        public ActionResult Create(long? id)
        {
            if (!_dataobject.sysbusinessrole.IsAdd) return PartialView(this._roleview);
            if (Request.Params["documentid"] != null)
            {
                long documentid = long.Parse(Request.Params["documentid"]);
                var dataline = _dataobject._db.Database.SqlQuery<Models.AppVCA02LineView>(string.Format("sp_AppVCA0201LineViewSByKey2 'DocumentID={0}'", Request.Params["documentid"])).ToList();
                var datamaster = _dataobject._db.AppVPO01Views.Where(m => m.DocumentID == documentid).SingleOrDefault();

                var newobject = _dataobject.GetNew(id);
                if (datamaster != null)
                {
                    newobject.CustomerID = datamaster.CustomerID;
                    newobject.CustomerCode = datamaster.CustomerCode;
                    newobject.CustomerName = datamaster.CustomerName;

                    newobject.Address = datamaster.Address;
                    newobject.Contact = datamaster.Contact;
                    //newobject.Description = datamaster.Description;

                    //newobject.DisplayNumberDebit = datamaster.DisplayNumberDebit;
                    //newobject.AccountDebitID = datamaster.AccountDebitID;

                    //newobject.DueDate = datamaster.DueDate;

                    newobject.CurrencyID = datamaster.CurrencyID;
                    newobject.IsoCode = datamaster.IsoCode;
                    newobject.ExchangeRate = datamaster.ExchangeRate;

                    newobject.SumAmount = datamaster.SumAmount;
                    newobject.SumAmountFC = datamaster.SumAmountFC;

                    newobject.SumTotal = datamaster.SumTotal;
                    newobject.SumTotalFC = datamaster.SumTotalFC;
                }

                newobject.AppVCA02LineViews = dataline;
                return PartialView(this._updateview, newobject);
            }
            return PartialView(this._updateview, _dataobject.GetNew(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.AppVCA02View collection)
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
        public ActionResult Edit(long id, Models.AppVCA02View collection)
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