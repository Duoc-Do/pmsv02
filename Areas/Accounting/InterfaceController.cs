using System;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Accounting.DAL;

namespace WebApp.Areas.Accounting
{
    [Authorize(Roles = "Admin,Accounting")]
    public abstract class AccountingController : Controller
    {
    }

    [Authorize(Roles = "Admin,Accounting")]
    [AppAuthorize]
    [AppFilter]
    public abstract class AppAccountingController : Controller
    {

        public ActionResult NoAction()
        {
            return Content("No action!");
        }

        public string ActionReturn()
        {
            if (Request.Form["action_return"] != null)
            {
                return Request.Form["action_return"].ToString();
            }
            return Request.Params["action_return"].ToString();
        }
    }

    public class AppAccountingFunctionController : AppAccountingController
    {
        public string _indexview = "_IndexFunction";
        public string _roleview = "_NoRole";
        public AppAccountingFunctionController() { }
        public void InitData(SenVietGeneralObject _dataobject)
        {
            ViewBag.ajaxoption = _dataobject.appajaxoption.ajaxoption;
            ViewBag.Title = _dataobject.sysbusiness.Des;
        }

    }


    public class AppAccountingReportController : AppAccountingController
    {
        public string _indexview = "_IndexReport";
        public string _roleview = "_NoRole";
        public SenVietReportObject __dataobject;

        public AppAccountingReportController() { }
        public void InitData(SenVietReportObject _dataobject)
        {
            ViewBag.ajaxoption = _dataobject.appajaxoption.ajaxoption;
            ViewBag.Title = _dataobject.sysbusiness.Des;
            this.__dataobject = _dataobject;
        }

        public virtual ActionResult ThrillDown()
        {

            var vouchercode = Request["VoucherCode"].ToString();
            var documentid = long.Parse(Request["DocumentID"].ToString());

            string actionname = "Edit";
            string controllername = Services.Voucher.GetControllerName(vouchercode);
            string url = Url.Action(actionname, controllername, new { id = documentid });

            return Redirect(url);
        }

        public virtual ActionResult ExportToExcel()
        {
            string FileName = string.Format("{0}{1}.xlsx", this.__dataobject._metaname, DateTime.Now.ToString("yyyyMMddhhmmss"));
            //byte[] bytes = this.__dataobject.ExportToExcel();
            //if (bytes == null) return new EmptyResult();

            return File(this.__dataobject.ExportToExcel(), "text/xls", FileName);
        }

    }

    public class AppAccountingListController : AppAccountingController
    {
        public string _deleteview = "_DeleteList";
        public string _indexview = "_IndexList";
        public string _updateview = "Update";
        public string _roleview = "_NoRole";
        public SenVietListObject __dataobject;

        public AppAccountingListController() { }

        public void InitData(SenVietListObject _dataobject)
        {
            ViewBag.ajaxoption = _dataobject.appajaxoption.ajaxoption;
            ViewBag.Title = string.Format("Danh sách {0}", _dataobject.sysbusiness.Des.ToLower());

            string actionname = RouteData.Values["action"].ToString().ToLower();
            if (actionname == "delete")
            { ViewBag.Title = string.Format("Xóa {0}", _dataobject.sysbusiness.Des.ToLower()); }
            if (actionname == "create")
            { ViewBag.Title = string.Format("Tạo {0}", _dataobject.sysbusiness.Des.ToLower()); }
            if (actionname == "edit")
            { ViewBag.Title = string.Format("Sửa {0}", _dataobject.sysbusiness.Des.ToLower()); }
            if (actionname == "importexcel")
            { ViewBag.Title = string.Format("Import excel {0}", _dataobject.sysbusiness.Des.ToLower()); }
            this.__dataobject = _dataobject;
        }


        public virtual ActionResult ExportToExcel()
        {
            string FileName = string.Format("{0}{1}.xlsx",this.__dataobject._metaname, DateTime.Now.ToString("yyyyMMddhhmmss"));
            //byte[] bytes = this.__dataobject.ExportToExcel();
            //if (bytes == null) return new EmptyResult();

            return File(this.__dataobject.ExportToExcel(), "text/xls", FileName);
        }


        public ActionResult ImportExcel()
        {
            return PartialView("_ImportExcel");
        }

        [HttpPost]
        public ActionResult ImportExcel(HttpPostedFileBase _file)
        {

            try
            {
                var file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    __dataobject.ImportFromExcel(file.InputStream);
                }
                else
                {
                    //ErrorNotification(_localizationService.GetResource("Admin.Common.UploadFile"));
                    return RedirectToAction(this.ActionReturn());
                }
                //SuccessNotification(_localizationService.GetResource("Admin.Catalog.Products.Imported"));
                return RedirectToAction(this.ActionReturn());
            }
            catch (Exception)
            {
                //ErrorNotification(exc);
                return RedirectToAction(this.ActionReturn());
            }
        }

    }

    public class AppAccountingVoucherController : AppAccountingController
    {
        public string _indexview = "_IndexVoucher";
        public string _deleteview = "_DeleteVoucher";
        public string _updateview = "Update";
        public string _roleview = "_NoRole";

        public SenVietVoucherObject __dataobject;

        public AppAccountingVoucherController() { }

        public void InitData(SenVietVoucherObject _dataobject)
        {
            ViewBag.ajaxoption = _dataobject.appajaxoption.ajaxoption;
            ViewBag.Title = string.Format("Danh sách {0}", _dataobject.sysbusiness.Des.ToLower());

            string actionname = RouteData.Values["action"].ToString().ToLower();
            if (actionname == "delete")
            { ViewBag.Title = string.Format("Xóa {0}", _dataobject.sysbusiness.Des.ToLower()); }
            if (actionname == "create")
            { ViewBag.Title = string.Format("Tạo {0}", _dataobject.sysbusiness.Des.ToLower()); }
            if (actionname == "edit")
            { ViewBag.Title = string.Format("Sửa {0}", _dataobject.sysbusiness.Des.ToLower()); }
            this.__dataobject = _dataobject;
        }

        public virtual ActionResult ExportToExcel()
        {
            string FileName = string.Format("{0}{1}.xlsx", this.__dataobject._metaname, DateTime.Now.ToString("yyyyMMddhhmmss"));
            //byte[] bytes = this.__dataobject.ExportToExcel();
            //if (bytes == null) return new EmptyResult();

            return File(this.__dataobject.ExportToExcel(), "text/xls", FileName);
        }
    }

    public class AppAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            //if (filterContext.HttpContext.Session["AppConnectionString"]==null)
            if (!Accounting.Services.GlobalVariant.IsLogin())
            {
                if ((((filterContext.ActionDescriptor).ControllerDescriptor).ControllerType).FullName.Contains("WebApp.Areas.Pos"))
                {

                    filterContext.Result = new RedirectToRouteResult("AppPosLogin", null);
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult("AppAccLogin", null);
                }
                

            }

            //base.OnActionExecuting(filterContext);
            //bool userNotRight = true;
            //var controllerName = filterContext.RouteData.Values["controller"];
            //var actionName = filterContext.RouteData.Values["action"];
            //SenViet.Uti.Security sec = new Security();

            //string[] userRoles = System.Web.Security.Roles.GetRolesForUser();

            //foreach (string userRole in userRoles)
            //{

            //    if (userRole.ToLower() == "sysadmin")
            //    {
            //        userNotRight = false;
            //        break;
            //    }

            //    if (sec.CheckRight(userRole, controllerName.ToString(), actionName.ToString()))
            //    {
            //        userNotRight = false;
            //        break;
            //    }

            //}



            //if (filterContext.HttpContext.User.Identity == null || !filterContext.HttpContext.User.Identity.IsAuthenticated)
            //{
            //    //System.Web.Security.FormsAuthentication.RedirectToLoginPage();
            //    filterContext.Result = new RedirectResult(System.Web.Security.FormsAuthentication.LoginUrl + "?returnUrl=" +
            //    filterContext.HttpContext.Server.UrlEncode(filterContext.HttpContext.Request.RawUrl));
            //    return;
            //}

            ////Check user right here
            //if (userNotRight)
            //{
            //    filterContext.HttpContext.Response.StatusCode = 302;
            //    filterContext.Result = new HttpUnauthorizedResult();
            //    return;
            //}

            ////#region Test
            ////string AppAccConnection = "AppAccConnectionKey";
            ////if (filterContext.HttpContext.Session[AppAccConnection] == null)
            ////{
            ////    filterContext.Result = new RedirectResult("/acc/accaccount/logon");

            ////    //filterContext.HttpContext.Session[AppAccConnection] = "metadata=res://*/Areas.ACC.Models.AccServiceModel.csdl|res://*/Areas.ACC.Models.AccServiceModel.ssdl|res://*/Areas.ACC.Models.AccServiceModel.msl;provider=System.Data.SqlClient;provider connection string=\"data source=.;initial catalog=lotusviet_web_wm;user id=sa;password=pheonix;multipleactiveresultsets=True;App=EntityFramework\"";
            ////    return;
            ////}

            ////#endregion

        }
    }

    public class AppFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }
    }


}
