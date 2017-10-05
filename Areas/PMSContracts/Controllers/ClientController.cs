using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Areas.PMSContracts.Controllers
{
    public class ClientController : Controller
    {
        //
        // GET: /PMSContracts/Client/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /PMSContracts/Client/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /PMSContracts/Client/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /PMSContracts/Client/Create

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
        // GET: /PMSContracts/Client/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /PMSContracts/Client/Edit/5

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
        // GET: /PMSContracts/Client/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /PMSContracts/Client/Delete/5

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
