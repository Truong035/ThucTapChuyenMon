using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mail;
using System.Web.Mvc;
using HtmlAgilityPack;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PagedList;
using PhamTrongTruong_5951071113.Models;

namespace PhamTrongTruong_5951071113.Controllers
{
 
   
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public List<KhoCauHoi> getdl()
        {
            List<KhoCauHoi> khocauhoi = new List<KhoCauHoi>();
            string link = "https://khoahoc.vietjack.com/thi-online/10-de-thi-thu-tot-nghiep-thpt-quoc-gia-nam-2021-mon-vat-li-co-loi-giai-chi-tiet/62201/ket-qua";
            HtmlWeb htmlWeb = new HtmlWeb()
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8  //Set UTF8 để hiển thị tiếng Việt
            };

            HtmlDocument document = htmlWeb.Load(link);

            var t0 = document.DocumentNode.SelectNodes("//div[@class='col-md-12']").ToList();
    
            //var threadItems = threadI.SelectNodes("//div[@class='col-md-12']").ToList();
            foreach (var s in t0)
            {
                //KhoCauHoi cauhoi = new KhoCauHoi();

                //var c = s.SelectSingleNode(".//div[@class='question-name']/p");
                //cauhoi.NoiDung = c.InnerText;
                //cauhoi.D_An = new List<D_An>();
                //for (int i = 0; i < 4; i++)
                //{
                //    cauhoi.D_An.Add(new D_An
                //    {

                //    });

                //}
                //var dapandung = s.SelectSingleNode(".//div[@ class='anwser-item col-xs-12 d-flex correct ']/p").InnerText;

                //var dapan = s.SelectNodes(".//div[@ class='anwser-item col-xs-12 d-flex']").ToList();
                //foreach (var cauhoisai in dapan)
                //{
                //    var dapansai = cauhoisai.SelectSingleNode(".//p").InnerText;
                //}


            }

            //   Lkhocauhoi = new List<KhoCauHoi>();
            return khocauhoi;

        }

        public AccountController()
        {


        }
        [AllowAnonymous]
        public ActionResult DoiMatKhau()
        {
            try
            {
                var tk = (TaiKhoan)Session["user"];

                RegisterViewModel RegisterViewMode = new RegisterViewModel();
                RegisterViewMode.Email = tk.MaTK;
                RegisterViewMode.Name = tk.Ten;
                return View(RegisterViewMode);
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }
          
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
            Session["user"]=null;
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
            try
            {
                if (ModelState.IsValid)
                {
                    string mk = GetMD5(taiKhoan.MatKhau);
                    var TK = new TracNghiemDB().TaiKhoans.SingleOrDefault(x => x.MaTK.Trim().Equals(taiKhoan.MaTK) && x.Quyen == false && x.MatKhau.Equals(mk));
                    if (TK != null)
                    {
                        if (TK.TrangThai == true)
                        {
                            Session.Add("user", TK);
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Tài khoản của bạn đã bị khóa");
                        }

                    }
                  
                    else
                    {
                        ModelState.AddModelError("", "Đăng Nhập Không Đúng ");
                    }

                }
            }
            catch
            {
                ModelState.AddModelError("", "Vui lòng nhập mật khẩu");
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
                    //  Session.Add("taotk", taiKhoan1);
                    //     SendEmail(model.Email, "Xác nhận tài khoản", "Please click here to confirm  <a class='btn btn-success' href =https://localhost:44343/Account/CreateAccount >Xác Nhận</a> ");
                    Session.Add("user", taiKhoan1);
                    return RedirectToAction("Index", "Home");
                //    return View("ForgotPasswordConfirmation");
                   // return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Email Đã Tồn Tại");
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [AllowAnonymous]
        public ActionResult CreateAccount()
        {
            try
            {
              var tk=(TaiKhoan)Session["taotk"];

                if (tk != null)
                {

                    TracNghiemDB tracNghiemDB = new TracNghiemDB();
                    tracNghiemDB.TaiKhoans.Add(tk);
                    tracNghiemDB.SaveChanges();
                    Session.Add("user",tk);
                    return RedirectToAction("Index", "Home");
                }

            }
            catch { }

            return View("Error");


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
                var Tk = new TracNghiemDB().TaiKhoans.SingleOrDefault(x=>x.MaTK.Equals(loginInfo.Email) && x.MatKhau!=null);
                

                if (Tk != null)
                {
                    if (Tk.TrangThai == true)
                    {
                        Session["user"] = Tk;

                        SendEmail(loginInfo.Email, "Xác nhận mật khẩu", "Please reset your password by clicking <a class='btn btn-success' href =https://localhost:44343/Account/DoiMatKhau1 >Xác Nhận</a> ");
                        return View("ForgotPasswordConfirmation");
                    }

                    ModelState.AddModelError("", "Tài khoản của bạn đã bị khóa");

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
        [AllowAnonymous]
        public ActionResult GioiThieu()
        {
            getdl();

            return View();
        }

        [AllowAnonymous]
       
        public ActionResult TimKiem(string txtsearch, int? page)
        {
            if (page == null)  page = 1;

            List<KhoCauHoi> links = new List<KhoCauHoi>();
                //new TracNghiemDB().KhoCauHois.Where(x => x.NoiDung.Contains(txtsearch)).OrderBy(x=>x.MucDọ).ToList();
            // 3. Tạo truy vấn, lưu ý phải sắp xếp theo trường nào đó, ví dụ OrderBy
            // theo LinkID mới có thể phân trang.
            foreach (var item in new TracNghiemDB().Bai_Hoc.Where(x => x.Xoa == true && x.Tên_Bai.Contains(txtsearch)))
            {

                links.AddRange(new TracNghiemDB().KhoCauHois.Where(x => x.Ma_Bai == item.Ma_Bai));

            }

            //var links = new TracNghiemDB().KhoCauHois.Where(x => x.Xoa == true && x.NoiDung.Contains(id)).OrderBy(x => x.MucDọ);

            // 4. Tạo kích thước trang (pageSize) hay là số Link hiển thị trên 1 trang
            int pageSize = 10;

            // 4.1 Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);
            ViewBag.search = txtsearch;

            // 5. Trả về các Link được phân trang theo kích thước và số trang.
            return View(links.ToPagedList(pageNumber, pageSize)) ;

          //  return View();
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
            TracNghiemDB tracNghiemDB = new TracNghiemDB();
            var Tk =  tracNghiemDB.TaiKhoans.Find(loginInfo.Email);
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            if (Tk == null)
            {
                TaiKhoan taiKhoan1 = new TaiKhoan();
                taiKhoan1.MaTK = loginInfo.Email;
                taiKhoan1.TrangThai = true;
                taiKhoan1.Quyen = false;
                taiKhoan1.NgayTao = DateTime.UtcNow;
                taiKhoan1.Ten = loginInfo.DefaultUserName;
              //  TracNghiemDB tracNghiemDB = new TracNghiemDB();
                Tk = taiKhoan1;
                taiKhoan1.MatKhau = "";
                tracNghiemDB.TaiKhoans.Add(taiKhoan1);
                tracNghiemDB.SaveChanges();
            }
            else if (Tk.TrangThai == false)
            {
                ModelState.AddModelError("", "Tài khoản của bạn đã bị khóa");
                return View("Login", new TaiKhoan { MaTK = Tk.MaTK, MatKhau = GetMD5(Tk.MatKhau), TrangThai = false });
             
                   
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