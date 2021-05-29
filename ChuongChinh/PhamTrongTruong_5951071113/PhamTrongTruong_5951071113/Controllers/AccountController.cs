﻿using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mail;
using System.Web.Mvc;
using System.Windows.Forms;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PhamTrongTruong_5951071113.Models;

namespace PhamTrongTruong_5951071113.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {


        }
        [AllowAnonymous]
        public ActionResult DoiMatKhau()
        {
            var tk=(TaiKhoan)Session["user"];
            RegisterViewModel RegisterViewMode = new RegisterViewModel();
            RegisterViewMode.Email = tk.MaTK;
            RegisterViewMode.Name = tk.Ten;
            return View(RegisterViewMode);
        }
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DoiMatKhau(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                      TracNghiemDB tracNghiemDB = new TracNghiemDB();
                     TaiKhoan taiKhoan1 =  tracNghiemDB.TaiKhoans.Find(model.Email);
                    taiKhoan1.MaTK = model.Email;
                    string mk = GetMD5(model.ConfirmPassword);
                    taiKhoan1.MatKhau = mk;
                    taiKhoan1.Quyen = false;
                    taiKhoan1.NgayTao = DateTime.UtcNow;
                    taiKhoan1.Ten = model.Name;
                    taiKhoan1.TrangThai = true;
                   tracNghiemDB.SaveChanges();
                    Session["user"]= taiKhoan1;
                    return RedirectToAction("Index", "Home");
                
               // ModelState.AddModelError("", "Email Đã Tồn Tại");
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }
        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {

            //uthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            // AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }



        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login (TaiKhoan taiKhoan, string returnUrl)
        {

            if (ModelState.IsValid)
            {
                string mk = GetMD5(taiKhoan.MatKhau);
                var TK = new TracNghiemDB().TaiKhoans.SingleOrDefault(x => x.MaTK.Equals(taiKhoan.MaTK) && x.Quyen==true && x.MatKhau.Equals(mk));
                if (TK != null)
                {

                    Session.Add("user", TK);

                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ModelState.AddModelError("", "Đăng Nhập Không Đúng ");
                }

            }

            return View(taiKhoan);

        }

        //
        // GET: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DoiMatKhau1(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                TracNghiemDB tracNghiemDB = new TracNghiemDB();
                TaiKhoan taiKhoan1 = tracNghiemDB.TaiKhoans.Find(model.Email);
                taiKhoan1.MaTK = model.Email;
                string mk = GetMD5(model.ConfirmPassword);
                taiKhoan1.MatKhau = mk;
                taiKhoan1.Quyen = false;
                taiKhoan1.NgayTao = DateTime.UtcNow;
                taiKhoan1.Ten = model.Name;
                taiKhoan1.TrangThai = true;
                tracNghiemDB.SaveChanges();
                Session["user"] = taiKhoan1;
                return RedirectToAction("Index", "Home");

                // ModelState.AddModelError("", "Email Đã Tồn Tại");
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
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
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var Tk = new TracNghiemDB().TaiKhoans.Find(model.Email);

                if (Tk == null)
                {
                    TaiKhoan taiKhoan1 = new TaiKhoan();
                    taiKhoan1.MaTK = model.Email;
                    string mk = GetMD5(model.ConfirmPassword);
                    taiKhoan1.MatKhau = mk;
                    taiKhoan1.Quyen = false;
                    taiKhoan1.NgayTao = DateTime.UtcNow;
                    taiKhoan1.Ten = model.Name;
                    taiKhoan1.TrangThai = true;
                    TracNghiemDB tracNghiemDB = new TracNghiemDB();
                    tracNghiemDB.TaiKhoans.Add(taiKhoan1);
                    tracNghiemDB.SaveChanges();
                    Session.Add("user", taiKhoan1);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Email Đã Tồn Tại");
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }
        
        public string GetMD5(string chuoi)
        {
            string str_md5 = "";
            byte[] mang = System.Text.Encoding.UTF8.GetBytes(chuoi);

            MD5CryptoServiceProvider my_md5 = new MD5CryptoServiceProvider();
            mang = my_md5.ComputeHash(mang);

            foreach (byte b in mang)
            {
                str_md5 += b.ToString("X2");
            }

            return str_md5;
        }
        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult DoiMatKhau1()
        {
           var tk = (TaiKhoan)Session["user"];
            RegisterViewModel RegisterViewMode = new RegisterViewModel();
            RegisterViewMode.Email = tk.MaTK;
            RegisterViewMode.Name = tk.Ten;
            return View(RegisterViewMode);
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel loginInfo)
        {
            if (ModelState.IsValid)
            {
                var Tk = new TracNghiemDB().TaiKhoans.Find(loginInfo.Email);
                
                if (Tk != null)
                {
                   
                    Session["user"] = Tk;
             
                    SendEmail(loginInfo.Email, "Xác nhận mật khẩu", "Please reset your password by clicking <a class='btn btn-success' href =https://localhost:44343/Account/DoiMatKhau1 >Xác Nhận</a> ");
                    return View("ForgotPasswordConfirmation");
                    
                }
                else
                {
                    ModelState.AddModelError("", "Email bạn nhập không tồn tại");
                }
                
            }

            // If we got this far, something failed, redisplay form
            return View(loginInfo);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

   
        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

  
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }
            var Tk = new TracNghiemDB().TaiKhoans.Find(loginInfo.Email);
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            if (Tk == null)
            {
                TaiKhoan taiKhoan1 = new TaiKhoan();
                taiKhoan1.MaTK = loginInfo.Email;
                taiKhoan1.TrangThai = true;
                taiKhoan1.Quyen = false;
                taiKhoan1.NgayTao = DateTime.UtcNow;
                taiKhoan1.Ten = loginInfo.DefaultUserName;
                TracNghiemDB tracNghiemDB = new TracNghiemDB();
                Tk = taiKhoan1;

                tracNghiemDB.TaiKhoans.Add(taiKhoan1);
                tracNghiemDB.SaveChanges();
            }
            Session.Add("user", Tk);

            return RedirectToAction("Index", "Home");
            
        }
        public void SendEmail(string address, string subject, string message)
        {
            string email = "tmooquiz40@gmail.com";
            string password = "0353573467";

            var loginInfo = new NetworkCredential(email, password);
            var msg = new System.Net.Mail.MailMessage();
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);

            msg.From = new MailAddress(email);
            msg.To.Add(new MailAddress(address));
            msg.Subject = subject;
            msg.Body = message;
            msg.IsBodyHtml = true;

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(msg);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}