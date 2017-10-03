using System;
using System.Linq;
using System.Web.Mvc;
using WebApp.Areas.Accounting.Services;

namespace WebApp.Areas.Accounting.Controllers
{
    public class AppVGL01ViewController : AppAccountingVoucherController
    {
        DAL.AppVGL01View _dataobject;

        protected override IActionInvoker CreateActionInvoker()
        {
            _dataobject = new DAL.AppVGL01View(Request);
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
            return Services.Report.Print.PrintVoucher<Models.AppVGL01PrintView>(this, id, sysreportid, reporttype, new System.Collections.Hashtable());
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
            // TODO: Add insert logic here
            return RedirectToAction("Create", new { action_return = "Index", createtype = collection["createtype"], month = collection["month"], year = collection["year"] });

        }


        public ActionResult Create(long? id)
        {
            if (!_dataobject.sysbusinessrole.IsAdd) return PartialView(this._roleview);

            if (Request.Params["createtype"] != null)
            {
                int createtype = int.Parse(Request.Params["createtype"].ToString());
                int month = int.Parse(Request.Params["month"].ToString());
                int year = int.Parse(Request.Params["year"].ToString());
                string storename = string.Format("sp_AppVGL01LineViewSByBT09 {0},{1}",month,year);
                switch (createtype)
                {
                    case 1:
                        storename = string.Format("sp_AppVGL01LineViewSByBT01 {0},{1}", month, year);
                        break;
                    case 2:
                        storename = string.Format("sp_AppVGL01LineViewSByBT02 {0},{1}", month, year);
                        break;
                    case 4:
                        storename = string.Format("sp_AppVGL01LineViewSByBT09Construction {0},{1}", month, year);
                        break;
                    default:
                        break;
                }
                var line = _dataobject._db.Database.SqlQuery<Models.AppVGL01LineView>(storename).ToList();
                var master = _dataobject.GetNew(id);

                DateTime voucherdate = new DateTime(year, month, 1);
                master.VoucherDate = voucherdate.EndOfMonth();
                master.AppVGL01LineViews = line;
                //master.AppVGL01LineViewz = (from m in line select m.DocumentLineID).ToList();
                                               

                return PartialView(this._updateview, master);

            }

            return PartialView(this._updateview, _dataobject.GetNew(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.AppVGL01View collection)
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
        public ActionResult Edit(long id, Models.AppVGL01View collection)
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