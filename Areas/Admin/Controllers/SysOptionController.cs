
using System;
using System.Linq;
using System.Web.Mvc;
using System.Linq.Dynamic;
using WebApp.Areas.Admin.DAL;

namespace WebApp.Areas.Admin.Controllers
{
    public class SysOptionController : AppAdminListController
    {
        DAL.SysOption _dataobject;

        protected override IActionInvoker CreateActionInvoker()
        {
            _dataobject = new DAL.SysOption(Request);
            this.InitData(_dataobject);
            return base.CreateActionInvoker();
        }

        public ActionResult Index2()
        {
            return PartialView(this._indexview, _dataobject.GetData());
            //return RedirectToAction("Edit2");
        }

        public ActionResult Index()
        {
            //return PartialView(this._indexview, _dataobject.GetData());
            return RedirectToAction("Edit2");
        }

        public ActionResult Edit2()
        {
            var result = _dataobject._db.Database.SqlQuery<Models.SysOption>(
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
'ReportCreatorName',
'RoleFilter'
)").ToList();

            return PartialView("Update2", result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit2(FormCollection collection)
        {
            string SysOptionName = collection["SysOptionName"].ToString();
            var data = _dataobject._db.SysOptions.Where(m => m.SysOptionName == SysOptionName).SingleOrDefault();
            if (collection["SysOptionValue"] != null)
            {
                data.SysOptionValue = collection["SysOptionValue"].ToString();

            }
            else
            {
                data.SysOptionValue = collection["SysOptionValue[]"].ToString();
            }
            try
            {
                int outputId = _dataobject.Update(data);
                return Content("succeeded");
            }
            catch (Exception ex)
            {
                return Content(string.Format("failed: {0}", ex.Message));
            }
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
'ReportCreatorName',
'RoleFilter'
)").ToList();

            return PartialView("Update2", result);
        }


        public ActionResult Create(int? id)
        {
            //if (!_dataobject.sysbusinessrole.IsAdd) return PartialView(this._roleview);
            return PartialView(this._updateview, _dataobject.GetNew(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.SysOption collection)
        {
            try
            {
                int outputId = _dataobject.Insert(collection);
                return RedirectToAction("Index2");
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(this._updateview, collection);
            }
        }

        public ActionResult Edit(int id)
        {
            //if (!_dataobject.sysbusinessrole.IsEdit) return PartialView(this._roleview);
            return PartialView(this._updateview, _dataobject.GetEdit(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Models.SysOption collection)
        {
            try
            {
                int outputId = _dataobject.Update(collection);
                return RedirectToAction("Index2");
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(this._updateview, collection);
            }
        }

        public ActionResult Delete(int id)
        {
            //if (!_dataobject.sysbusinessrole.IsDelete) return PartialView(this._roleview);
            return PartialView(this._deleteview, _dataobject.GetDelete(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                _dataobject.Delete(id);
                return RedirectToAction("Index2");
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(this._deleteview, _dataobject.GetDelete(id));
            }
        }
    }
}