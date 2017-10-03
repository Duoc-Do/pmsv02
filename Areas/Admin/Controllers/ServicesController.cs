using System.Web.Mvc;
using WebApp.Areas.Admin.Services;

namespace WebApp.Areas.Admin.Controllers
{
    public class ServicesController : Controller
    {
        //
        // GET: /Service/UI/
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult AutoComplete(string fieldname, string keyword, string tablename = "")
        {
            object results = null;
            results = Data.GetByKeyword(this.Request);
            return Json(results, JsonRequestBehavior.AllowGet);
        }


        //
        // GET: /Service/UI/
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetRowByCode(string fieldname, string keyword, string tablename = "")
        {
            object results = null;
            results = Data.GetByCode(this.Request);
            return Json(results, JsonRequestBehavior.AllowGet);

            //Models.AdminContext db = new Models.AdminContext();
            ////int maxRows = 1; 
            //object rows = null;
            //object results = null;

            //switch (fieldname)
            //{
            //    case "WarehouseLineCode":
            //    case "WarehouseCode":
            //        var warehousecode = db.Database.SqlQuery<Models.AppWarehouseTable>("Select * from AppWarehouseTable Where WarehouseCode={0}", keyword).SingleOrDefault();
            //        results = (new { rows = warehousecode });
            //        return Json(results, JsonRequestBehavior.AllowGet);

            //        default:
            //        break;
            //}
            //return Json(rows, JsonRequestBehavior.AllowGet);
        }

    }



}


