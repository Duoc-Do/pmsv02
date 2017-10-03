using System;
using System.Web.Mvc;
using WebApp.Areas.Admin.DAL;

namespace WebApp.Areas.Admin
{
    [Authorize(Roles = "Admin")]
    public abstract class AppController : Controller
    {
    }

    [Authorize(Roles = "Admin")]
    public abstract class AppAdminController : Controller
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

    public class AppAdminListController : AppAdminController
    {
        public string _deleteview = "_DeleteList";
        public string _indexview = "_IndexList";
        public string _updateview = "Update";
        public string _roleview = "_NoRole";
        public SenVietListObject __dataobject;
        public AppAdminListController() { }

        public void InitData(SenVietListObject _dataobject)
        {
            ViewBag.appajaxoption = _dataobject.appajaxoption;
            ViewBag.ajaxoption = _dataobject.appajaxoption.ajaxoption;
            //ViewBag.Title = string.Format("Danh sách {0}", _dataobject.sysbusiness.Des.ToLower());
            ViewBag.Title = _dataobject.appajaxoption.ajaxoption.ContainsKey("title") ? _dataobject.appajaxoption.ajaxoption["title"] : "";

            string actionname = RouteData.Values["action"].ToString().ToLower();
            if (actionname == "delete")
            { ViewBag.Title = string.Format("Xóa {0}", ""); }
            if (actionname == "create")
            { ViewBag.Title = string.Format("Tạo {0}", ""); }
            if (actionname == "edit")
            { ViewBag.Title = string.Format("Sửa {0}", ""); }

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



    [Authorize(Roles = "Admin,CRM")]
    public abstract class AppCRMController : Controller
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

    public class AppCRMListController : AppCRMController
    {
        public string _deleteview = "_DeleteList";
        public string _indexview = "_IndexList";
        public string _updateview = "Update";
        public string _roleview = "_NoRole";

        public AppCRMListController() { }

        public void InitData(SenVietListObject _dataobject)
        {
            ViewBag.appajaxoption = _dataobject.appajaxoption;
            ViewBag.ajaxoption = _dataobject.appajaxoption.ajaxoption;
            ViewBag.Title = _dataobject.appajaxoption.ajaxoption.ContainsKey("title") ? _dataobject.appajaxoption.ajaxoption["title"] : "";

            string actionname = RouteData.Values["action"].ToString().ToLower();
            if (actionname == "delete")
            { ViewBag.Title = string.Format("Xóa {0}", ""); }
            if (actionname == "create")
            { ViewBag.Title = string.Format("Tạo {0}", ""); }
            if (actionname == "edit")
            { ViewBag.Title = string.Format("Sửa {0}", ""); }
        }

    }
}
