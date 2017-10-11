using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebApp.Areas.PMSContracts.Models;

namespace WebApp.Areas.PMSContracts.Controllers
{
    public class ContractController : Controller
    {
        PMSDataContext objContext = new PMSDataContext();

        public ActionResult Index()
        {
            var tbContract = objContext.CONTRACTS.OrderByDescending(s => s.ContractID).Take(20).ToList();
            return View(tbContract);
        }
        [HttpGet]
        public ActionResult Index(string search) // ten bien phai giong voi name="search" cua button
        {
            //var tbContract = from s in objContext.CONTRACTS select s;
            var tbContract = objContext.CONTRACTS.OrderByDescending(x => x.ContractID);

            if (!String.IsNullOrEmpty(search))
            {
                tbContract = tbContract.Where(s => s.Description_VN.ToUpper().Contains(search.ToUpper())
                                       || s.ContractCode.ToUpper().Contains(search.ToUpper()) || s.ContractIDERP.ToUpper().Contains(search.ToUpper()) || s.proj_status.ToUpper().Contains(search.ToUpper())).OrderByDescending(x=>x.ContractID);
            }
            return View(tbContract.Take(20).ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractModel tbContract = objContext.CONTRACTS.Find(id);
            ContractModel ct = new ContractModel();
            var client = from b in objContext.CONTRACTS_CLIENTS
                         join a in objContext.CONTRACTS on b.ClientID equals a.ClientID
                         where a.ContractID == id
                         select new { b.ClientID, b.ResName, b.Territory, b.AccountNumber, b.Bank, b.CompanyAddress, b.DeskPhone, b.Email, b.Type, b.CompanyName, b.Fax, b.HandPhone, b.Status, b.ModifiedDate };
            if (tbContract == null)
            {
                return HttpNotFound();
            }
            return View(tbContract);
        }
        public ActionResult Create() // For view layer
        {
            return View(new ContractModel());
        }
        [HttpPost]
        public ActionResult Create(ContractModel model) // For action create
        {
            objContext.CONTRACTS.Add(model);
            objContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id) // For view layer
        {
            ContractModel tbContract = objContext.CONTRACTS.Find(id);

            return View(tbContract);
        }
        [HttpPost]
        public ActionResult Delete(int id, ContractModel model) // For action detete
        {
            var tbContract =
              objContext.CONTRACTS.Where(x => x.ContractID == id).SingleOrDefault();
            if (tbContract != null)
            {
                objContext.CONTRACTS.Remove(tbContract);
                objContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id) // For view layer
        {
            var tbContract = objContext.CONTRACTS.Where(x => x.ContractID == id).SingleOrDefault();
            var tbcontract1 = objContext.CONTRACTS_CLIENTS.ToList();
            SelectList list_contract = new SelectList(tbcontract1, "ClientID", "CompanyName"); 
            ViewBag.lContract = tbContract;
            return View(tbContract);
        }
        [HttpPost]
        public ActionResult Edit(ContractModel model) // For action edit
        {
            {
                ContractModel tbContract = objContext.CONTRACTS.Where(x => x.ContractID == model.ContractID).SingleOrDefault();
                if (tbContract != null)
                {
                    objContext.Entry(tbContract).CurrentValues.SetValues(model);
                    objContext.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(tbContract);
            }
        }
        [HttpGet]
        public ActionResult Contract(int page = 1, int pageSize = 10, int isjson = 0)
        {
            var skipRecords = page * pageSize;
            var loaddb = objContext.CONTRACTS.OrderBy(x => x.ContractID).OrderByDescending(x=>x.ContractID).Skip(skipRecords).Take(pageSize).ToList();
            if (isjson == 1)
            {
                return Json(new { rows = loaddb, status = 1, message = "completed" }, JsonRequestBehavior.AllowGet);
            }
            if (isjson == 2)
            {
                return PartialView("_ContractPartial", loaddb);
            }

            return View("Index", loaddb);

        }
        [HttpGet]
        public List<ClientModel> GetClientList(long id)
        {
            var clients = from d in objContext.CONTRACTS
                                                 join b in objContext.CONTRACTS_CLIENTS on d.ClientID equals b.ClientID
                                                 where d.ContractID == id
                                                 select new { b.ClientID,b.ResName};
            List<ClientModel> bn = new List<ClientModel>();
            ClientModel f = new ClientModel();
            foreach(var t in clients)
            {
                f.ClientID = t.ClientID;
                f.ResName = t.ResName;
                bn.Add(f);
            }                                 
            return bn;
        }

        public ActionResult GetSearchValue(string search)
        {
            List<ClientModel> allsearch = objContext.CONTRACTS_CLIENTS.Where(x => x.CompanyName.Contains(search)).Select(x => new ClientModel
            {
                ClientID = x.ClientID,
                CompanyName = x.CompanyName
            }).ToList();
            //return new JsonResult { Data = allsearch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return Json(allsearch, JsonRequestBehavior.AllowGet);
        }

        //const int recordsPerPage = 8;
        //public ActionResult Contract(int? id)
        //{
        //    var page = id ?? 0;
        //    if (Request.IsAjaxRequest())
        //    {
        //        return PartialView("_ContractPartial", GetPaginatedProducts(page));
        //    }
        //    return View("Index","Contract", objContext.CONTRACTS.Where(x => x.ContractCode != null).Take(recordsPerPage));
        //}
        //private List<ContractModel> GetPaginatedProducts(int page = 1)
        //{
        //    var skipRecords = page * recordsPerPage;

        //    return objContext.CONTRACTS.Where(x => x.ContractCode != null)
        //        .OrderBy(x => x.ContractID)
        //        .Skip(skipRecords)
        //        .Take(recordsPerPage).ToList();
        //}
        [HttpGet]
        public JsonResult AutoCompleteCity(string Prefix)
        { 
            var CityName = (from N in objContext.CONTRACTS_CLIENTS
                            where N.CompanyName.StartsWith(Prefix)
                            select new { N.CompanyName });
            return Json(CityName, JsonRequestBehavior.AllowGet);
        }
    }
}