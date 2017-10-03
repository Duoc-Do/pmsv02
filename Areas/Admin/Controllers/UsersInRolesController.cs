using System;
using System.Web.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    public class UsersInRolesController : AppAdminListController
    {
        DAL.aspnet_UsersInRolesView _dataobject;

        protected override IActionInvoker CreateActionInvoker()
        {
            _dataobject = new DAL.aspnet_UsersInRolesView(Request);
            this.InitData(_dataobject);
            return base.CreateActionInvoker();
        }

        public ActionResult AutoComplete()
        {
            DAL.aspnet_UsersInRolesView _dataobject = new DAL.aspnet_UsersInRolesView(Request);
            return Json(_dataobject.AutoComplete(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FieldChange()
        {
            DAL.aspnet_UsersInRolesView _dataobject = new DAL.aspnet_UsersInRolesView(Request);
            return Json(_dataobject.FieldChange(), JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Index()
        {
            return PartialView(this._indexview, _dataobject.GetData());
        }

        public ActionResult Create(Guid? UserId, Guid? RoleId)
        {
            //if (!_dataobject.sysbusinessrole.IsAdd) return PartialView(this._roleview);        
            return PartialView(this._updateview, _dataobject.GetNew(UserId, RoleId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.aspnet_UsersInRolesView collection)
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

        public ActionResult Edit(Guid UserId, Guid RoleId)
        {
            //if (!_dataobject.sysbusinessrole.IsEdit) return PartialView(this._roleview);        
            return PartialView(this._updateview, _dataobject.GetEdit(UserId, RoleId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid UserId, Guid RoleId, Models.aspnet_UsersInRolesView collection)
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

        public ActionResult Delete(Guid UserId, Guid RoleId)
        {
            //if (!_dataobject.sysbusinessrole.IsDelete) return PartialView(this._roleview);        
            return PartialView(this._deleteview, _dataobject.GetDelete( UserId,  RoleId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid UserId, Guid RoleId, FormCollection collection)
        {
            try
            {
                _dataobject.Delete( UserId,  RoleId);
                return RedirectToAction(this.ActionReturn()); 
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(this._deleteview, _dataobject.GetDelete( UserId,  RoleId));
            }
        }
    }
}