using System;
using System.Linq;
using System.Web.Mvc;



namespace WebApp.Areas.Accounting.Controllers
{
    [Authorize(Roles = "Admin,Accounting")]
    public class AppAccountController : AccountingController
    {
        //
        // GET: /Account/Login
        string pnameusername = "accounting.login.serviceid_{0}.username";
        string pnamepassword = "accounting.login.serviceid_{0}.password";
        public ActionResult Login(string returnUrl, int? serviceid, int? isdefault)
        {

            ViewBag.ReturnUrl = returnUrl;
            var loginmodel = new Models.AppLoginModel();

            loginmodel.ServiceId = serviceid ?? 0;
            loginmodel.UserName = WebApp.Services.GlobalUserContext.GetProfile(string.Format(pnameusername, loginmodel.ServiceId));
            loginmodel.Password = WebApp.Services.GlobalUserContext.GetProfile(string.Format(pnamepassword, loginmodel.ServiceId));

            if (isdefault == 1)
            {
                try
                {

                    if (String.IsNullOrEmpty(loginmodel.UserName) || String.IsNullOrEmpty(loginmodel.Password) || loginmodel.ServiceId == 0) throw new InvalidOperationException("Phải nhập tên đăng nhập và mật khẩu.");

                    WebApp.Models.SenContext sendb = new WebApp.Models.SenContext();
                    var company = sendb.SenServices.Where(m => m.ServiceId == loginmodel.ServiceId && m.UserProfile.UserName == User.Identity.Name).FirstOrDefault();

                    if (company.SenCompany.IsApproved != true) throw new InvalidOperationException("Doanh nghiệp đang chờ duyệt./n Không thể truy cập được.");

                    string cnn = company.SenCompany.ConnectionString;
                    Models.WebAppAccEntities db = new Models.WebAppAccEntities(cnn);

                    string CompanyName = db.SysCompanies.Where(m => m.EnumValue == "CompanyName").FirstOrDefault().EnumName;


                    WebApp.Areas.Accounting.Services.GlobalVariant.SetConnection(cnn);
                    WebApp.Areas.Accounting.Services.GlobalVariant.SetCompanyName(CompanyName);
                    WebApp.Areas.Accounting.Services.GlobalVariant.SetCompanyId(company.CompanyId.ToString());

                    if (!WebApp.Areas.Accounting.Services.User.Login(loginmodel.UserName, loginmodel.Password)) throw new InvalidOperationException("Tên đăng nhập và mật khẩu không hợp lệ.");

                    WebApp.Services.GlobalUserContext.AddProfile(string.Format(pnameusername, loginmodel.ServiceId), loginmodel.UserName);
                    WebApp.Services.GlobalUserContext.AddProfile(string.Format(pnamepassword, loginmodel.ServiceId), loginmodel.Password);


                    company.SenCompany.LastLoginDate = DateTime.Now;

                    sendb.Entry(company.SenCompany).State = System.Data.Entity.EntityState.Modified;
                    sendb.SaveChanges();



                    return RedirectToLocal(returnUrl);
                }
                catch (Exception)
                {
                }
            }

            return View(loginmodel);
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Models.AppLoginModel model, string returnUrl)
        {
            try
            {
                if (String.IsNullOrEmpty(model.UserName) || String.IsNullOrEmpty(model.Password) || model.ServiceId == 0) throw new InvalidOperationException("Phải nhập tên đăng nhập và mật khẩu.");

                WebApp.Models.SenContext sendb = new WebApp.Models.SenContext();
                var company = sendb.SenServices.Where(m => m.ServiceId == model.ServiceId && m.UserProfile.UserName == User.Identity.Name).FirstOrDefault();

                if (company.SenCompany.IsApproved != true) throw new InvalidOperationException("Doanh nghiệp đang chờ duyệt./n Không thể truy cập được.");

                string cnn = company.SenCompany.ConnectionString;
                Models.WebAppAccEntities db = new Models.WebAppAccEntities(cnn);

                string CompanyName = db.SysCompanies.Where(m => m.EnumValue == "CompanyName").FirstOrDefault().EnumName;

                Services.GlobalVariant.SetConnection(cnn);
                Services.GlobalVariant.SetCompanyName(CompanyName);

                Services.GlobalVariant.SetCompanyId(company.CompanyId.ToString());

                if (!Services.User.Login(model.UserName, model.Password)) throw new InvalidOperationException("Tên đăng nhập và mật khẩu không hợp lệ.");

                WebApp.Services.GlobalUserContext.AddProfile(string.Format(pnameusername, model.ServiceId), model.UserName);
                WebApp.Services.GlobalUserContext.AddProfile(string.Format(pnamepassword, model.ServiceId), model.Password);

                
                company.SenCompany.LastLoginDate = DateTime.Now;

                sendb.Entry(company.SenCompany).State = System.Data.Entity.EntityState.Modified;
                sendb.SaveChanges();

                return RedirectToLocal(returnUrl);
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, ex);
                return View(model);
            }
        }


        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {

                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "AppHome", new { area = "Accounting" });
            }
        }
    }






    //public class ActionAuthorizeAttribute : ActionFilterAttribute
    //{
    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        bool userNotRight = true;
    //        var controllerName = filterContext.RouteData.Values["controller"];
    //        var actionName = filterContext.RouteData.Values["action"];
    //        SenViet.Uti.Security sec = new Security();

    //        string[] userRoles = System.Web.Security.Roles.GetRolesForUser();

    //        foreach (string userRole in userRoles)
    //        {

    //            if (userRole.ToLower() == "sysadmin")
    //            {
    //                userNotRight = false;
    //                break;
    //            }

    //            if (sec.CheckRight(userRole, controllerName.ToString(), actionName.ToString()))
    //            {
    //                userNotRight = false;
    //                break;
    //            }

    //        }



    //        if (filterContext.HttpContext.User.Identity == null || !filterContext.HttpContext.User.Identity.IsAuthenticated)
    //        {
    //            //System.Web.Security.FormsAuthentication.RedirectToLoginPage();
    //            filterContext.Result = new RedirectResult(System.Web.Security.FormsAuthentication.LoginUrl + "?returnUrl=" +
    //            filterContext.HttpContext.Server.UrlEncode(filterContext.HttpContext.Request.RawUrl));
    //            return;
    //        }

    //        //Check user right here
    //        if (userNotRight)
    //        {
    //            filterContext.HttpContext.Response.StatusCode = 302;
    //            filterContext.Result = new HttpUnauthorizedResult();
    //            return;
    //        }

    //        //#region Test
    //        //string AppAccConnection = "AppAccConnectionKey";
    //        //if (filterContext.HttpContext.Session[AppAccConnection] == null)
    //        //{
    //        //    filterContext.Result = new RedirectResult("/acc/accaccount/logon");

    //        //    //filterContext.HttpContext.Session[AppAccConnection] = "metadata=res://*/Areas.ACC.Models.AccServiceModel.csdl|res://*/Areas.ACC.Models.AccServiceModel.ssdl|res://*/Areas.ACC.Models.AccServiceModel.msl;provider=System.Data.SqlClient;provider connection string=\"data source=.;initial catalog=lotusviet_web_wm;user id=sa;password=pheonix;multipleactiveresultsets=True;App=EntityFramework\"";
    //        //    return;
    //        //}

    //        //#endregion

    //    }
    //}

}
