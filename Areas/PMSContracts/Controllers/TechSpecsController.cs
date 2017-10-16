using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.PMSContracts.Models;

namespace WebApp.Areas.PMSContracts.Controllers
{
    public class TechSpecsController : Controller
    {
        //
        // GET: /PMSContracts/TechSpecs/
        PMSDataContext objContext = new PMSDataContext();
        public ActionResult Index()
        {
            var tbTechspecs = objContext.CONTRACTS_CONDITIONS_TECHSPECS.ToList();
            return View(tbTechspecs);
        }

        //
        // GET: /PMSContracts/TechSpecs/Details/5

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tbTechspecs = objContext.CONTRACTS_CONDITIONS_TECHSPECS.Find(id);
            if (tbTechspecs == null)
            {
                return HttpNotFound();
            }
            return View(tbTechspecs);
        }

        //
        // GET: /PMSContracts/TechSpecs/Create

        public ActionResult Create()
        {
            return View(new TechSpecsModel());
        }

        //
        // POST: /PMSContracts/TechSpecs/Create

        [HttpPost]
        public ActionResult Create(TechSpecsModel model)
        {
            //try
            //{
            // TODO: Add insert logic here
            objContext.CONTRACTS_CONDITIONS_TECHSPECS.Add(model);
            objContext.SaveChanges();
            return RedirectToAction("Index");
            //}
            //catch
            //{
            //    return View(model);
            //}
        }

        //
        // GET: /PMSContracts/TechSpecs/Edit/5
        public ActionResult SearchListOnTab(string search, int id)
        {

            var tbTechspecs = objContext.CONTRACTS_CONDITIONS_TECHSPECS.Where(x => x.ContractID == id).OrderByDescending(x => x.TechSpecID).ToList();

            if (!String.IsNullOrEmpty(search))
            {
                tbTechspecs = tbTechspecs.Where(s => s.ContractID == id && (s.TechSpecCode.ToUpper().Contains(search.ToUpper()) || s.Description.ToUpper().Contains(search.ToUpper()))).ToList();
            }
            return View(tbTechspecs);

        }
        public ActionResult SearchListOnList(string search, int id)
        {

            var tbTechspecs = objContext.CONTRACTS_CONDITIONS_TECHSPECS.Where(x => x.ContractID == id).OrderByDescending(x => x.TechSpecID).ToList();

            if (!String.IsNullOrEmpty(search))
            {
                tbTechspecs = tbTechspecs.Where(s => s.ContractID == id && (s.TechSpecCode.ToUpper().Contains(search.ToUpper()) || s.Description.ToUpper().Contains(search.ToUpper()))).ToList();
            }
            return View(tbTechspecs);

        }
        public ActionResult Edit(int id)
        {
            var tbTechspecs = objContext.CONTRACTS_CONDITIONS_TECHSPECS.Where(x => x.TechSpecID == id).SingleOrDefault();

            return View(tbTechspecs);
        }

        //
        // POST: /PMSContracts/TechSpecs/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, TechSpecsModel model)
        {
            TechSpecsModel tbTechspecs = objContext.CONTRACTS_CONDITIONS_TECHSPECS.Where(x => x.TechSpecID == model.TechSpecID).SingleOrDefault();
            if (tbTechspecs != null)
            {
                objContext.Entry(tbTechspecs).CurrentValues.SetValues(model);
                objContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbTechspecs);
        }

        //
        // GET: /PMSContracts/TechSpecs/Delete/5

        public ActionResult Delete(int id)
        {
            TechSpecsModel tbTechspecs = objContext.CONTRACTS_CONDITIONS_TECHSPECS.Find(id);

            return View(tbTechspecs);
        }

        //
        // POST: /PMSContracts/TechSpecs/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, TechSpecsModel collection)
        {
            var tbTechspecs =
              objContext.CONTRACTS_CONDITIONS_TECHSPECS.Where(x => x.TechSpecID == id).SingleOrDefault();
            if (tbTechspecs != null)
            {
                objContext.CONTRACTS_CONDITIONS_TECHSPECS.Remove(tbTechspecs);
                objContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
