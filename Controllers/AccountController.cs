using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using WebApp.Models;
using WebApp.Services.Helpers;
using WebApp.Services;
using WebApp.Services.Captcha;
using System.IO;

namespace WebApp.Controllers
{
    [Authorize]
    //[InitializeSimpleMembership]
    public class AccountController : Controller
    {


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserPromotion()
        {

            return PartialView();

        }

        //Notification
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserPromotion(string couponcode)
        {

            try
            {
                #region kiem tra coupon
                var _dataobject = new WebApp.Areas.Admin.DAL.SenGiftCard(Request);



                var giftcard = _dataobject.GetByCouponCode(couponcode);

                if (giftcard == null) throw new Exception("Mã khuyến mãi không tồn tại!");
                if (giftcard.IsGiftCardActivated) throw new Exception("Mã khuyến mãi đã được kích hoạt!");

                var _dataobject2 = new WebApp.Areas.Admin.DAL.SenUserCash(Request);
                int id = _dataobject2.InsertGiftCard(giftcard);

                #endregion
                giftcard.UserCashId = id;
                giftcard.IsGiftCardActivated = true;
                _dataobject.Update(giftcard);
            }
            catch (Exception ex)
            {
                return Json(new { ketqua = ex.Message });
            }



            return Json(new { ketqua = @"Xin chúc mừng! Mã khuyến mại của bạn đã cập nhật thành công. Xin vui lòng vào phần lịch sử giao dịch để kiểm tra lại." });
        }


        public ActionResult UserProfile()
        {
            var _dataobject = new DAL.SenUser(Request);
            ViewBag.ajaxoption = _dataobject.appajaxoption.ajaxoption;
            var result = _dataobject.GetSenUser();
            return PartialView(result);

        }

        //
        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserProfile(SenUser model)
        {
            var _dataobject = new DAL.SenUser(Request);
            ViewBag.ajaxoption = _dataobject.appajaxoption.ajaxoption;
            try
            {
                var id = _dataobject.Update(model);
                model.UserId = id;
                return PartialView(model);
            }
            catch (Exception)
            {
                return PartialView(model);
            }

        }

        public ActionResult UserCompany()
        {
            var _dataobject = new DAL.SenCompany(Request);
            ViewBag.ajaxoption = _dataobject.appajaxoption.ajaxoption;
            return PartialView(_dataobject.GetDataByUser());
        }

        public ActionResult UserPartner()
        {
            var _dataobject = new WebApp.Areas.Admin.DAL.SenContract(Request);
            ViewBag.ajaxoption = _dataobject.appajaxoption.ajaxoption;

            if (Request.Form["pcontractstatus"] != null)
            {
                ViewBag.ContractStatus = Request.Form["pcontractstatus"];
            }
            else
            {
                ViewBag.ContractStatus = "";
            }
            if (Request.Form["pcommissionpayment"] != null)
            {
                ViewBag.CommissionPayment = Request.Form["pcommissionpayment"];
            }
            else
            {
                ViewBag.CommissionPayment = "";
            }
            return PartialView(_dataobject.GetDataByUser(ViewBag));
        }

        public ActionResult UserPartnerView(int id)
        {
            var _dataobject = new WebApp.Areas.Admin.DAL.SenContract(Request);
            ViewBag.ajaxoption = _dataobject.appajaxoption.ajaxoption;
            var query = _dataobject.ViewByUser(id);

            return PartialView(query);

        }


        public ActionResult UpdateCompany(int companyid)
        {
            try
            {
                var _dataobject = new DAL.SenCompany(Request);
                ViewBag.ajaxoption = _dataobject.appajaxoption.ajaxoption;

                Guid userid = Guid.Parse(Membership.GetUser().ProviderUserKey.ToString());
                var _db = new SenContext();
                var _recservice = _db.SenCompanys.SingleOrDefault(m => m.CompanyId == companyid && m.UserId == userid);
                if (_recservice == null) throw new InvalidOperationException("Doanh nghiệp không tồn tại.");

                return PartialView(_recservice);
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, ex);
                return PartialView();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCompany(int companyid, SenCompany model)
        {
            var _dataobject = new DAL.SenCompany(Request);
            ViewBag.ajaxoption = _dataobject.appajaxoption.ajaxoption;

            if (!ModelState.IsValid) return PartialView(model);
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
                _rec.IsApproved = false;

                _db.Entry(_rec).State = System.Data.Entity.EntityState.Modified;
                _db.SaveChanges();

                if (this.Request["action_return"] != null) return Redirect(this.Request["action_return"].ToString());
                return Json(new { ketqua = "success" });
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, ex);
                return PartialView(model);
                //ModelState.AddModelError("sumerror", "Lỗi.");
                //if (this.Request["action_return"] != null) return Redirect(this.Request["action_return"].ToString());
                //return Content("failed");
                //return Json(new { ketqua = ex.Message });

            }

        }

        public ActionResult UserPayment(int companyid)
        {
            try
            {
                var _dataobject = new DAL.SenCompany(Request);
                ViewBag.ajaxoption = _dataobject.appajaxoption.ajaxoption;

                Guid userid = Guid.Parse(Membership.GetUser().ProviderUserKey.ToString());
                var _db = new SenContext();
                var _recservice = _db.SenCompanys.SingleOrDefault(m => m.CompanyId == companyid && m.UserId == userid);
                if (_recservice == null) throw new InvalidOperationException("Doanh nghiệp không tồn tại.");

                var _dataobject2 = new DAL.SenUserTransaction(Request);
                ViewBag.Balance = WebApp.Services.ExConvert.Data2String(_dataobject2.GetBalance(), "Numeric", "n", "");

                return PartialView(_recservice);
            }
            catch (Exception ex)
            {
                Services.GlobalErrors.Parse(ModelState, ex);
                return PartialView();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserPayment(int companyid, FormCollection model)
        {
            try
            {
                Guid userid = Guid.Parse(Membership.GetUser().ProviderUserKey.ToString());
                var _db = new SenContext();
                var _recservice = _db.SenCompanys.SingleOrDefault(m => m.CompanyId == companyid && m.UserId == userid);
                if (_recservice == null) throw new InvalidOperationException("Doanh nghiệp không tồn tại.");

                var _dataobject = new WebApp.Areas.Admin.DAL.SenUserPayment(Request);
                WebApp.Areas.Admin.Models.SenUserPaymentView collection = new Areas.Admin.Models.SenUserPaymentView();
                collection.Quantity = decimal.Parse(model["Quantity"].ToString());
                collection.UnitPrice = _recservice.SenPackage.UnitPrice;
                collection.Amount = collection.Quantity * collection.UnitPrice;

                if (_dataobject.AmountLimit(collection.Amount)) throw new InvalidOperationException("Số dư không đủ để thanh toán.");

                collection.VoucherDate = DateTime.UtcNow;
                collection.CompanyId = companyid;
                collection.CompanyName = _recservice.Name;
                collection.Description = "Gia hạn ứng dụng.";
                collection.UserId = userid;
                int outputId = _dataobject.Insert(collection);

                if (this.Request["action_return"] != null) return Redirect(this.Request["action_return"].ToString());
                return Json(new { ketqua = "success" });
            }
            catch (Exception ex)
            {
                var _dataobject = new DAL.SenCompany(Request);
                ViewBag.ajaxoption = _dataobject.appajaxoption.ajaxoption;

                Guid userid = Guid.Parse(Membership.GetUser().ProviderUserKey.ToString());
                var _db = new SenContext();
                var _recservice = _db.SenCompanys.SingleOrDefault(m => m.CompanyId == companyid && m.UserId == userid);
                Services.GlobalErrors.Parse(ModelState, ex);

                var _dataobject2 = new DAL.SenUserTransaction(Request);
                ViewBag.Balance = WebApp.Services.ExConvert.Data2String(_dataobject2.GetBalance(), "Numeric", "n", "");

                return PartialView(_recservice);
            }

        }

        public ActionResult UserTransaction()
        {
            var _dataobject = new DAL.SenUserTransaction(Request);

            var datefrom = DateTime.Today;
            var dateto = DateTime.Today;
            WebApp.Services.Helpers.Uti.Date.SetDateRange(this.Request, out datefrom, out dateto);

            ViewBag.datefrom = datefrom;
            ViewBag.dateto = dateto;
            ViewBag.ajaxoption = _dataobject.appajaxoption.ajaxoption;
            ViewBag.LastInfo = _dataobject.GetLastInfo();
            return PartialView(_dataobject.GetDataByUser());
        }


        public ActionResult UserTransactionExportToExcel()
        {
            var _dataobject = new DAL.SenUserTransaction(Request);
            byte[] bytes = null;
            using (var stream = new MemoryStream())
            {
                Export.ExportToXlsx(stream, _dataobject.GetDataByUser(), _dataobject._metaobject.GetMetaTable());
                bytes = stream.ToArray();
            }
            string FileName = string.Format("lotusviet{0}.xlsx", DateTime.Now.ToString("yyyyMMddhhmmss"));
            return File(bytes, "text/xls", FileName);
        }

        public ActionResult PrivateMessage()
        {
            var _dataobject = new DAL.SenPrivateMessage(Request);
            ViewBag.ajaxoption = _dataobject.appajaxoption.ajaxoption;
            return PartialView(_dataobject.GetDataByUser(User.Identity.Name));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PrivateMessage(FormCollection formcontext)
        {
            var _dataobject = new DAL.SenPrivateMessage(Request);
            ViewBag.ajaxoption = _dataobject.appajaxoption.ajaxoption;
            return PartialView(_dataobject.GetDataByUser(User.Identity.Name));
        }


        public ActionResult PrivateMessageSend(Guid? userid)
        {
            var _dataobject = new DAL.SenPrivateMessage(Request);
            ViewBag.ajaxoption = _dataobject.appajaxoption.ajaxoption;
            var data = _dataobject.GetNew(null);
            data.ToUserId = userid ?? Guid.Empty;
            return PartialView(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PrivateMessageSend(FormCollection formcontext)
        {
            try
            {
                var _dataobject = new DAL.SenPrivateMessage(Request);
                _dataobject.PrivateMessageSend();
                if (this.Request["action_return"] != null) return RedirectToLocal(this.Request["action_return"]);
                //ViewBag.ajaxoption = _dataobject.appajaxoption.ajaxoption;
                //return Content("success");
                //return RedirectToAction("PrivateMessage");
                return Json(new { ketqua = "success" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("sumerror", "Lỗi.");
                if (this.Request["action_return"] != null) return RedirectToLocal(this.Request["action_return"]);
                //return Content("failed");
                return Json(new { ketqua = ex.Message });
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PrivateMessageReads()
        {
            var _dataobject = new DAL.SenPrivateMessage(Request);
            if (Request.Form["rowselected"] != null)
            {
                string[] ids = Request.Form["rowselected"].Split(new Char[] { ',' });
                _dataobject.PrivateMessageReads(User.Identity.Name, ids);
            }
            return RedirectToAction("PrivateMessage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PrivateMessageDeleteByRecipients()
        {
            var _dataobject = new DAL.SenPrivateMessage(Request);
            if (Request.Form["rowselected"] != null)
            {
                string[] ids = Request.Form["rowselected"].Split(new Char[] { ',' });
                _dataobject.PrivateMessageDeleteByRecipients(User.Identity.Name, ids);
            }
            return RedirectToAction("PrivateMessage");
        }



        public ActionResult PrivateMessageView(int messageid)
        {
            var _dataobject = new DAL.SenPrivateMessage(Request);
            ViewBag.ajaxoption = _dataobject.appajaxoption.ajaxoption;
            var query = _dataobject.PrivateMessageRead(User.Identity.Name, messageid);

            return PartialView(query);

        }

        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            //if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            //{
            //    return RedirectToLocal(returnUrl);
            //}

            //// If we got this far, something failed, redisplay form
            //ModelState.AddModelError("", "The user name or password provided is incorrect.");
            //return View(model);

            //Membership.


            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);

                    //sẽ gọi ngữ cảnh cho user này
                    WebApp.Services.GlobalUserContext.SetContext();

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập và mật khẩu không chính xác.");
                }
            }

            // If we got this far, something failed, redisplay form
            if (Request.IsAjaxRequest())
            {
                return PartialView("LogOnAjax", model);
            }
            else
            {
                return View(model);

            }


        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            //WebSecurity.Logout();
            //return RedirectToAction("Index", "Home");

            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [CaptchaValidator]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model, bool captchaValid)
        {
            //validate CAPTCHA
            if (bool.Parse(System.Configuration.ConfigurationManager.AppSettings["captchasettings.enabled"]) && !captchaValid)
            {
                ModelState.AddModelError("", "Captcha không chính xác");
            }

            model.UserName = model.Email;
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, false, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    //FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    Roles.AddUserToRole(model.UserName, "Accounting"); // chỉ dùng tạm thời khi egiare chưa có nhiều thành viên
                    Roles.AddUserToRole(model.UserName, "POS"); // chỉ dùng tạm thời khi egiare chưa có nhiều thành viên
                

                    try
                    {

                        System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser(model.UserName);
                        if (user != null && !string.IsNullOrEmpty(model.Email))
                        {

                            #region mail cho người dùng

                            IWebHelper webhelper = new WebHelper(this.HttpContext);
                            var urlactive = Url.Action("RegisterActive").Remove(0, 1);
                            string RegisterActiveUrl = string.Format("{0}{1}?id={2}", webhelper.GetStoreLocation(), urlactive, user.ProviderUserKey.ToString());
                            string emailto = model.Email;

                            // xử lí tạm cần dùng một mẫu tạm để gửi cho khách hàng
                            #region thiết lập token
                            GlobalToken.setToken("Account.RegisterActiveURL", RegisterActiveUrl);
                            GlobalToken.setToken("Account.Name", model.UserName);
                            #endregion


                            var db = new SenContext();
                            var messagetemplate = db.SenMessageTemplates.SingleOrDefault(m => m.Name == "account.registeractive" && m.IsActive == true);

                            var queuedemail = new Models.SenQueuedEmail();
                            queuedemail.CreatedOnUtc = DateTime.UtcNow;
                            queuedemail.EmailAccountId = messagetemplate.EmailAccountId;
                            queuedemail.From = messagetemplate.SenEmailAccount.Email;
                            queuedemail.FromName = messagetemplate.SenEmailAccount.DisplayName;
                            queuedemail.Bcc = messagetemplate.BccEmailAddresses;
                            queuedemail.Subject = GlobalToken.MappingToken(messagetemplate.Subject);
                            queuedemail.To = emailto;
                            queuedemail.ToName = model.UserName;
                            queuedemail.Priority = 0;
                            queuedemail.Body = GlobalToken.MappingToken(messagetemplate.Body);

                            db.SenQueuedEmails.Add(queuedemail);
                            db.SaveChanges();


                            //add user ngam dinh vao SenUser
                            db.Database.ExecuteSqlCommand(string.Format("sp_SenUserSynUser '{0}'", model.UserName));

                            #endregion

                        }
                        else
                        {
                            ModelState.AddModelError("", "Đã có lỗi trong quá trình xử lý. Xin vui lòng liên hệ với chúng tôi để được hỗ trợ. ");
                        }

                    }
                    catch //(System.Exception ex)
                    {
                        ModelState.AddModelError("", "Đã có lỗi trong quá trình xử lý. Xin vui lòng liên hệ với chúng tôi để được hỗ trợ. ");
                        return View(model);
                    }


                    if (Membership.ValidateUser(model.UserName, model.Password))
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, true);

                        //khi xác thực thực hiện trộn cart id cho user id này
                        // eExtensions.MergeCart2User(this.HttpContext, model.UserName);

                        //sẽ gọi ngữ cảnh 

                        //if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        //    && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        //{
                        //    return Redirect(returnUrl);
                        //}
                        return RedirectToAction("Index", "Home");
                    }


                    return RedirectToAction("RegisterSuccess", "Account");
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);

            //if (ModelState.IsValid)
            //{
            //    // Attempt to register the user
            //    try
            //    {
            //        WebSecurity.CreateUserAndAccount(model.UserName, model.Password,false);
            //        WebSecurity.Login(model.UserName, model.Password);
            //        return RedirectToAction("Index", "Home");
            //    }
            //    catch (MembershipCreateUserException e)
            //    {
            //        ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
            //    }
            //}

            //// If we got this far, something failed, redisplay form
            //return View(model);
        }


        [AllowAnonymous]
        public ActionResult RegisterSuccess()
        {
            return View();
        }


        [AllowAnonymous]
        public ActionResult RegisterActive()
        {
            ViewBag.IsError = true;

            if (Request.Params["id"] != null)
            {
                var db = new WebApp.Areas.Admin.Models.AdminContext();

                Guid userid = Guid.Parse(Request.Params["id"].ToString());

                var row = db.aspnet_Memberships.Where(m => m.UserId == userid).SingleOrDefault();
                if (row != null)
                {
                    row.IsApproved = true;
                    try
                    {
                        //UpdateModel(row);
                        db.Entry(row).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        ViewBag.IsError = false;
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordModel model)
        {
            model.UserName = model.Email;
            if (ModelState.IsValid)
            {
                MembershipUser user = Membership.GetUser(model.UserName);
                if (user != null)
                {
                    if (model.Email.ToLower() == user.Email.ToLower())
                    {
                        try
                        {

                            string newPassword = user.ResetPassword();
                            if (!string.IsNullOrEmpty(newPassword))
                            {

                                #region mail cho người dùng

                                IWebHelper webhelper = new WebHelper(this.HttpContext);
                                var urllogin = Url.Action("ChangePassword").Remove(0, 1);
                                string changepasswordUrl = string.Format("{0}{1}", webhelper.GetStoreLocation(), urllogin);

                                string emailto = model.Email;

                                // xử lí tạm cần dùng một mẫu tạm để gửi cho khách hàng
                                #region thiết lập token
                                GlobalToken.setToken("Account.ChangePasswordURL", changepasswordUrl);
                                GlobalToken.setToken("Account.Name", model.UserName);
                                GlobalToken.setToken("Account.Password", newPassword);
                                #endregion


                                var db = new SenContext();
                                var messagetemplate = db.SenMessageTemplates.SingleOrDefault(m => m.Name == "account.forgotpassword" && m.IsActive == true);

                                var queuedemail = new Models.SenQueuedEmail();
                                queuedemail.CreatedOnUtc = DateTime.UtcNow;
                                queuedemail.EmailAccountId = messagetemplate.EmailAccountId;
                                queuedemail.From = messagetemplate.SenEmailAccount.Email;
                                queuedemail.FromName = messagetemplate.SenEmailAccount.DisplayName;
                                queuedemail.Bcc = messagetemplate.BccEmailAddresses;
                                queuedemail.Subject = GlobalToken.MappingToken(messagetemplate.Subject);
                                queuedemail.To = emailto;
                                queuedemail.ToName = model.UserName;
                                queuedemail.Priority = 0;
                                queuedemail.Body = GlobalToken.MappingToken(messagetemplate.Body);

                                db.SenQueuedEmails.Add(queuedemail);
                                db.SaveChanges();
                                #endregion

                                return RedirectToAction("ForgotPasswordSuccess", "Account");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Đã có lỗi trong quá trình xử lý. Xin vui lòng liên hệ với chúng tôi để được hỗ trợ. ");
                            }

                        }
                        catch //(System.Exception ex)
                        {
                            ModelState.AddModelError("", "Đã có lỗi trong quá trình xử lý. Xin vui lòng liên hệ với chúng tôi để được hỗ trợ. ");
                            //ModelState.AddModelError("", "There was an error processing your request [" + ex.Message + "]. Please contact us for assistance.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Tên đăng nhập và địa chỉ email không chính xác. ");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập không tồn tại. ");
                }

            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordSuccess()
        {
            return View();
        }


        public ActionResult ChangePassword()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "Mật khẩu hiện hành không chính xác hoặc mật khẩu mới không phù hợp.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess
        [AllowAnonymous]
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        //
        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = true;// OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", String.Format("Unable to create local account. An account with the name \"{0}\" may already exist.", User.Identity.Name));
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (Membership.ValidateUser(result.UserName, result.ProviderUserId))
            {

                FormsAuthentication.SetAuthCookie(result.UserName, false);
                //sẽ gọi ngữ cảnh cho user này
                WebApp.Services.GlobalUserContext.SetContext();
                return RedirectToLocal(returnUrl);
            }

            //if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            //{
            //    return RedirectToLocal(returnUrl);
            //}

            System.Web.Security.MembershipUser _user = System.Web.Security.Membership.GetUser(result.UserName);
            if (_user != null)
            {
                FormsAuthentication.SetAuthCookie(result.UserName, false);
                //sẽ gọi ngữ cảnh cho user này
                WebApp.Services.GlobalUserContext.SetContext();
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                //OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                //sẽ gọi ngữ cảnh cho user này
                WebApp.Services.GlobalUserContext.SetContext();
                return RedirectToLocal(returnUrl);
            }
            else
            {
                //tao moi user

                MembershipCreateStatus createStatus;
                Membership.CreateUser(result.UserName, result.ProviderUserId, result.UserName, null, null, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    Roles.AddUserToRole(result.UserName, "Accounting"); // chỉ dùng tạm thời khi egiare chưa có nhiều thành viên
                    Roles.AddUserToRole(result.UserName, "POS"); // chỉ dùng tạm thời khi egiare chưa có nhiều thành viên
                    FormsAuthentication.SetAuthCookie(result.UserName, false /* createPersistentCookie */);

                    //add user ngam dinh vao SenUser
                    var db = new SenContext();
                    db.Database.ExecuteSqlCommand(string.Format("sp_SenUserSynUser '{0}'", result.UserName));

                    //var user = db.UserProfiles.Where(m=>m.
                    var senuser = db.SenUsers.Where(m => m.UserProfile.UserName == result.UserName).SingleOrDefault();
                    senuser.SenUserProfiles.Add(new SenUserProfile() { SenUserId = senuser.SenUserId, LastUpdatedDate = DateTime.Now, PropertyName = "account.facebook.accesstoken", PropertyValue = result.ExtraData["accesstoken"].ToString() });
                    if (senuser != null)
                    {
                        senuser.FullName = result.ExtraData["name"].ToString();
                        senuser.Address = result.ExtraData["link"].ToString();
                        senuser.Avatar = @"https://graph.facebook.com/" + result.ProviderUserId + @"/picture?type=large";
                        db.Entry(senuser).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }

                    //email
                    #region mail cho người dùng
                    System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser(result.UserName);
                    IWebHelper webhelper = new WebHelper(this.HttpContext);
                    var urlactive = Url.Action("RegisterActive").Remove(0, 1);
                    string RegisterActiveUrl = string.Format("{0}{1}?id={2}", webhelper.GetStoreLocation(), urlactive, user.ProviderUserKey.ToString());
                    string emailto = user.Email;

                    // xử lí tạm cần dùng một mẫu tạm để gửi cho khách hàng
                    #region thiết lập token
                    GlobalToken.setToken("Account.RegisterActiveURL", RegisterActiveUrl);
                    GlobalToken.setToken("Account.Name", user.UserName);
                    #endregion

                    var messagetemplate = db.SenMessageTemplates.SingleOrDefault(m => m.Name == "account.registeractive" && m.IsActive == true);

                    var queuedemail = new Models.SenQueuedEmail();
                    queuedemail.CreatedOnUtc = DateTime.UtcNow;
                    queuedemail.EmailAccountId = messagetemplate.EmailAccountId;
                    queuedemail.From = messagetemplate.SenEmailAccount.Email;
                    queuedemail.FromName = messagetemplate.SenEmailAccount.DisplayName;
                    queuedemail.Bcc = messagetemplate.BccEmailAddresses;
                    queuedemail.Subject = GlobalToken.MappingToken(messagetemplate.Subject);
                    queuedemail.To = emailto;
                    queuedemail.ToName = user.UserName;
                    queuedemail.Priority = 0;
                    queuedemail.Body = GlobalToken.MappingToken(messagetemplate.Body);

                    db.SenQueuedEmails.Add(queuedemail);
                    db.SaveChanges();

                    #endregion
                    //sẽ gọi ngữ cảnh cho user này
                    WebApp.Services.GlobalUserContext.SetContext();
                }
                else
                {
                    return RedirectToAction("ExternalLoginFailure");
                }
                return RedirectToLocal(returnUrl);

                //// User is new, ask for their desired membership name
                //string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                //ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                //ViewBag.ReturnUrl = returnUrl;
                //return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });



            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            //if (ModelState.IsValid)
            //{
            //    // Insert a new user into the database
            //    using (UsersContext db = new UsersContext())
            //    {
            //        UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
            //        // Check if user already exists
            //        if (user == null)
            //        {
            //            // Insert name into the profile table
            //            db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
            //            db.SaveChanges();

            //            OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
            //            OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

            //            return RedirectToLocal(returnUrl);
            //        }
            //        else
            //        {
            //            ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
            //        }
            //    }
            //}

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
    && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            //if (Url.IsLocalUrl(returnUrl))
            //{
            //    return Redirect(returnUrl);
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Home");
            //}
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
