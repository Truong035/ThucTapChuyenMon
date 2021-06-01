using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using PhamTrongTruong_5951071113.Models;
namespace PhamTrongTruong_5951071113.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        [AllowAnonymous]
   
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index( TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                string mk = GetMD5(taiKhoan.MatKhau);
                var TK = new TracNghiemDB().TaiKhoans.SingleOrDefault(x => x.MaTK.Equals(taiKhoan.MaTK) && x.MatKhau.Equals(mk));
                if (TK != null)
                {

                    Session.Add("Admin", TK);

                    return RedirectToAction("Index", "/Admin/Index");

                }
                else
                {
                    ModelState.AddModelError("", "Đăng Nhập Không Đúng ");
                }

            }

            return View(taiKhoan);

           
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
    }
}