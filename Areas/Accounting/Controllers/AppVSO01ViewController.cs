using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;

namespace WebApp.Areas.Accounting.Controllers
{
    public class AppVSO01ViewController : AppAccountingVoucherController
    {
        DAL.AppVSO01View _dataobject;
        protected override IActionInvoker CreateActionInvoker()
        {
            _dataobject = new DAL.AppVSO01View(Request);
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
            return Services.Report.Print.PrintVoucher<Models.AppVSO01PrintView>(this, id, sysreportid, reporttype, new System.Collections.Hashtable());
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

        public ActionResult CreateByPos(int createtype)
        {
            if (!_dataobject.sysbusinessrole.IsAdd) return PartialView(this._roleview);
            return PartialView(createtype);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateByPos(int createtype, FormCollection collection)
        {
            try
            {
                //@DateFrom datetime,
                //@DateTo datetime,
                //@VoucherDate datetime,
                //@CustomerID int,
                //@AccountID int,
                //@Description nvarchar(256)

                var DateFrom = DateTime.Parse(collection["DateFrom"].ToString());
                var DateTo = DateTime.Parse(collection["DateTo"].ToString());
                var VoucherDate = DateTime.Parse(collection["VoucherDate"].ToString());

                var CustomerCode = collection["CustomerCode"].ToString();
                var DisplayNumber = collection["DisplayNumber"].ToString();
                var Description = collection["Description"].ToString();

                if (string.IsNullOrEmpty(CustomerCode)) ModelState.AddModelError("CustomerCode", "Phải nhập mã khách ngầm định.");
                if (string.IsNullOrEmpty(DisplayNumber)) ModelState.AddModelError("DisplayNumber", "Phải nhập tài khoản nợ.");
                if (!ModelState.IsValid) throw new Exception("");

                var customerid = _dataobject._db.AppCustomerTables.Where(m => m.CustomerCode == CustomerCode).Single().CustomerID;
                var acountid = _dataobject._db.AppAccountTables.Where(m => m.DisplayNumber == DisplayNumber).Single().AccountID;


                var kq = _dataobject._db.Database.ExecuteSqlCommand(string.Format("sp_AppVSO0101LineViewSByKey3 '{0}','{1}','{2}',{3},{4},N'{5}'", DateFrom.ToString("yyyy-MM-dd HH:mm:ss"), DateTo.ToString("yyyy-MM-dd HH:mm:ss"), VoucherDate.ToString("yyyyMMdd"), customerid, acountid, Description));
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, ex);
                return PartialView(createtype);
                throw;
            }
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


        public ActionResult Create2(string datefrom, string dateto)
        {


            var dataline = _dataobject._db.Database.SqlQuery<Models.AppVSO01LineView>(string.Format("sp_AppVSO0101LineViewSByKey3 '{0}','{1}'", datefrom, dateto)).ToList();
            var newobject = _dataobject.GetNew(null);
            newobject.AppVSO01LineViews = dataline;
            return PartialView(this._updateview, newobject);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create2(Models.AppVSO01View collection)
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


        public ActionResult Create(long? id)
        {
            if (!_dataobject.sysbusinessrole.IsAdd) return PartialView(this._roleview);

            if (Request.Params["documentid"] != null)
            {
                long documentid = long.Parse(Request.Params["documentid"]);
                var dataline = _dataobject._db.Database.SqlQuery<Models.AppVSO01LineView>(string.Format("sp_AppVSO0101LineViewSByKey2 'ParentID={0}'", Request.Params["documentid"])).ToList();
                var datamaster = _dataobject._db.AppVSO05Views.Where(m => m.DocumentID == documentid).SingleOrDefault();

                var newobject = _dataobject.GetNew(id);
                if (datamaster != null)
                {
                    newobject.CustomerID = datamaster.CustomerID;
                    newobject.CustomerCode = datamaster.CustomerCode;
                    newobject.CustomerName = datamaster.CustomerName;

                    newobject.Address = datamaster.Address;
                    newobject.Contact = datamaster.Contact;
                    newobject.Description = datamaster.Description;

                    newobject.DisplayNumberDebit = datamaster.DisplayNumberDebit;
                    newobject.AccountDebitID = datamaster.AccountDebitID;

                    newobject.DueDate = datamaster.DueDate;

                    newobject.CurrencyID = datamaster.CurrencyID;
                    newobject.IsoCode = datamaster.IsoCode;
                    newobject.ExchangeRate = datamaster.ExchangeRate;

                    newobject.SumAmount = datamaster.SumAmount;
                    newobject.SumAmountFC = datamaster.SumAmountFC;

                    newobject.SumTotal = datamaster.SumTotal;
                    newobject.SumTotalFC = datamaster.SumTotalFC;
                }

                newobject.AppVSO01LineViews = dataline;
                return PartialView(this._updateview, newobject);
            }
            return PartialView(this._updateview, _dataobject.GetNew(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.AppVSO01View collection)
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
        public ActionResult Edit(long id, Models.AppVSO01View collection)
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