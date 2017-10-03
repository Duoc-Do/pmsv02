using System;
using System.Linq;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace WebApp.Areas.Accounting.Controllers
{
    public class GapJournalHarvestController : AppAccountingListController
    {
        DAL.GapJournalHarvest _dataobject;

        protected override IActionInvoker CreateActionInvoker()
        {
            _dataobject = new DAL.GapJournalHarvest(Request);
            this.InitData(_dataobject);
            return base.CreateActionInvoker();
        }

        public ActionResult AutoComplete()
        {
            DAL.GapJournalHarvest _dataobject = new DAL.GapJournalHarvest(Request);
            return Json(_dataobject.AutoComplete(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FieldChange()
        {
            DAL.GapJournalHarvest _dataobject = new DAL.GapJournalHarvest(Request);
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
        public ActionResult Create2(int[] fieldid, Models.GapJournalHarvest collection)
        {
            try
            {
                if (fieldid.Count() == 0)
                {
                    throw new Exception("Phải chọn thửa đất");
                }

                var _dataobject2 = new DAL.GapField(Request);
                var _dataobject3 = new DAL.GapJournal(Request);


                foreach (var item in fieldid)
                {
                    //_dataobject.MapView2Table();
                    var gapfield = _dataobject2.GetEdit(item);
                    if (gapfield.RefJournalId != null)
                    {
                        var gapjournal = _dataobject3.GetById2(gapfield.RefJournalId.Value);

                        if (gapjournal.IsolationDateEnd()>DateTime.Today)
                        {
                            continue;
                        }

                        collection.JournalId = gapjournal.JournalId;
                        collection.RefIsolationDate = gapjournal.IsolationDate;
                        collection.RefIsolationDay = gapjournal.IsolationDay;

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
        public ActionResult Create(Models.GapJournalHarvest collection)
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
        public ActionResult Edit(int JournalHarvestId, Models.GapJournalHarvest collection)
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