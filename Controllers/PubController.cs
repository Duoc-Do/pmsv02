using System.Linq;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class PubController : Controller
    {

        public ActionResult ChangeLang(int lang)
        {
            WebApp.Services.GlobalVariant.LanguageId = lang;
            return RedirectToRoute("HomePage");
        }

        public ActionResult Gap(int id,bool isjson=false)
        {
            WebApp.Models.SenContext sendb = new WebApp.Models.SenContext();
            var sencompany = sendb.SenCompanys.Where(m => m.CompanyId == id).FirstOrDefault();

            if (sencompany == null) return Content("");

            string cnn = sencompany.ConnectionString;



            Areas.Accounting.Models.WebAppAccEntities db = new Areas.Accounting.Models.WebAppAccEntities(cnn);

            if (Request.Params["journalid"] != null)
            {
                int journalid = int.Parse(Request.Params["journalid"]);
                var model = db.GapJournals.SingleOrDefault(m => m.JournalId == journalid);
                //return Content(cnn);

                //return Json(new { gapjournalcares = model.GapJournalCares, gapjournalharvests = model.GapJournalHarvests }, JsonRequestBehavior.AllowGet);

                //var modelJson = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                if (isjson)
                {
                    return Json(new { model = model, company = sencompany }, JsonRequestBehavior.AllowGet);
                }

                ViewBag.sencompany = sencompany;
                return View(model);
            }



            return Content("");
        }



    }
}
