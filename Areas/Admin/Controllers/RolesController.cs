
using System;
using System.Web.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    public class RolesController : AppAdminListController
    {
        DAL.aspnet_Roles _dataobject;

        protected override IActionInvoker CreateActionInvoker()
        {
            _dataobject = new DAL.aspnet_Roles(Request); 
            this.InitData(_dataobject);
            return base.CreateActionInvoker();
        }

        public ActionResult AutoComplete()
        {
            DAL.aspnet_Roles _dataobject = new DAL.aspnet_Roles(Request);
            return Json(_dataobject.AutoComplete(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FieldChange()
        {
            DAL.aspnet_Roles _dataobject = new DAL.aspnet_Roles(Request);
            return Json(_dataobject.FieldChange(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return PartialView(this._indexview, _dataobject.GetData());
        }

        public ActionResult Create(Guid? RoleId)
        {
            //if (!_dataobject.sysbusinessrole.IsAdd) return PartialView(this._roleview);
            return PartialView(this._updateview, _dataobject.GetNew(RoleId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.aspnet_Roles collection)
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

        public ActionResult Edit(Guid RoleId)
        {
            //if (!_dataobject.sysbusinessrole.IsEdit) return PartialView(this._roleview);
            return PartialView(this._updateview, _dataobject.GetEdit(RoleId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid RoleId, Models.aspnet_Roles collection)
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

        public ActionResult Delete(Guid RoleId)
        {
            //if (!_dataobject.sysbusinessrole.IsDelete) return PartialView(this._roleview);
            return PartialView(this._deleteview, _dataobject.GetDelete(RoleId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid RoleId, FormCollection collection)
        {
            try
            {
                _dataobject.Delete(RoleId);
                return RedirectToAction(this.ActionReturn());
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(this._deleteview, _dataobject.GetDelete(RoleId));
            }
        }
    }
}
