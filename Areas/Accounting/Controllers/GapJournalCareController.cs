using System;
using System.Linq;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace WebApp.Areas.Accounting.Controllers
{
    public class GapJournalCareController : AppAccountingListController
    {
        DAL.GapJournalCare _dataobject;

        protected override IActionInvoker CreateActionInvoker()
        {
            _dataobject = new DAL.GapJournalCare(Request);
            this.InitData(_dataobject);
            return base.CreateActionInvoker();
        }

        public ActionResult AutoComplete()
        {
            DAL.GapJournalCare _dataobject = new DAL.GapJournalCare(Request);
            return Json(_dataobject.AutoComplete(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FieldChange()
        {
            DAL.GapJournalCare _dataobject = new DAL.GapJournalCare(Request);
            return Json(_dataobject.FieldChange(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return PartialView(this._indexview, _dataobject.GetData());
        }


        public ActionResult Create2(int? id, int[] fieldid)
        {
            var model = _dataobject.GetNew(id);
            ViewBag.fieldid = fieldid;
            return PartialView(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create2(int[] fieldid, Models.GapJournalCare collection)
        {
            try
            {
                if (fieldid.Count() == 0)
                {
                    throw new Exception("Phải chọn thửa đất");
                }

                var _dataobject2 = new DAL.GapField(Request);

                foreach (var item in fieldid)
                {
                    //_dataobject.MapView2Table();
                    var gapfield = _dataobject2.GetEdit(item);
                    if (gapfield.RefJournalId!=null)
                    {
                        collection.JournalId = gapfield.RefJournalId.Value;
                        int outputId = _dataobject.Insert(collection);
                    }

                }
                return RedirectToAction(this.ActionReturn());
            }
            catch (Exception ex)
            {
                ViewBag.fieldid = fieldid;
                Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(collection);
            }
        }

        public ActionResult Create(int? id, int? journalid)
        {
            var model = _dataobject.GetNew(id);
            if (journalid > 0)
            {
                model.JournalId = journalid ?? 0;
            }
            return PartialView(this._updateview, model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.GapJournalCare collection)
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
            return PartialView(this._updateview, _dataobject.GetEdit(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int JournalCareId, Models.GapJournalCare collection)
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