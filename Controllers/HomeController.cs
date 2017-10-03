using System.Web.Mvc;

namespace WebApp.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {

        public ActionResult SenChat()
        {
            return View();
        }


      

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return Redirect("http://localhost:60587/pmscontracts/contract");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
