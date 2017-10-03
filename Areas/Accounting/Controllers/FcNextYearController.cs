
using System;
using System.Web.Mvc;

namespace WebApp.Areas.Accounting.Controllers
{
    public class FcNextYearController : AppAccountingFunctionController
    {
        DAL.SenVietGeneralObject _dataobject;

        protected override IActionInvoker CreateActionInvoker()
        {
            _dataobject = new DAL.SenVietGeneralObject(Request, "FcNextYear");
            this.InitData(_dataobject);
            return base.CreateActionInvoker();
        }

        public ActionResult Index()
        {
            return RedirectToAction("Edit2");
        }

        public ActionResult Edit2()
        {

            //Phải kiểm tra quyền : không phải admin không cho thực hiện chức năng này
            bool _role = false;
            var appuser = Services.GlobalVariant.GetAppUser();
            if (appuser.RoleName == "Systems" || appuser.RoleName == "Administrators") _role = true;
            if (!_role) return PartialView(this._roleview);

            return PartialView("Update2");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit2(FormCollection collection)
        {
            //string SysOptionName = collection["SysOptionName"].ToString();
            //var data = _dataobject._db.SysOptions.Where(m => m.SysOptionName == SysOptionName).SingleOrDefault();
            //data.SysOptionValue = collection["SysOptionValue"].ToString();

            var db = new Models.WebAppAccEntities(Services.GlobalVariant.GetConnection());
            var lockdateyear = DateTime.Parse(Services.GlobalVariant.GetSysOption()["LockDate"].ToString()).Year;
            var thisyear = int.Parse(collection["thisyear"].ToString());

            if (lockdateyear > thisyear) ModelState.AddModelError("", "Năm đã khóa sổ không chuyển được.");
            if (!ModelState.IsValid) return PartialView("Update2");

            try
            {
                db.Database.ExecuteSqlCommand(string.Format("sp_FcNextYear {0}", thisyear));
                ViewBag.FcNextYearMessage = "Đã chuyển thành công";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return PartialView("Update2");

        }

    }
}