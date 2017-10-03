using System;
using System.Linq;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace WebApp.Areas.Admin.Controllers
{
    public class SenCommissionController : AppAdminListController
    {
        DAL.SenCommission _dataobject;

        protected override IActionInvoker CreateActionInvoker()
        {
            _dataobject = new DAL.SenCommission(Request);
            this.InitData(_dataobject);
            return base.CreateActionInvoker();
        }

        public ActionResult AutoComplete()
        {
            DAL.SenCommission _dataobject = new DAL.SenCommission(Request);
            return Json(_dataobject.AutoComplete(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FieldChange()
        {
            DAL.SenCommission _dataobject = new DAL.SenCommission(Request);
            return Json(_dataobject.FieldChange(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return PartialView(_dataobject.GetData());
        }

        public ActionResult Create(int? CommissionId, int? ContractId)
        {
            var model = _dataobject.GetNew(CommissionId);
            if (ContractId != null)
            {
                var sencontract = _dataobject._db.SenContracts.Where(m => m.ContractId == ContractId).SingleOrDefault();
                if (sencontract != null)
                {
                    model.ContractId = sencontract.ContractId;
                    model.Amount = sencontract.CommissionPayment??0;
                }

            }
            return PartialView(this._updateview, model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.SenCommission collection)
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

        public ActionResult Edit(int CommissionId)
        {
            return PartialView(this._updateview, _dataobject.GetEdit(CommissionId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int CommissionId, Models.SenCommission collection)
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

        public ActionResult Delete(int CommissionId)
        {
            return PartialView(this._deleteview, _dataobject.GetDelete(CommissionId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int CommissionId, FormCollection collection)
        {
            try
            {
                _dataobject.Delete(CommissionId);
                return RedirectToAction(this.ActionReturn());
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(this._deleteview, _dataobject.GetDelete(CommissionId));
            }
        }
    }
}