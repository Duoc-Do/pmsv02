using System.Linq;
using System.Web.Mvc;
using WebApp.Areas.Accounting.Services;

namespace WebApp.Areas.Accounting.Controllers
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
            Models.WebAppAccEntities db = new Models.WebAppAccEntities(Services.GlobalVariant.GetConnection());
            //int maxRows = 1; 
            object rows = null;
            object results = null;

            switch (fieldname)
            {
                case "WarehouseLineCode":
                case "WarehouseCode":
                    var warehousecode = db.Database.SqlQuery<Models.AppWarehouseTable>("Select * from AppWarehouseTable Where WarehouseCode={0}", keyword).SingleOrDefault();
                    results = (new { rows = warehousecode });
                    return Json(results, JsonRequestBehavior.AllowGet);

                case "IsoCode":
                    var isocode = db.Database.SqlQuery<Models.AppCurrencyTable>("Select * from AppCurrencyTable Where IsoCode={0}", keyword).SingleOrDefault();
                    results = (new { rows = isocode });
                    return Json(results, JsonRequestBehavior.AllowGet);

                case "DisplayNumberLineCredit":
                case "DisplayNumberLineDebit":
                case "DisplayNumberCredit":
                case "DisplayNumberDebit":
                case "DisplayNumber":
                    var displaynumber = db.Database.SqlQuery<Models.AppAccountView2>("Select * from AppAccountView2 Where DisplayNumber={0}", keyword).SingleOrDefault();
                    results = (new { rows = displaynumber });
                    return Json(results, JsonRequestBehavior.AllowGet);
                case "ParentDisplayNumber":
                    var parentdisplaynumber = db.Database.SqlQuery<Models.AppAccountView>("Select * from AppAccountView Where DisplayNumber={0}", keyword).SingleOrDefault();
                    results = (new { rows = parentdisplaynumber });
                    return Json(results, JsonRequestBehavior.AllowGet);

                case "CustomerCode":
                    var customercode = db.Database.SqlQuery<Models.AppCustomerView>("Select * from AppCustomerView Where CustomerCode={0}", keyword).SingleOrDefault();
                    results = (new { rows = customercode });
                    return Json(results, JsonRequestBehavior.AllowGet);

                case "CustomerGroupCode":
                    var customergroupcode = db.Database.SqlQuery<Models.AppCustomerGroupView>("Select * from AppCustomerGroupView Where CustomerGroupCode={0}", keyword).SingleOrDefault();
                    results = (new { rows = customergroupcode });
                    return Json(results, JsonRequestBehavior.AllowGet);

                case "ItemGroupCode":
                    var itemgroupcode = db.Database.SqlQuery<Models.AppItemGroupTable>("Select * from AppItemGroupTable Where ItemGroupCode={0}", keyword).SingleOrDefault();
                    results = (new { rows = itemgroupcode });
                    return Json(results, JsonRequestBehavior.AllowGet);

                case "ItemCode":
                    var itemcode = db.Database.SqlQuery<Models.AppItemView>("Select * from AppItemView Where ItemCode={0}", keyword).SingleOrDefault();
                    results = (new { rows = itemcode });
                    return Json(results, JsonRequestBehavior.AllowGet);

                case "UOMLinkCode":
                case "UOMCode":
                    var uomcode = db.Database.SqlQuery<Models.AppUnitOfMeasureTable>("Select * from AppUnitOfMeasureTable Where UOMCode={0}", keyword).SingleOrDefault();
                    results = (new { rows = uomcode });
                    return Json(results, JsonRequestBehavior.AllowGet);

                case "ItemTypeName":
                    var itemtypename = db.Database.SqlQuery<Models.AppItemTypeTable>("Select * from AppItemTypeTable Where EnumName={0}", keyword).SingleOrDefault();
                    results = (new { rows = itemtypename });
                    return Json(results, JsonRequestBehavior.AllowGet);

                case "ItemMethodTypeName":
                    var itemmethodtypename = db.Database.SqlQuery<Models.AppItemMethodTypeTable>("Select * from AppItemMethodTypeTable Where EnumName={0}", keyword).SingleOrDefault();
                    results = (new { rows = itemmethodtypename });
                    return Json(results, JsonRequestBehavior.AllowGet);

                default:
                    break;
            }
            return Json(rows, JsonRequestBehavior.AllowGet);
        }

    }



}


