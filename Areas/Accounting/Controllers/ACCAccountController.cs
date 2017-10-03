using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SenViet.Areas.ACC.Controllers
{
    public class ACCAccountController : Controller
    {
        //
        // GET: /ACC/Account/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(string returnUrl)
        {
            string AppAccConnection = "AppAccConnectionKey";
            this.HttpContext.Session[AppAccConnection] = "Đã đăng nhập ứng dụng";

            return RedirectToAction("Index", "ACCAdmin");
        }



        //[HttpPost]
        //public ActionResult LogOn(LogOnModel model, string returnUrl)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (Membership.ValidateUser(model.UserName, model.Password))
        //        {
        //            FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);

        //            //khi xác thực thực hiện trộn cart id cho user id này
        //            eExtensions.MergeCart2User(this.HttpContext, model.UserName);

        //            //sẽ gọi ngữ cảnh 
        //            eExtensions.SetWorkContext(this.HttpContext);


        //            if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
        //                && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
        //            {
        //                return Redirect(returnUrl);
        //            }
        //            else
        //            {
        //                return RedirectToAction("Index", "Home");
        //            }
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Tên đăng nhập và mật khẩu không chính xác.");
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    if (Request.IsAjaxRequest())
        //    {
        //        return PartialView("LogOn", model);
        //    }
        //    else
        //    {
        //        return View(model);

        //    }
        //}

    }
}
