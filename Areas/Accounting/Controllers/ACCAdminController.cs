using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SenViet.Areas.ACC.Controllers
{
    [SenViet.Uti.ActionAuthorizeAttribute]
    public class ACCAdminController : Controller
    {
        //
        // GET: /ACC/ACCAdmin/

        public ActionResult Index()
        {
            return View();
        }

    }
}
