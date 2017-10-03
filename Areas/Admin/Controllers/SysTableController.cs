using System;
using System.Web.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    public class SysTableController : AppAdminListController
    {
        DAL.SysTable _dataobject;

        protected override IActionInvoker CreateActionInvoker()
        {
            _dataobject = new DAL.SysTable(Request);
            this.InitData(_dataobject);
            return base.CreateActionInvoker();
        }

        public ActionResult AutoComplete()
        {
            DAL.SysTable _dataobject = new DAL.SysTable(Request);
            return Json(_dataobject.AutoComplete(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FieldChange()
        {
            DAL.SysTable _dataobject = new DAL.SysTable(Request);
            return Json(_dataobject.FieldChange(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            
            return PartialView(this._indexview, _dataobject.GetData());
        }

        public ActionResult Create(string TableName)
        {
            //if (!_dataobject.sysbusinessrole.IsAdd) return PartialView(this._roleview);        
            return PartialView(this._updateview, _dataobject.GetNew(TableName));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.SysTable collection)
        {
            try
            {
                string outputId = _dataobject.Insert(collection);
                return RedirectToAction(this.ActionReturn());
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(this._updateview, collection);
            }
        }

        public ActionResult Edit(string TableName)
        {
            //if (!_dataobject.sysbusinessrole.IsEdit) return PartialView(this._roleview);        
            return PartialView(this._updateview, _dataobject.GetEdit(TableName));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string TableName, Models.SysTable collection)
        {
            
            try
            {
                string outputId = _dataobject.Update(collection);
                return RedirectToAction(this.ActionReturn());
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(this._updateview, collection);
            }
        }

        public ActionResult Delete(string TableName)
        {
            return PartialView(this._deleteview, _dataobject.GetDelete(TableName));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string TableName, FormCollection collection)
        {
            try
            {
                _dataobject.Delete(TableName);
                return RedirectToAction(this.ActionReturn()); 
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(this._deleteview, _dataobject.GetDelete(TableName));
            }
        }
    }
}