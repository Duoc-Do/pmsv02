
using System;
using System.Linq;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace WebApp.Areas.Accounting.Controllers
{
    public class GapJournalController : AppAccountingListController
    {
        DAL.GapJournal _dataobject;

        protected override IActionInvoker CreateActionInvoker()
        {
            _dataobject = new DAL.GapJournal(Request);
            this.InitData(_dataobject);
            return base.CreateActionInvoker();
        }

        public ActionResult AutoComplete()
        {
            DAL.GapJournal _dataobject = new DAL.GapJournal(Request);
            return Json(_dataobject.AutoComplete(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTree(int treeid)
        {

            DAL.GapTree _dataobject = new DAL.GapTree(Request);
            //return PartialView(_dataobject.GetById(treeid));
            return Json(_dataobject.GetById(treeid), JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Complete(int[] fieldid)
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
                    //collection.FieldId = item;
                    //int outputId = _dataobject.Insert(collection);

                    var gapfield = _dataobject2.GetEdit(item);
                    var gapjournal = _dataobject.GetById(gapfield.RefJournalId??0);
                    if (gapjournal!=null)
                    {
                        //if (gapjournal.GapJournalHarvests.Count==0)
                        //{

                        //}

                        gapjournal.StatusId = 0;
                        _dataobject.Update(gapjournal);
                    }
                    gapfield.RefJournalId = null;
                    _dataobject2.Update(gapfield);
                }

            }
            catch (Exception ex)
            {
                return Json(new { ketqua = ex.Message });
            }

            return Json(new { ketqua = "Đã xong!" });
        }

        public ActionResult FieldChange()
        {
            DAL.GapJournal _dataobject = new DAL.GapJournal(Request);
            return Json(_dataobject.FieldChange(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return PartialView(this._indexview, _dataobject.GetData());
        }

        public ActionResult JournalView(int id)
        {
            return PartialView(_dataobject.GetEdit(id));
        }

        public ActionResult JournalQRCode(int id)
        {
            return PartialView(_dataobject.GetEdit(id));
        }

        public ActionResult Create2(int? id, int[] fieldid)
        {
            var model = _dataobject.GetNew(id);
            if (fieldid.Count() > 0)
            {
                model.FieldId = fieldid[0];
            }
            ViewBag.fieldid = fieldid;
            return PartialView(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create2(int[] fieldid,Models.GapJournal collection)
        {
            try
            {
                if (fieldid.Count()==0)
                {
                    throw new Exception("Phải chọn thửa đất");
                }

                var _dataobject2 = new DAL.GapField(Request);

                foreach (var item in fieldid)
                {
                    //_dataobject.MapView2Table();
                    var gapfield = _dataobject2.GetEdit(item);
                    if (gapfield.RefJournalId>0)
                    {
                        continue;
                    }
                    collection.FieldId = item;
                    int outputId = _dataobject.Insert(collection);


                    gapfield.RefJournalId = outputId;
                    _dataobject2.Update(gapfield);
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

        public ActionResult Create(int? id, int? fieldid)
        {
            var model = _dataobject.GetNew(id);
            if (fieldid > 0)
            {
                model.FieldId = fieldid ?? 0;
            }
            return PartialView(this._updateview, model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.GapJournal collection)
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
        public ActionResult Edit(int JournalId, Models.GapJournal collection)
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