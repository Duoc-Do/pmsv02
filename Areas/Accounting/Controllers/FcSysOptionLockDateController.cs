
using System;
using System.Linq;
using System.Web.Mvc;
using System.Linq.Dynamic;
using WebApp.Areas.Accounting.DAL;

namespace WebApp.Areas.Accounting.Controllers
{
    public class FcSysOptionLockDateController : AppAccountingListController
    {
        DAL.SysOption _dataobject;

        protected override IActionInvoker CreateActionInvoker()
        {
            _dataobject = new DAL.SysOption(Request);
            this.InitData(_dataobject);
            ViewBag.Title = string.Format("{0}", "Khóa số liệu");
            return base.CreateActionInvoker();
        }

        public ActionResult Index()
        {
            return RedirectToAction("Edit2");
        }

        public ActionResult Edit2()
        {
            bool _role = false;
            var appuser = Services.GlobalVariant.GetAppUser();
            if (appuser.RoleName == "Systems" || appuser.RoleName == "Administrators") _role = true;
            if (!_dataobject.sysbusinessrole.IsEdit || !_role) return PartialView(this._roleview);

            string rolename = Services.GlobalVariant.GetAppUser().RoleName;
            if (!"Systems.Administrators".Contains(rolename)) return PartialView(this._roleview);
            var result = _dataobject._db.Database.SqlQuery<Models.SysOption>(
                @"select * from SysOption 
                where SysOptionName in 
                (
                'LockDate'
                )").ToList();
            return PartialView("Update2", result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit2(FormCollection collection)
        {
            string SysOptionName = "LockDate";//collection["SysOptionName"].ToString();
            var data = _dataobject._db.SysOptions.Where(m => m.SysOptionName == SysOptionName).SingleOrDefault();
            data.SysOptionValue = collection["SysOptionValue"].ToString();

            //FcSysOptionLockDateMessage
            try
            {
                int outputId = _dataobject.Update(data);
                ViewBag.FcSysOptionLockDateMessage = "succeeded";
                //return Content("succeeded");
            }
            catch (Exception ex)
            {
                //return Content(string.Format("failed: {0}", ex.Message));
                ModelState.AddModelError("", ex.Message);
            }
            string rolename = Services.GlobalVariant.GetAppUser().RoleName;
            if (!"Systems.Administrators".Contains(rolename)) return PartialView(this._roleview);
            var result = _dataobject._db.Database.SqlQuery<Models.SysOption>(
                @"select * from SysOption 
                where SysOptionName in 
                (
                'LockDate'
                )").ToList();
            return PartialView("Update2", result);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection collection)
        {
            //Xử lý post

            var result = _dataobject._db.Database.SqlQuery<SysOption>(
                @"select * from SysOption 
                    where SysOptionName in 
                    (
                    'RoundQuantity',
                    'RoundAmount',
                    'RoundAmountFC',
                    'RoundUnitPrice',
                    'RoundUnitPriceFC',
                    'DirectorName',
                    'ChiefAccountantName',
                    'ReportCreatorName'
                    )").ToList();

            return PartialView("Update2", result);
        }


        public ActionResult Create(int? id)
        {
            if (!_dataobject.sysbusinessrole.IsAdd) return PartialView(this._roleview);
            return PartialView(this._updateview, _dataobject.GetNew(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.SysOption collection)
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

        public ActionResult Edit(int id)
        {
            if (!_dataobject.sysbusinessrole.IsEdit) return PartialView(this._roleview);
            return PartialView(this._updateview, _dataobject.GetEdit(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Models.SysOption collection)
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

        public ActionResult Delete(int id)
        {
            if (!_dataobject.sysbusinessrole.IsDelete) return PartialView(this._roleview);
            return PartialView(this._deleteview, _dataobject.GetDelete(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
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