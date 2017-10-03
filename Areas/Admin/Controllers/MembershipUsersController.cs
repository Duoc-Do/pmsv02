
using System;
using System.Web.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    public class MembershipUsersController : AppAdminListController
    {
        DAL.vw_aspnet_MembershipUsers _dataobject;

        protected override IActionInvoker CreateActionInvoker()
        {
            _dataobject = new DAL.vw_aspnet_MembershipUsers(Request);
            this.InitData(_dataobject);

            ViewBag.ajaxoption = _dataobject.appajaxoption.ajaxoption;

            return base.CreateActionInvoker();
        }

        public ActionResult AutoComplete()
        {
            DAL.vw_aspnet_MembershipUsers _dataobject = new DAL.vw_aspnet_MembershipUsers(Request);
            return Json(_dataobject.AutoComplete(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FieldChange()
        {
            DAL.vw_aspnet_MembershipUsers _dataobject = new DAL.vw_aspnet_MembershipUsers(Request);
            return Json(_dataobject.FieldChange(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return PartialView(_dataobject.GetData());
        }

        public ActionResult AddCash(Guid UserId)
        {
            return PartialView(UserId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCash(Guid UserId, FormCollection formcontext)
        {
            try
            {
                var _senuserobject = new DAL.SenUser(Request);
                var _senusercashobject = new DAL.SenUserCash(Request);

                decimal amount = Services.ExConvert.String2Decimal(formcontext["Amount"]);
                _senuserobject.AddCash(UserId, amount);

                var senusercash = _senusercashobject.GetNew(null);
                senusercash.UserId = UserId;
                senusercash.VoucherDate = DateTime.UtcNow;
                senusercash.CreateDate = DateTime.UtcNow;
                senusercash.Amount = amount;
                senusercash.Description = "Ứng tiền";
                senusercash.CashType = 0;
                senusercash.UserName = User.Identity.Name;

                _senusercashobject.Insert(senusercash);

                #region email thong bao
                
                #endregion



                if (this.Request["action_return"] != null) return Redirect(this.Request["action_return"]);

                return Json(new { ketqua = "success" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("sumerror", "Lỗi.");
                if (this.Request["action_return"] != null) return Redirect(this.Request["action_return"]);
                //return Content("failed");
                return Json(new { ketqua = ex.Message });
            }

        }



        public ActionResult Create(Guid? UserId)
        {
            //if (!_dataobject.sysbusinessrole.IsAdd) return PartialView(this._roleview);
            return PartialView(this._updateview, _dataobject.GetNew(UserId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.vw_aspnet_MembershipUsers collection)
        {
            try
            {
                int outputId = _dataobject.Insert(collection);
                return RedirectToAction(this.ActionReturn());
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                collection.Roles = _dataobject.GetAllRoles();
                return PartialView(this._updateview, collection);
            }
        }

        public ActionResult Edit(Guid UserId)
        {
            //if (!_dataobject.sysbusinessrole.IsEdit) return PartialView(this._roleview);
            return PartialView(this._updateview, _dataobject.GetEdit(UserId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid UserId, Models.vw_aspnet_MembershipUsers collection)
        {
            try
            {

                int outputId = _dataobject.Update(collection);
                return RedirectToAction(this.ActionReturn());
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                collection.Roles = _dataobject.GetAllRoles();
                return PartialView(this._updateview, collection);
            }
        }

        public ActionResult Delete(Guid UserId)
        {
            //if (!_dataobject.sysbusinessrole.IsDelete) return PartialView(this._roleview);
            return PartialView(this._deleteview, _dataobject.GetDelete(UserId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid UserId, FormCollection collection)
        {
            try
            {
                _dataobject.Delete(UserId);
                return RedirectToAction(this.ActionReturn());
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(this._deleteview, _dataobject.GetDelete(UserId));
            }
        }
    }
}