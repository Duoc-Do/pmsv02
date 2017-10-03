using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using WebApp.Models;

namespace WebApp.Controllers
{

    [Authorize]
    public class CompanyController : Controller
    {
        public ActionResult Update(int id)
        {
            try
            {
                Guid userid = Guid.Parse(Membership.GetUser().ProviderUserKey.ToString());
                var _db = new SenContext();
                var _recservice = _db.SenServiceViews.SingleOrDefault(m => m.ServiceId == id && m.UserIdCreated == userid);
                if (_recservice == null) throw new InvalidOperationException("Doanh nghiệp không tồn tại.");

                var _rec = _recservice.SenCompany;
                return View(_rec);
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, ex);
                return View("UpdateError");
            }
        }




        //
        // POST: /Account/Register

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(int id, SenCompany model)
        {
            if (!ModelState.IsValid) return View(model);
            try
            {
                Guid userid = Guid.Parse(Membership.GetUser().ProviderUserKey.ToString());
                var _db = new SenContext();
                var _recservice = _db.SenCompanys.SingleOrDefault(m => m.CompanyId == model.CompanyId && m.UserId == userid);
                if (_recservice == null) throw new InvalidOperationException("Doanh nghiệp không tồn tại.");

                var _rec = _recservice;

                _rec.Name = model.Name;
                _rec.Address = model.Address;
                _rec.TaxCode = model.TaxCode;
                _rec.Email = model.Email;
                _rec.Telephone = model.Telephone;
                _rec.FaxNumber = model.FaxNumber;
                //_rec.PackageId = model.PackageId;
                _rec.Website = model.Website;
                _rec.Logo = model.Logo;
                if (_rec.Name!=model.Name)
                {
                    _rec.IsApproved = false;
                }

                _db.Entry(_rec).State = System.Data.Entity.EntityState.Modified;
                _db.SaveChanges();

                return RedirectToAction("RegisterSuccess");
            }
            catch (Exception)
            {

                return View(model);

            }

        }


        public ActionResult Register(int id)
        {
            var _db = new SenContext();
            var _rec = new SenCompany();
            List<Models.SenApplication> apps;

            if (id == 0)
            {
                apps = _db.SenApplications.ToList();
            }
            else
            {
                apps = _db.SenApplications.Where(m => m.ApplicationId == id).ToList();
            }

            ViewBag.apps = apps;
            return View(_rec);
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(int id, SenCompany model)
        {
            var _db = new SenContext();

            try
            {
                if (!ModelState.IsValid) throw new Exception();
                // If we got this far, something failed, redisplay form
                model.CreateDate = DateTime.UtcNow;
                model.UserId = Guid.Parse(Membership.GetUser().ProviderUserKey.ToString());
                model.IsApproved = false;
                model.IsLockedOut = false;

                _db.SenCompanys.Add(model);
                _db.SaveChanges();

                var userservicenew = new Models.SenService() { CompanyId = model.CompanyId, UserId = model.UserId ?? Guid.Empty };
                _db.SenServices.Add(userservicenew);
                _db.SaveChanges();

                return RedirectToAction("RegisterSuccess");
            }
            catch (Exception)
            {

                List<Models.SenApplication> apps;

                if (id == 0)
                {
                    apps = _db.SenApplications.ToList();
                }
                else
                {
                    apps = _db.SenApplications.Where(m => m.ApplicationId == id).ToList();
                }

                ViewBag.apps = apps;

                return View(model);

            }

        }

        public ActionResult RegisterSuccess()
        {
            return View();
        }

        //public ActionResult UserPayment(int companyid)
        //{
        //    try
        //    {
        //        var _dataobject = new DAL.SenCompany(Request);
        //        ViewBag.ajaxoption = _dataobject.appajaxoption.ajaxoption;

        //        Guid userid = Guid.Parse(Membership.GetUser().ProviderUserKey.ToString());
        //        var _db = new SenContext();
        //        var _recservice = _db.SenCompanys.SingleOrDefault(m => m.CompanyId == companyid && m.UserId == userid);
        //        if (_recservice == null) throw new InvalidOperationException("Doanh nghiệp không tồn tại.");

        //        var _rec = _recservice;
        //        return PartialView(_rec);
        //    }
        //    catch (Exception ex)
        //    {
        //        Services.GlobalErrors.Parse(ModelState, ex);
        //        return PartialView();
        //    }
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult UserPayment(int companyid,FormCollection model)
        //{
        //    try
        //    {
        //        Guid userid = Guid.Parse(Membership.GetUser().ProviderUserKey.ToString());
        //        var _db = new SenContext();
        //        var _recservice = _db.SenCompanys.SingleOrDefault(m => m.CompanyId == companyid && m.UserId == userid);
        //        if (_recservice == null) throw new InvalidOperationException("Doanh nghiệp không tồn tại.");

        //        var _dataobject = new WebApp.Areas.Admin.DAL.SenUserPayment(Request);
        //        WebApp.Areas.Admin.Models.SenUserPaymentView collection = new Areas.Admin.Models.SenUserPaymentView();
        //        collection.Quantity = decimal.Parse(model["Quantity30"].ToString()) * 30;
        //        collection.UnitPrice = _recservice.SenPackage.UnitPrice;
        //        collection.Amount = collection.Quantity * collection.UnitPrice;

        //        if (_dataobject.AmountLimit(collection.Amount)) throw new InvalidOperationException("Số dư không đủ để thanh toán.");

        //        collection.VoucherDate = DateTime.UtcNow;
        //        collection.CompanyId = companyid;
        //        collection.CompanyName = _recservice.Name;
        //        collection.Description = "Gia hạn ứng dụng.";
        //        collection.UserId = userid;
        //        int outputId = _dataobject.Insert(collection);



        //        if (this.Request["action_return"] != null) return Redirect(this.Request["action_return"].ToString());
        //        return Json(new { ketqua = "success" });
        //    }
        //    catch (Exception ex)
        //    {

        //        if (this.Request["action_return"] != null) return Redirect(this.Request["action_return"].ToString());
        //        //return Content("failed");
        //        return Json(new { ketqua = ex.Message });

        //    }

        //}

    }
}
