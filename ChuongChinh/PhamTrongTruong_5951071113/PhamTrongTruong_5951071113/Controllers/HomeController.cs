using PagedList;
using PhamTrongTruong_5951071113.Models;
using PhamTrongTruong_5951071113.Models.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using DanhGia = PhamTrongTruong_5951071113.Models.Dao.DanhGia;

namespace PhamTrongTruong_5951071113.Controllers
{
    public class HomeController : TracNghiemOnline.Controllers.BaseController
    {
        public ActionResult Index()
        {
            try
            {
                TaiKhoan tk = (TaiKhoan)Session["user"];
                double[] pt = new double[100];
                List<Chuong_Hoc> list = new TracNghiemDB().Chuong_Hoc.Where(x => x.Xóa == true).ToList();
                int i = 0;
                foreach (var item2 in list)
                {
                    pt[i] = 0;
                    double dem = 0;
                    var bai = new TracNghiemDB().Bai_Hoc.Where(x => x.Xoa == true && x.Ma_Chuong == item2.Ma_Chuong);
                    foreach (var item in bai)
                    {
                        int a = 0;

                        try
                        {

                            a = Convert.ToInt32(item.DS_BaiHoc.SingleOrDefault(x => x.Ma_TK.Equals(tk.MaTK) && x.Ma_Bai == item.Ma_Bai).SoCauDung);


                            int b = Convert.ToInt32(item.DS_BaiHoc.SingleOrDefault(x => x.Ma_TK.Equals(tk.MaTK) && x.Ma_Bai == item.Ma_Bai).SoCauSai);
                            if (a != 0 && b != 0)
                            {
                                double c = (double)(a + b);
                                double so1= ((double)(double)(a) /c ) * (double)100;
                                pt[i] = ((double)pt[i] + so1);
                            }
                            else
                            {
                                pt[i] = pt[i];
                            }

                            dem++;
                        }
                        catch
                        {

                        }

                    }

                    pt[i] = pt[i]/(dem);
                    i++;
                }

                ViewBag.pt = pt;
                ViewBag.Name = tk.Ten;
                return View(new TracNghiemDB().Chuong_Hoc.Where(x => x.Xóa == true).ToArray());

            }
            catch {
                return View("Error");
            }
            

        }
        
        public ActionResult OnTap()
        {

            TaiKhoan tk = (TaiKhoan)Session["user"];

            ViewBag.Name = tk.Ten;
            return View(new TracNghiemDB().Chuong_Hoc.Where(x => x.Xóa == true).ToList());
        }

        public ActionResult QuanLy(int? page)
        {
            TaiKhoan tk = (TaiKhoan)Session["user"];
            // 2. Nếu page = null thì đặt lại là 1.
            if (page == null) page = 1;

            // 3. Tạo truy vấn, lưu ý phải sắp xếp theo trường nào đó, ví dụ OrderBy
            // theo LinkID mới có thể phân trang.
            var links = (from l in new TracNghiemDB().DeThis.Where(x => x.MaTK.Equals(tk.MaTK)).ToList()
                         select l).OrderBy(x => x.NgayThi);

            // 4. Tạo kích thước trang (pageSize) hay là số Link hiển thị trên 1 trang
            int pageSize = 4;

            // 4.1 Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);
            ViewBag.Name = tk.Ten;
            // 5. Trả về các Link được phân trang theo kích thước và số trang.
            return View(links.ToPagedList(pageNumber, pageSize));


        }

        public ActionResult SeachDethi(long? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            TaiKhoan tk = (TaiKhoan)Session["user"];
            DanhGia danhGia = new DanhGia();
            new TaoDeDao().TimKiem(danhGia, long.Parse(id.ToString()));
            Session["lambai"] = danhGia;
            Session["a"] = (int)0;
            ViewBag.Name = tk.Ten;
            return RedirectToAction("KetQuathi", "Home");

        }
        public JsonResult ThongKe(int mabai)
        {
            string startut = "";
            TaiKhoan tk = (TaiKhoan)Session["user"];
            List<DeThi> deThis = new List<DeThi>();
            foreach (var item in new TracNghiemDB().DeThis.Where(x => x.MaTK.Equals(tk.MaTK)).ToList())
            {
                if (new TracNghiemDB().DanhGias.Where(x => x.Ma_Bai == mabai).ToList().Exists(x => x.Ma_De == item.Ma_De))
                {
                    deThis.Add(item);
                }

            }

            if (deThis.Count == 0)
            {
                return Json(new
                {
                    arr = new int[0],
                    startut
                }, JsonRequestBehavior.AllowGet);
            }


            List<PhamTrongTruong_5951071113.Models.DanhGia> danhGias = new List<Models.DanhGia>();

            DateTime dateTime = DateTime.Now;
            try
            {
                dateTime = deThis.Where(x => x.MaTK.Equals(tk.MaTK)).ToList().Last().NgayThi;
            }
            catch { }
            double[] DTB = new double[4];

            for (int i = 0; i < 3; i++)
            {
                List<PhamTrongTruong_5951071113.Models.DanhGia> danhGias1 = new List<Models.DanhGia>();
                DateTime dateTime1 = dateTime.AddDays(-3);
                var dethi = deThis.Where(x => x.NgayThi <= dateTime && x.NgayThi > dateTime1);
                var decuoi = dethi.ToList().First();

                foreach (var item in dethi)
                {
                    var danhgia = new TracNghiemDB().DanhGias.SingleOrDefault(x => x.Ma_De == item.Ma_De && x.Ma_Bai == mabai);
                    if (danhgia != null)
                    {

                        if (!danhGias.Exists(x => x.Ma_De == item.Ma_De))
                        {
                            danhGias.Add(danhgia);
                        }
                        danhGias1.Add(danhgia);
                    }
                }
                double dtb = 0; ;
                foreach (var item in danhGias1)
                {
                    dtb += item.Diem;
                }
                try
                {


                    dtb = dtb / (double)danhGias1.Count;
                    DTB[i] = dtb;

                }
                catch { dtb = 0; }
                try
                {
                    dateTime = deThis.Where(x => x.NgayThi < decuoi.NgayThi).ToList().Last().NgayThi;
                }
                catch
                {
                    break;
                }

            }
            double NX1 = (DTB[0] + DTB[1] + DTB[2]) / (double)3;



            if (DTB[0] > DTB[1] && DTB[1] >= DTB[2])
            {
                startut = "Có sự tiến bộ ổn định trong thời gian qua.";
            }

            else if (DTB[0] < DTB[1] && DTB[1] <= DTB[2])
            {
                startut = "Bạn không có sự tiến bộ trong thời gian qua. Kết quả" +
  "các bài kiếm tra có chứa nội dung này đang giảm .";
            }
            else if (DTB[0] <= DTB[1] && DTB[1] >= DTB[2])
            {
                startut = " Trong thời gian gần đây bạn không có tiến bộ. Kết quả các bài kiểm tra có chứa nội dung này giảm xuống.";
            }
            else if (DTB[0] > DTB[1] && DTB[1] <= DTB[2])
            {
                startut = " Bạn có sự tiến bộ hơn trong thời gian trước.";
            }


            if (NX1 < 5)
            {
                startut += "Kiến thức phần này của bạn còn rất hạn chế điểm phần này bài test còn chưa cao.Bạn cần cố gắng cải thiện hơn nữa";
            }
            else if (NX1 >= 5 && NX1 < 7)
            {

                startut += "Kiến thức của bạn ở phần này chỉ ở mức trung bình. Bạn cần cố gắng hơn để cải thiện thành tích của mình";
            }

            else if (NX1 >= 7 && NX1 < 8.5)
            {
                startut += "Kiến thức của bạn ở phần này khá tốt. Bạn cố gắng thêm để đặt được số điểm cao hơn nữa";
            }
            else if (NX1 >= 8.5)
            {
                startut += "Kiến thức của bạn ở phần bạn rất làm rất tốt. Bạn cố gắng duy trì phong độ nhé";
            }



            var arr = from c in danhGias.ToList().OrderBy(x => x.Ma_De)
                      select new
                      {
                          c.Ma_De,
                          c.Diem,

                      };

            return Json(new
            {
                arr,
                startut
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DanhGia()
        {
            TaiKhoan tk = (TaiKhoan)Session["user"];
            ViewBag.Name = tk.Ten;
            return View(new TracNghiemDB().Bai_Hoc.Where(x => x.Xoa == true).ToList());
        }
        public ActionResult BaiHoc(long? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            TaiKhoan tk = (TaiKhoan)Session["user"];
            ViewBag.ID = tk.MaTK;

            ViewBag.Name = tk.Ten;
            Session["machuong"] = id;
            foreach (var item in new TracNghiemDB().Bai_Hoc.Where(x => x.Xoa == true))
            {
                if (!new TracNghiemDB().DS_BaiHoc.Select(x => x).ToList().Exists(x => x.Ma_Bai == item.Ma_Bai && x.Ma_TK.Equals(tk.MaTK)))
                {

                    TracNghiemDB tracNghiemDB = new TracNghiemDB();
                    tracNghiemDB.DS_BaiHoc.Add(new DS_BaiHoc()
                    {
                        SoCauSai = 0,
                        Ma_Bai = item.Ma_Bai,
                        Ma_TK = tk.MaTK,
                        SoCauDung = 0,
                        ListCauHoi = ""

                    });
                    tracNghiemDB.SaveChanges();
                }

            }
            return View(new TracNghiemDB().Bai_Hoc.Where(x => x.Ma_Chuong == id && x.Xoa == true));
        }
        public ActionResult LienHe()
        {
            TaiKhoan tk = (TaiKhoan)Session["user"];
            ViewBag.Name = tk.Ten;
            return View();
        }
        public void Guimail(string nd, string tieude, string name)
        {
            TaiKhoan tk = (TaiKhoan)Session["user"];
            LienHe lienHe = new LienHe();
            lienHe.Ma_TK = tk.MaTK;
            lienHe.NoiDung = nd;
            lienHe.HoTen = name;
            lienHe.Title = tieude;
            lienHe.TrangThai = false;

            TracNghiemDB db = new TracNghiemDB();
            db.LienHes.Add(lienHe);
            db.SaveChanges();
        }

        public void LuuDapAn(string listCH)
        {
            var option = new JavaScriptSerializer().Deserialize<List<Da_LuaChon>>(listCH);

            var danhGia = (DanhGia)Session["lambai"];

            foreach (var item in danhGia.ketQuaThi.Cau_Hoi)
            {
                foreach (var item1 in item.KhoCauHoi.D_An)
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
            Session["lambai"] = danhGia;
        }
        public void TaoDe(string nd, string tgbd, int sl, int mucdo)
        {
            TaiKhoan tk = (TaiKhoan)Session["user"];
            ViewBag.Name = tk.Ten;
            string[] list = nd.Split('/');
            string[] ngay = tgbd.Split('/');
            List<NoiDungThi> bai_Hocs = new List<NoiDungThi>();
            for (int i = 0; i < list.Length - 1; i++)
            {
                NoiDungThi noiDungthi = new NoiDungThi();
                noiDungthi.noidung = new TracNghiemDB().Bai_Hoc.Find(int.Parse(list[i]));
                bai_Hocs.Add(noiDungthi);
            }
            DanhGia danhGia = new DanhGia();
            danhGia.DanhGiaMucDo = bai_Hocs;
            danhGia.ketQuaThi = new DeThi();
            new TaoDeDao().TaoDe(danhGia, sl, mucdo);
            Session["noidung"] = bai_Hocs;
            danhGia.ketQuaThi.NgayThi = new DateTime(int.Parse(ngay[0]), int.Parse(ngay[1]), int.Parse(ngay[2]), int.Parse(ngay[3]), int.Parse(ngay[4]), int.Parse(ngay[5])).AddMinutes(2 * sl);
            danhGia.ketQuaThi.ThoiGianThi = 2 * sl;
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
            Session["a"] = (int)1;

        }
        public ActionResult Thi()
        {
            try
            {
                TaiKhoan tk = (TaiKhoan)Session["user"];
                ViewBag.Name = tk.Ten;
                var danhGia = (DanhGia)Session["lambai"];
                DateTime dateTime = DateTime.Parse(danhGia.ketQuaThi.NgayThi.ToString());
                ViewBag.GioThi = dateTime.ToString("yyyy/MM/dd HH:mm:ss");
                return View(danhGia.ketQuaThi);
            }
            catch
            {
                return RedirectToAction("OnTap");
            }

        }

        public ActionResult HocBai(long? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            TaiKhoan tk = (TaiKhoan)Session["user"];
            ViewBag.Name = tk.Ten;
            ViewBag.ID = id;
            ViewBag.machuong = (long)Session["machuong"];
            return View();
        }
        public JsonResult kiemTrade()
        {
            if (Session["lambai"] == null)
            {
                return Json(new { startust = false }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { startust = true }, JsonRequestBehavior.AllowGet);
        }
        public void HuyDe()
        {
            Session["lambai"] = null;
        }
        public ActionResult KetQuathi()
        {
            TaiKhoan tk = (TaiKhoan)Session["user"];
            ViewBag.Name = tk.Ten;
            var danhGia = (DanhGia)Session["lambai"];
            int a = (int)Session["a"];
            new TaoDeDao().Mark(danhGia, a);
            Session["a"] = (int)0;

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

                Session["dethi"] = "";
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

                return Json(new { arr, result = false }, JsonRequestBehavior.AllowGet);
            }
            catch { }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);


        }
        public void Check(long id, long mabai)
        {
            TaiKhoan tk = (TaiKhoan)Session["user"];
            TracNghiemDB db = new TracNghiemDB();
            if (db.D_An.ToList().Exists(x => x.Ma_Dan == id && x.TrangThai == true))
            {
                var danhgia = db.DS_BaiHoc.SingleOrDefault(x => x.Ma_Bai == mabai && x.Ma_TK.Equals(tk.MaTK));
                danhgia.SoCauDung++;
                danhgia.ListCauHoi = danhgia.ListCauHoi.Trim() + new TracNghiemDB().D_An.Find(id).Ma_CH + "/";
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