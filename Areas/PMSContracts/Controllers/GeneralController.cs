using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.PMSContracts.Models;
using excel = Microsoft.Office.Interop.Excel;

namespace WebApp.Areas.PMSContracts.Controllers
{
    public class GeneralController : Controller
    {
        //
        // GET: /PMSContracts/General/
        PMSDataContext objContext = new PMSDataContext();
        public ActionResult Index()
        {
            var tbGeneral = objContext.CONTRACTS_CONDITIONS_GENERAL.OrderByDescending(s => s.Id).ToList();
            return View(tbGeneral);
        }
        [HttpGet]
        public ActionResult Index(string search)
        {
            var tbGeneral = from s in objContext.CONTRACTS_CONDITIONS_GENERAL select s;

            if (!String.IsNullOrEmpty(search))
            {
                tbGeneral = tbGeneral.Where(s => s.ClauseCode.ToUpper().Contains(search.ToUpper())
                                       || s.ClauseContent.ToUpper().Contains(search.ToUpper()));
            }
            return View(tbGeneral.ToList());
        }
        public ActionResult ListOnTab(int id)
        {
            var tbGeneral = objContext.CONTRACTS_CONDITIONS_GENERAL.OrderByDescending(s => s.ContractID).Where(s => s.ContractID == id).ToList();
            return View(tbGeneral);
        }
        [HttpGet]
        public ActionResult ListOnTab(string search)
        {
            var tbGeneral = from s in objContext.CONTRACTS_CONDITIONS_GENERAL select s;

            if (!String.IsNullOrEmpty(search))
            {
                tbGeneral = tbGeneral.Where(s => s.ClauseCode.ToUpper().Contains(search.ToUpper())
                                       || s.ClauseContent.ToUpper().Contains(search.ToUpper()));
            }
            return View(tbGeneral.ToList());
        }
        [HttpGet]
        public ActionResult SearchListOnTab(string search, long idcontract)
        {
            var tbGeneral = from s in objContext.CONTRACTS_CONDITIONS_GENERAL where s.ContractID==idcontract select s;

            if (!string.IsNullOrEmpty(search))
            {
                tbGeneral = tbGeneral.Where(s => s.ClauseCode.ToUpper().Contains(search.ToUpper())
                                       || s.ClauseContent.ToUpper().Contains(search.ToUpper());
            }
            return View(tbGeneral.ToList());
        }
        public ActionResult Import(HttpPostedFileBase excelfile)
        {
            if (excelfile==null||excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Please select a excel file";
                return View("Index");
            }
            else
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    //string path = Server.MapPath("~/Areas/PMSContracts/" + excelfile.FileName);
                    string fileName = Path.GetFileName(excelfile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content"), fileName);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    excelfile.SaveAs(path);
                    // Read dataz from excel file
                    excel.Application application = new excel.Application();
                    excel.Workbook workbook = application.Workbooks.Open(path);
                    excel.Worksheet worksheet = workbook.ActiveSheet;
                    excel.Range range = worksheet.UsedRange;
                    for(int row=3; row <= range.Rows.Count; row++)
                    {

                    }
                    return View("Success");
                }
                else
                {
                    ViewBag.Error = "File type is incorrect<br>";
                    return View();
                }
            }
        }
        public ActionResult Create() // For view layer
        {
            return View(new GeneralModel());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tbGeneral = objContext.CONTRACTS_CONDITIONS_GENERAL.Find(id);
            if (tbGeneral == null)
            {
                return HttpNotFound();
            }
            return View(tbGeneral);
        }
        //
        // GET: /PMSContracts/General/Create

        //public ActionResult Create()
        //{
        //    return View();
        //}

        //
        // POST: /PMSContracts/General/Create

        [HttpPost]
        public ActionResult Create(GeneralModel model)
        {
            //try
            //{
                // TODO: Add insert logic here
                objContext.CONTRACTS_CONDITIONS_GENERAL.Add(model);
                objContext.SaveChanges();
                return RedirectToAction("Index");
            //}
            //catch
            //{
            //    return View(model);
            //}
        }

        //
        // GET: /PMSContracts/General/Edit/5

        public ActionResult Edit(int id)
        {
            var tbGeneral = objContext.CONTRACTS_CONDITIONS_GENERAL.Where(x => x.Id == id).SingleOrDefault();
    
            return View(tbGeneral);
        }

        //
        // POST: /PMSContracts/General/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, GeneralModel model)
        {
            GeneralModel tbGeneral = objContext.CONTRACTS_CONDITIONS_GENERAL.Where(x => x.Id == model.Id).SingleOrDefault();
            if (tbGeneral != null)
            {
                objContext.Entry(tbGeneral).CurrentValues.SetValues(model);
                objContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbGeneral);
        }

        //
        // GET: /PMSContracts/General/Delete/5

        public ActionResult Delete(int id)
        {
            GeneralModel tbGeneral = objContext.CONTRACTS_CONDITIONS_GENERAL.Find(id);

            return View(tbGeneral);
        }

        //
        // POST: /PMSContracts/General/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, GeneralModel collection)
        {
            var tbGeneral =
              objContext.CONTRACTS_CONDITIONS_GENERAL.Where(x => x.Id == id).SingleOrDefault();
            if (tbGeneral != null)
            {
                objContext.CONTRACTS_CONDITIONS_GENERAL.Remove(tbGeneral);
                objContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }
}
