using System;
using System.Linq;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace WebApp.Areas.Admin.Controllers
{
    public class SenContractCashController : AppAdminListController
    {
        DAL.SenContractCash _dataobject;

        protected override IActionInvoker CreateActionInvoker()
        {
            _dataobject = new DAL.SenContractCash(Request);
            this.InitData(_dataobject);
            return base.CreateActionInvoker();
        }

        public ActionResult AutoComplete()
        {
            DAL.SenContractCash _dataobject = new DAL.SenContractCash(Request);
            return Json(_dataobject.AutoComplete(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FieldChange()
        {
            DAL.SenContractCash _dataobject = new DAL.SenContractCash(Request);
            return Json(_dataobject.FieldChange(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return PartialView(_dataobject.GetData());
        }

        public ActionResult Create(int? ContractCashId, int? ContractId)
        {
            var model = _dataobject.GetNew(ContractCashId);
            if (ContractId!=null)
            {
                var sencontract = _dataobject._db.SenContracts.Where(m => m.ContractId == ContractId).SingleOrDefault();
                if (sencontract!=null)
                {
                    model.ContractId = sencontract.ContractId;
                    model.Amount = sencontract.Amount-sencontract.Payment;
                }
                
            }
            return PartialView(this._updateview, model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.SenContractCash collection)
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

        public ActionResult Edit(int ContractCashId)
        {
            return PartialView(this._updateview, _dataobject.GetEdit(ContractCashId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int ContractCashId, Models.SenContractCash collection)
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

        public ActionResult Delete(int ContractCashId)
        {
            return PartialView(this._deleteview, _dataobject.GetDelete(ContractCashId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int ContractCashId, FormCollection collection)
        {
            try
            {
                _dataobject.Delete(ContractCashId);
                return RedirectToAction(this.ActionReturn());
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(this._deleteview, _dataobject.GetDelete(ContractCashId));
            }
        }
    }
}