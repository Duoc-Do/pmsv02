using System.Web.Mvc;

namespace WebApp.Areas.Accounting.Controllers
{
    [Authorize]
    public class GapHomeController : AppAccountingController
    {
        // nhật ký canh tác
        // GET: /Accounting/Home/
        public ActionResult Index()
        {
            return PartialView();
        }

        //
        // GET: /Accounting/Home/ cây trồng
        public ActionResult Index2()
        {
            return PartialView();
        }

        //
        // GET: /Accounting/Home/ vật tư
        public ActionResult Index3()
        {
            return PartialView();
        }

        //
        // GET: /Accounting/Home/
        public ActionResult GetField(int farmid)
        {
            var _dataobject = new DAL.GapFarm(Request);
            
            return PartialView(_dataobject.GetById(farmid));
        }

        //
        // GET: /Accounting/Home/
        public ActionResult GetRow(int fieldid)
        {
            var _dataobject = new DAL.GapField(Request);

            return PartialView(_dataobject.GetById(fieldid));
        }


        //
        // GET: /Accounting/Home/
        public ActionResult GetSeed(int treeid)
        {
            var _dataobject = new DAL.GapTree(Request);

            return PartialView(_dataobject.GetById(treeid));
        }


        //
        // GET: /Accounting/Home/
        public ActionResult GetItem(int itemgroupid)
        {
            var _dataobject = new DAL.AppItemGroupTable(Request);

            return PartialView(_dataobject.GetById2(itemgroupid));
        }

    }
}
