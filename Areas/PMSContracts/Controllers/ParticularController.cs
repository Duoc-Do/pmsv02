using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Areas.PMSContracts.Controllers
{
    public class ParticularController : Controller
    {
        //
        // GET: /PMSContracts/Particular/
        PMSDataContext db = new PMSDataContext();
        public ActionResult Index()
        {
            return View(db.CONTRACTS_CONDITIONS_PARTICULAR.ToList());
        }

        //
        // GET: /PMSContracts/Particular/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /PMSContracts/Particular/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /PMSContracts/Particular/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /PMSContracts/Particular/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /PMSContracts/Particular/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /PMSContracts/Particular/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /PMSContracts/Particular/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
