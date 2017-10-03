using System.Web.Mvc;

namespace WebApp.Areas.Accounting.Controllers
{
    [Authorize]
    public class ResHomeController : AppAccountingController
    {
        //
        // GET: /Accounting/Home/
        public ActionResult Index()
        {
            return PartialView();
        }

        //
        // GET: /Accounting/Home/
        public ActionResult Kitchen()
        {
            return PartialView();
        }

    }
}
