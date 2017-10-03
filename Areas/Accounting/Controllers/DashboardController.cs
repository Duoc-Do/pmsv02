using System.Web.Mvc;


namespace WebApp.Areas.Accounting.Controllers
{
    [Authorize]
    public class DashboardController : AccountingController
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Khu vực kế toán online.";

            return View();
        }

    }
}
