using System.Web.Mvc;

namespace WebApp.Areas.Accounting.Controllers
{
    [Authorize]
    public class ResReportController : AppAccountingController
    {
        //
        // GET: /Accounting/Home/
        public ActionResult Index()
        {
            return PartialView();
        }


    }
}
