using System;
using System.Web.Mvc;


namespace WebApp.Areas.Accounting.Controllers
{
    public class AppAccountTableController : AppAccountingListController
    {
        DAL.AppAccountTable _dataobject;

        protected override IActionInvoker CreateActionInvoker()
        {
            _dataobject = new DAL.AppAccountTable(Request);
            this.InitData(_dataobject);
            return base.CreateActionInvoker();
        }

        //protected override IActionInvoker CreateActionInvoker()
        //{
        //    _dataobject = new DAL.AppAccountTable(Request);
        //    ViewBag.ajaxoption = _dataobject.appajaxoption.ajaxoption;
        //    ViewBag.Title = string.Format("Danh sách {0}", _dataobject.sysbusiness.Des.ToLower());

        //    string actionname = RouteData.Values["action"].ToString().ToLower();
        //    if (actionname == "delete")
        //    { ViewBag.Title = string.Format("Xóa {0}", _dataobject.sysbusiness.Des.ToLower()); }
        //    if (actionname == "create")
        //    { ViewBag.Title = string.Format("Tạo {0}", _dataobject.sysbusiness.Des.ToLower()); }
        //    if (actionname == "edit")
        //    { ViewBag.Title = string.Format("Sửa {0}", _dataobject.sysbusiness.Des.ToLower()); }

        //    return base.CreateActionInvoker();
        //}
        //
        // GET: /CRM/AppAccountTable/

        public ActionResult AutoComplete()
        {
            DAL.AppCustomerTable _dataobject = new DAL.AppCustomerTable(Request);
            return Json(_dataobject.AutoComplete(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FieldChange()
        {
            DAL.AppCustomerTable _dataobject = new DAL.AppCustomerTable(Request);
            return Json(_dataobject.FieldChange(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return PartialView(this._indexview, _dataobject.GetData());
        }

        //
        // GET: /CRM/AppAccountTable/Create

        public ActionResult Create(int? id)
        {
            if (!_dataobject.sysbusinessrole.IsAdd) return PartialView(this._roleview);
            return PartialView(this._updateview, _dataobject.GetNew(id));
        }

        //
        // POST: /CRM/AppAccountTable/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.AppAccountView collection)
        {
            // TODO: Add insert logic here
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

        //
        // GET: /CRM/AppAccountTable/Edit/5

        public ActionResult Edit(int id)
        {
            if (!_dataobject.sysbusinessrole.IsEdit) return PartialView(this._roleview);
            return PartialView(this._updateview, _dataobject.GetEdit(id));
        }

        //
        // POST: /CRM/AppAccountTable/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Models.AppAccountView collection)
        {
            // TODO: Add insert logic here
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

        //
        // GET: /CRM/AppAccountTable/Delete/5

        public ActionResult Delete(int id)
        {
            if (!_dataobject.sysbusinessrole.IsDelete) return PartialView(this._roleview);
            return PartialView(this._deleteview, _dataobject.GetDelete(id));
        }

        //
        // POST: /CRM/AppAccountTable/Delete/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            // TODO: Add insert logic here
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
