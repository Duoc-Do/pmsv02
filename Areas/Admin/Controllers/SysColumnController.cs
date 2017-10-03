using System;
using System.Web.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    public class SysColumnController : AppAdminListController
    {
        DAL.SysColumn _dataobject;

        protected override IActionInvoker CreateActionInvoker()
        {
            _dataobject = new DAL.SysColumn(Request);
            this.InitData(_dataobject);
            return base.CreateActionInvoker();
        }

        public ActionResult AutoComplete()
        {
            DAL.SysColumn _dataobject = new DAL.SysColumn(Request);
            return Json(_dataobject.AutoComplete(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FieldChange()
        {
            DAL.SysColumn _dataobject = new DAL.SysColumn(Request);
            return Json(_dataobject.FieldChange(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            
            return PartialView(this._indexview, _dataobject.GetData());
        }

        public ActionResult Create(string Name)
        {
            //if (!_dataobject.sysbusinessrole.IsAdd) return PartialView(this._roleview);        
            return PartialView(this._updateview, _dataobject.GetNew(Name));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.SysColumn collection)
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

        public ActionResult Edit(string Name)
        {
            //if (!_dataobject.sysbusinessrole.IsEdit) return PartialView(this._roleview);        
            return PartialView(this._updateview, _dataobject.GetEdit(Name));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string Name, Models.SysColumn collection)
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

        public ActionResult Delete(string Name)
        {
            return PartialView(this._deleteview, _dataobject.GetDelete(Name));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string Name, FormCollection collection)
        {
            try
            {
                _dataobject.Delete(Name);
                return RedirectToAction(this.ActionReturn()); 
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(this._deleteview, _dataobject.GetDelete(Name));
            }
        }
    }
}