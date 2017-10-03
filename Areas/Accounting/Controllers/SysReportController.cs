using System;
using System.Web.Mvc;

namespace WebApp.Areas.Accounting.Controllers
{
    public class SysReportController : AppAccountingListController
    {
        DAL.SysReport _dataobject;

        protected override IActionInvoker CreateActionInvoker()
        {
            _dataobject = new DAL.SysReport(Request);
            this.InitData(_dataobject);
            return base.CreateActionInvoker();
        }

        public ActionResult Index(string businesscode)
        {
            if (string.IsNullOrEmpty(businesscode))
            {
                return PartialView(_dataobject.GetData());

            }
            return PartialView(_dataobject.GetData(businesscode));
        }

        public ActionResult Create(int? sysreportid)
        {
            if (!_dataobject.sysbusinessrole.IsAdd) return PartialView(this._roleview);
            return PartialView(this._updateview, _dataobject.GetNew(sysreportid));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.SysReport collection)
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

        public ActionResult Edit(int sysreportid)
        {

            if (!_dataobject.sysbusinessrole.IsEdit) return PartialView(this._roleview);
            return PartialView(this._updateview, _dataobject.GetEdit(sysreportid));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int sysreportid, Models.SysReport collection)
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

        public ActionResult Delete(int sysreportid)
        {
            if (!_dataobject.sysbusinessrole.IsDelete) return PartialView(this._roleview);
            return PartialView(this._deleteview, _dataobject.GetDelete(sysreportid));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int sysreportid, FormCollection collection)
        {
            try
            {
                _dataobject.Delete(sysreportid);
                return RedirectToAction(this.ActionReturn());
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(this._deleteview, _dataobject.GetDelete(sysreportid));
            }
        }
    }
}