using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.PMSContracts.Models;

namespace WebApp.Areas.PMSContracts.Controllers
{
    public class ParticularController : Controller
    {
        //
        // GET: /PMSContracts/Particular/
        PMSDataContext objContext = new PMSDataContext();
        public ActionResult Index()
        {
            return View(objContext.CONTRACTS_CONDITIONS_PARTICULAR.ToList());
        }

        //
        // GET: /PMSContracts/Particular/Details/5

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tbParticular = objContext.CONTRACTS_CONDITIONS_PARTICULAR.Find(id);
            if (tbParticular == null)
            {
                return HttpNotFound();
            }
            return View(tbParticular);
        }
        [HttpGet]
        public ActionResult SearchListOnTab(string search, int id)
        {

            var tbParticular = objContext.CONTRACTS_CONDITIONS_PARTICULAR.Where(x => x.GeneralId == id).OrderByDescending(x => x.ParentId).ToList();

            if (!String.IsNullOrEmpty(search))
            {
                tbParticular = tbParticular.Where(s => s.ParentId == id && (s.ClauseCode.ToUpper().Contains(search.ToUpper()) || s.ClauseContent.ToUpper().Contains(search.ToUpper()))).ToList();
            }
            return View(tbParticular);

        }
        public ActionResult SearchListOnList(string search, int id)
        {

            var tbParticular = objContext.CONTRACTS_CONDITIONS_PARTICULAR.Where(x => x.GeneralId == id).OrderByDescending(x => x.ParentId).ToList();

            if (!String.IsNullOrEmpty(search))
            {
                tbParticular = tbParticular.Where(s => s.ParentId == id && (s.ClauseCode.ToUpper().Contains(search.ToUpper()) || s.ClauseContent.ToUpper().Contains(search.ToUpper()))).ToList();
            }
            return View(tbParticular);

        }
        //
        // GET: /PMSContracts/Particular/Create

        public ActionResult Create()
        {
            //ViewBag.ClauseContent = "Nhà thầu không được sử dụng và chiếm lĩnh toàn bộ đường đi, vỉa hè bất kể nó là công cộng hay thuộc quyền kiểm soát của Chủ đầu tư hoặc những người khác";
            return View(new ParticularModel());
        }

        //
        // POST: /PMSContracts/Particular/Create

        [HttpPost]
        public ActionResult Create(ParticularModel model)
        {
            //try
            //{
                // TODO: Add insert logic here
                objContext.CONTRACTS_CONDITIONS_PARTICULAR.Add(model);
                objContext.SaveChanges();
                return RedirectToAction("Index");
            //}
            //catch
            //{
            //    return View();
            //}
        }

        //
        // GET: /PMSContracts/Particular/Edit/5

        public ActionResult Edit(int id)
        {
            var tbParticular = objContext.CONTRACTS_CONDITIONS_PARTICULAR.Where(x => x.ParticularId == id).SingleOrDefault();

            return View(tbParticular);
        }

        //
        // POST: /PMSContracts/Particular/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, ParticularModel model)
        {
            //try
            //{
            ParticularModel tbParticular = objContext.CONTRACTS_CONDITIONS_PARTICULAR.Where(x => x.ParticularId == model.ParticularId).SingleOrDefault();
                if (tbParticular != null)
                {
                    objContext.Entry(tbParticular).CurrentValues.SetValues(model);
                    objContext.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(tbParticular);
            //}
            //catch
            //{
            //    return View();
            //}
        }

        //
        // GET: /PMSContracts/Particular/Delete/5

        public ActionResult Delete(int id)
        {
            ParticularModel tbParticular = objContext.CONTRACTS_CONDITIONS_PARTICULAR.Find(id);

            return View(tbParticular);
        }

        //
        // POST: /PMSContracts/Particular/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var tbParticular =
              objContext.CONTRACTS_CONDITIONS_PARTICULAR.Where(x => x.ParticularId == id).SingleOrDefault();
            if (tbParticular != null)
            {
                objContext.CONTRACTS_CONDITIONS_PARTICULAR.Remove(tbParticular);
                objContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
