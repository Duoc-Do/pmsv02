using System;
using System.Linq;
using System.Web.Mvc;

namespace WebApp.Areas.Accounting.Controllers
{
    [Authorize]
    public class SysMenuRolePermissionController : AppAccountingFunctionController
    {
        DAL.SenVietGeneralObject _dataobject;
        protected override IActionInvoker CreateActionInvoker()
        {
            _dataobject = new DAL.SenVietGeneralObject(Request, "SysMenuRole");
            this.InitData(_dataobject);
            return base.CreateActionInvoker();
        }
        //
        // GET: /Accounting/Home/
        public ActionResult Index()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult UpdateMenuRole(int RoleID, string MenuID)
        {
            try
            {
                //var db = new WebApp.Areas.Accounting.Models.WebAppAccEntities(GlobalVariant.GetConnection());
                var row = _dataobject._db.SysMenuRoles.Where(m => m.RoleID == RoleID && m.MenuID == MenuID).SingleOrDefault();
                if (row == null) throw new Exception("Không tồn tại.");
                row.IsActive = !row.IsActive;
                _dataobject._db.Entry(row).State = System.Data.Entity.EntityState.Modified;
                _dataobject._db.SaveChanges();
                return Content("succeeded");
            }
            catch (Exception ex)
            {
                return Content(string.Format("failed: {0}", ex.Message));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult UpdateBusinessRole(int RoleID, string BusinessCode, int RoleType)
        {
            try
            {
                string _BusinessCode = BusinessCode.Replace("Browse", "").Replace("Edit", "");

                var rows = _dataobject._db.SysBusinessRoles.Where(m => m.RoleID == RoleID && m.BusinessCode.Contains(_BusinessCode)).ToList();

                foreach (var row in rows)
                {
                    if (row == null) throw new Exception("Không tồn tại.");
                    switch (RoleType)
                    {
                        case 1://IsAdd
                            row.IsAdd = !row.IsAdd;
                            break;
                        case 2://IsDelete
                            row.IsDelete = !row.IsDelete;
                            break;
                        case 3://IsEdit
                            row.IsEdit = !row.IsEdit;
                            break;
                        default:
                            break;
                    }
                    _dataobject._db.Entry(row).State = System.Data.Entity.EntityState.Modified;
                    _dataobject._db.SaveChanges();
                }

                return Content("succeeded");
            }
            catch (Exception ex)
            {
                return Content(string.Format("failed: {0}", ex.Message));
            }
        }

        public ActionResult GetMenuByRole(int Id, string ParentId)
        {
            //var db = new WebApp.Areas.Accounting.Models.WebAppAccEntities(GlobalVariant.GetConnection());
            object rows = null;
            object results = null;


            rows = (from q in _dataobject._db.SysMenuRoles
                    join m in _dataobject._db.SysMenus on q.MenuID equals m.MenuID
                    join o0 in _dataobject._db.SysBusinessRoles on new { q.RoleID, m.BusinessCode } equals new { o0.RoleID, o0.BusinessCode } into o1
                    from o in o1.DefaultIfEmpty()
                    where m.IsActive && q.RoleID == Id && m.ParentID == ParentId
                    select new
                    {
                        RoleID = q.RoleID,
                        MenuID = q.MenuID,
                        Des = q.SysMenu.Des,
                        IsActive = q.IsActive,
                        Childs = _dataobject._db.SysMenus.Where(n => n.ParentID == q.MenuID).Count(),
                        BusinessCode = m.BusinessCode,
                        MenuType = m.MenuType,
                        IsAdd = (o == null ? false : o.IsAdd),
                        IsDelete = (o == null ? false : o.IsDelete),
                        IsEdit = (o == null ? false : o.IsEdit),

                    }
                    ).ToList();


            results = (new { rows = rows });
            return Json(results, JsonRequestBehavior.AllowGet);
        }
    }
}
