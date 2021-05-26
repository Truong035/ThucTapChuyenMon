using PhamTrongTruong_5951071113.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhamTrongTruong_5951071113.Models.Dao;
using System.Web.Script.Serialization;

namespace PhamTrongTruong_5951071113.Controllers
{
    public class HomeController :TracNghiemOnline.Controllers.BaseController
    {
        public ActionResult Index()
        {
            TaiKhoan tk = (TaiKhoan)Session["user"];
           
            ViewBag.Name = tk.Ten;
          return View(new TracNghiemDB().Chuong_Hoc.Where(x=>x.Xóa==true).ToArray());

        }

        public ActionResult OnTap()
        {
            
            TaiKhoan tk = (TaiKhoan)Session["user"];

            ViewBag.Name = tk.Ten;
            return View(new TracNghiemDB().Chuong_Hoc.Where(x=>x.Xóa==true).ToList());
        }

        public ActionResult QuanLy()
        {

          
            TaiKhoan tk = (TaiKhoan)Session["user"];
            ViewBag.Name = tk.Ten;
            return View(new TracNghiemDB().DeThis.Where(x=>x.MaTK.Equals(tk.MaTK)).ToList());
        }
        public ActionResult SeachDethi(long id)
        {

            TaiKhoan tk = (TaiKhoan)Session["user"];
            DanhGia danhGia = new DanhGia();
            new TaoDeDao().TimKiem(danhGia, id);
            Session["lambai"] = danhGia;
            Session["a"] = (int)0;
            ViewBag.Name = tk.Ten;
            return  RedirectToAction("KetQuathi", "Home");

        }
       
        public ActionResult DanhGia()
        {
            TaiKhoan tk = (TaiKhoan)Session["user"];
            ViewBag.Name = tk.Ten;
            return View();
        }
        public ActionResult BaiHoc(long id)
        {
            TaiKhoan tk = (TaiKhoan)Session["user"];
            ViewBag.ID = tk.MaTK;
          
            ViewBag.Name = tk.Ten;
            Session["machuong"]= id;
            foreach (var item in new TracNghiemDB().Bai_Hoc.Where(x=>x.Xoa==true))
            {
                if(!new TracNghiemDB().DS_BaiHoc.Select(x=>x).ToList().Exists(x=>x.Ma_Bai==item.Ma_Bai && x.Ma_TK.Equals(tk.MaTK)))
                {

                    TracNghiemDB tracNghiemDB = new TracNghiemDB();
                    tracNghiemDB.DS_BaiHoc.Add(new DS_BaiHoc()
                    {
                        SoCauSai = 0,
                        Ma_Bai = item.Ma_Bai,
                        Ma_TK = tk.MaTK,
                        SoCauDung = 0,
                        ListCauHoi =""

                    }) ;
                    tracNghiemDB.SaveChanges();
                }
               
            }
            return View(new TracNghiemDB().Bai_Hoc.Where(x=>x.Ma_Chuong==id));
        }
        public ActionResult LienHe()
        {
            TaiKhoan tk = (TaiKhoan)Session["user"];
            ViewBag.Name = tk.Ten;
            return View();
        }

        public void LuuDapAn(string listCH)
        {
            var option = new JavaScriptSerializer().Deserialize<List<Da_LuaChon>>(listCH);

            var danhGia = (DanhGia)Session["lambai"];

            foreach (var item in danhGia.ketQuaThi.Cau_Hoi)
            {
                foreach (var item1  in item.KhoCauHoi.D_An)
                {
                    if (option.Exists(x => x.Ma_Dan == item1.Ma_Dan))
                    {
                        item1.TrangThai = true;
                    }
                    else
                    {
                        item1.TrangThai = false;
                    }
                }

            } 
            danhGia.ketQuaThi.Da_LuaChon = option;
          Session["lambai"]=danhGia;
        }
        public void TaoDe(string nd, int tg,string tgbd, int sl,int mucdo)
        {
            TaiKhoan tk = (TaiKhoan)Session["user"];
            ViewBag.Name = tk.Ten;
            string[] list = nd.Split('/');
            string[] ngay = tgbd.Split('/');
            List<NoiDungThi> bai_Hocs = new List<NoiDungThi>();
            for (int i = 0; i < list.Length-1; i++)
            {
                NoiDungThi noiDungthi = new NoiDungThi();
                noiDungthi.noidung= new TracNghiemDB().Bai_Hoc.Find(int.Parse(list[i]));
                bai_Hocs.Add(noiDungthi);
            }
            DanhGia danhGia = new DanhGia();
            danhGia.DanhGiaMucDo = bai_Hocs;
            danhGia.ketQuaThi = new DeThi();
             new TaoDeDao().TaoDe(danhGia, sl, mucdo);
            Session["noidung"] = bai_Hocs;
            danhGia.ketQuaThi.NgayThi = new DateTime(int.Parse(ngay[0]),int.Parse(ngay[1]), int.Parse(ngay[2]), int.Parse(ngay[3]), int.Parse(ngay[4]), int.Parse(ngay[5]));
            danhGia.ketQuaThi.ThoiGianThi = tg;
            danhGia.ketQuaThi.DiêmSo = 0;
            danhGia.ketQuaThi.TrangThai = false;
            foreach (var item in danhGia.ketQuaThi.Cau_Hoi)
            {
                foreach (var item1 in item.KhoCauHoi.D_An)
                {
                    item1.TrangThai = false;
                }
            }
            DateTime dateTime = new DateTime(int.Parse(ngay[0]), int.Parse(ngay[1]), int.Parse(ngay[2]), int.Parse(ngay[3]), int.Parse(ngay[3]), int.Parse(ngay[4]));
     
           
            danhGia.ketQuaThi.MaTK = tk.MaTK;
            Session["lambai"] = danhGia;
            Session["a"] =(int) 1;

        }
        public ActionResult Thi()
        {
            TaiKhoan tk = (TaiKhoan)Session["user"];
            ViewBag.Name = tk.Ten;
            var danhGia = (DanhGia)Session["lambai"];
            DateTime dateTime = DateTime.Parse(danhGia.ketQuaThi.NgayThi.ToString());
            ViewBag.GioThi = dateTime.ToString("yyyy/MM/dd HH:mm:ss");
            return View(danhGia.ketQuaThi);
        }

        public ActionResult HocBai(long id)
        {

            TaiKhoan tk = (TaiKhoan)Session["user"];
            ViewBag.Name = tk.Ten;
            ViewBag.ID = id;
            ViewBag.machuong =(long)Session["machuong"];
            return View();
        }
        public  ActionResult KetQuathi()
        {
            TaiKhoan tk = (TaiKhoan)Session["user"];
            ViewBag.Name = tk.Ten;
            var danhGia = (DanhGia)Session["lambai"];
           int a= (int) Session["a"];
            new TaoDeDao().Mark(danhGia,a);
            Session["a"]=(int) 0;

            return View(danhGia);
        }
        public JsonResult KetQuaHocTap()
        {
            try
            {
                TaiKhoan tk = (TaiKhoan)Session["user"];
                DeThi thi = (DeThi)Session["dethi"];
                var arr = from c in thi.Cau_Hoi.ToList()
                          select new
                          {
                              c.KhoCauHoi.Ma_CH,
                              c.KhoCauHoi.NoiDung,
                              c.KhoCauHoi.HinhAnh,
                              Dapan = from d in new TracNghiemDB().D_An.Where(x => x.Ma_CH == c.Ma_CH).ToList()
                                      select new
                                      {
                                          d.Ma_Dan,
                                          d.NoiDung,
                                          d.TrangThai,
                                          d.HinhAnh,
                                      }
                          };

                Session["dethi"] ="";
                return Json(new { arr, result = false }, JsonRequestBehavior.AllowGet);
            }
            catch { }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CauHoi(long id)
        {
            try
            {
                TaiKhoan tk = (TaiKhoan)Session["user"];
                DeThi thi = new DeThi();
                new TaoDeDao().CreateTopic(thi, id, tk);
                var arr = from c in thi.Cau_Hoi.ToList()
                          select new
                          {
                              c.KhoCauHoi.Ma_CH,
                              c.KhoCauHoi.NoiDung,
                              c.KhoCauHoi.HinhAnh,
                              Dapan = from d in new TracNghiemDB().D_An.Where(x => x.Ma_CH == c.Ma_CH).ToList()
                                      select new
                                      {
                                          d.Ma_Dan,
                                          d.NoiDung,
                                          TrangThai = false,
                                          d.HinhAnh,

                                      }


                          };
            
                Session["dethi"] = thi;
                
                return Json(new { arr  ,result = false }, JsonRequestBehavior.AllowGet);
            }
           catch { }
            return Json(new { result=false }, JsonRequestBehavior.AllowGet);


        }
        public void Check(long id ,long mabai)
        {
            TaiKhoan tk = (TaiKhoan)Session["user"];
            TracNghiemDB     db=  new TracNghiemDB();
            if (db.D_An.ToList().Exists(x => x.Ma_Dan == id && x.TrangThai == true))
            {
                var danhgia = db.DS_BaiHoc.SingleOrDefault(x => x.Ma_Bai == mabai && x.Ma_TK.Equals(tk.MaTK));
                danhgia.SoCauDung++;
                danhgia.ListCauHoi= danhgia.ListCauHoi.Trim()+new TracNghiemDB().D_An.Find(id).Ma_CH + "/";
                db.SaveChanges();
            }
            else
            {
                var danhgia = db.DS_BaiHoc.SingleOrDefault(x => x.Ma_Bai == mabai && x.Ma_TK.Equals(tk.MaTK));
                danhgia.SoCauSai++;
                db.SaveChanges();
            }

        }

    }
}