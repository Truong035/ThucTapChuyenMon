using PhamTrongTruong_5951071113.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhamTrongTruong_5951071113.Controllers
{
   
    public class LoginController : Controller
    {
        TracNghiemDB db;
        // GET: Login
        public ActionResult Index()
        {
           
            return View();
        }
        public ActionResult Login( TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                var TK = new TracNghiemDB().TaiKhoans.SingleOrDefault(x=>x.MaTK.Equals(taiKhoan.MaTK) && x.MatKhau.Equals(taiKhoan.MatKhau));
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
           return View("Index");
        }


    }
}