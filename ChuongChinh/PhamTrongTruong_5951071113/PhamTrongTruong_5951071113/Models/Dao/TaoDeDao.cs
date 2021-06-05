using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhamTrongTruong_5951071113.Models.Dao
{
    public class TaoDeDao
    {
        TracNghiemDB db = new TracNghiemDB();
        public void Mark(DanhGia exam ,int a)
        {
           // exam.Da_SVLuaChon = db.Da_SVLuaChon.Where(x => x.MaDeThi == exam.MaDeThi).ToList();
           int socauDung = 0;
            var kho_CauHoi2 = new List<KhoCauHoi>();
            var kho_CauHoi1 = new List<KhoCauHoi>();
            var noiDungThis = new List<NoiDungThi>();
            foreach (var item in exam.ketQuaThi.Cau_Hoi)
            {
                var khocauhoi = db.KhoCauHois.Find(item.KhoCauHoi.Ma_CH);
                item.KhoCauHoi = khocauhoi;
                kho_CauHoi2.Add(khocauhoi);
                foreach (var item1 in khocauhoi.D_An)
                {
                    if (exam.ketQuaThi.Da_LuaChon.ToList().Exists(x => x.Ma_Dan == item1.Ma_Dan && item1.TrangThai == true))
                    {
                        socauDung++;
                        kho_CauHoi1.Add(khocauhoi);
                    }

                }

            }
           // Lay Ra So Cau sv da lam dung cua moi chuong
            foreach (var item0 in exam.DanhGiaMucDo)
            {
                noiDungThis.Add(item0);
             
                    item0.noidung.KhoCauHois = kho_CauHoi1.Where(x => x.Ma_Bai == item0.noidung.Ma_Bai).ToList();

            }
            DanhGia danhGia = new DanhGia();
            //danhGia.DanhGiaMucDo = new List<SoLuongChuong>();
            //danhGia.ketQuaThi = new KetQuaThi();
            exam.ketQuaThi.DanhGias = new List<Models.DanhGia > ();
            for (int i = 0; i < exam.DanhGiaMucDo.Count; i++)
            {
                    db = new TracNghiemDB();
                double a1 = (double)(kho_CauHoi1.Where(X => X.MucDọ==1 && X.Ma_Bai==exam.DanhGiaMucDo[i].noidung.Ma_Bai).ToList().Count)*(double)1;
                double a2 = (double)(kho_CauHoi1.Where(X => X.MucDọ==2 && X.Ma_Bai==exam.DanhGiaMucDo[i].noidung.Ma_Bai).ToList().Count) * (double)2 ;
                double a3 = (double)(kho_CauHoi1.Where(X => X.MucDọ==3 && X.Ma_Bai==exam.DanhGiaMucDo[i].noidung.Ma_Bai).ToList().Count) * (double)3;
                double a4 = (double)(kho_CauHoi1.Where(X => X.MucDọ==4 && X.Ma_Bai==exam.DanhGiaMucDo[i].noidung.Ma_Bai).ToList().Count) * (double)4;
           

                
                exam.DanhGiaMucDo[i].nhanbiet= (kho_CauHoi1.Where(X => X.MucDọ == 1 && X.Ma_Bai == exam.DanhGiaMucDo[i].noidung.Ma_Bai).ToList().Count) + "/"+ (kho_CauHoi2.Where(X => X.MucDọ == 1 && X.Ma_Bai == exam.DanhGiaMucDo[i].noidung.Ma_Bai).ToList().Count);
                exam.DanhGiaMucDo[i].Thonghieu = (kho_CauHoi1.Where(X => X.MucDọ == 2 && X.Ma_Bai == exam.DanhGiaMucDo[i].noidung.Ma_Bai).ToList().Count) + "/" + (kho_CauHoi2.Where(X => X.MucDọ == 2 && X.Ma_Bai == exam.DanhGiaMucDo[i].noidung.Ma_Bai).ToList().Count);
                exam.DanhGiaMucDo[i].vandung = (kho_CauHoi1.Where(X => X.MucDọ == 3 && X.Ma_Bai == exam.DanhGiaMucDo[i].noidung.Ma_Bai).ToList().Count) + "/" + (kho_CauHoi2.Where(X => X.MucDọ == 3 && X.Ma_Bai == exam.DanhGiaMucDo[i].noidung.Ma_Bai).ToList().Count);
                exam.DanhGiaMucDo[i].vandungcao = (kho_CauHoi1.Where(X => X.MucDọ == 4 && X.Ma_Bai == exam.DanhGiaMucDo[i].noidung.Ma_Bai).ToList().Count) + "/" + (kho_CauHoi2.Where(X => X.MucDọ == 4 && X.Ma_Bai == exam.DanhGiaMucDo[i].noidung.Ma_Bai).ToList().Count);


                double b1 = (double)(kho_CauHoi2.Where(X => X.MucDọ == 1 && X.Ma_Bai == exam.DanhGiaMucDo[i].noidung.Ma_Bai).ToList().Count) * (double)1 ;
                double b2 = (double)(kho_CauHoi2.Where(X => X.MucDọ == 2 && X.Ma_Bai == exam.DanhGiaMucDo[i].noidung.Ma_Bai).ToList().Count) * (double)2;
                double b3 = (double)(kho_CauHoi2.Where(X => X.MucDọ == 3 && X.Ma_Bai == exam.DanhGiaMucDo[i].noidung.Ma_Bai).ToList().Count) * (double)3;
                double b4 = (double)(kho_CauHoi2.Where(X => X.MucDọ == 4 && X.Ma_Bai == exam.DanhGiaMucDo[i].noidung.Ma_Bai).ToList().Count) * (double)4 ;


                double DG = 0;
                double A = a1 + a2 + a3 + a4;
                double b = b1 + b2 + b3 + b4;
                DG = (double)(A / b) * 10;
                double tile = 0;
               
                    tile = (double)((double)(kho_CauHoi1.Where(x=>x.Ma_Bai== exam.DanhGiaMucDo[i].noidung.Ma_Bai).ToList().Count)/(double)(kho_CauHoi2.Where(x => x.Ma_Bai == exam.DanhGiaMucDo[i].noidung.Ma_Bai).ToList().Count)* (double)100);
               
                Models.DanhGia danhGia1 = new  Models.DanhGia();
                danhGia1.Diem = DG;
                danhGia1.Ma_Bai = exam.DanhGiaMucDo[i].noidung.Ma_Bai;
                exam.ketQuaThi.DanhGias.Add(danhGia1);
               tile = Math.Round(tile, 3);
                exam.DanhGiaMucDo[i].danh_Gia = new Danh_Gia();
                exam.DanhGiaMucDo[i].danh_Gia.NhanXet = new string[100];
                exam.DanhGiaMucDo[i].danh_Gia.SoCauDung = (kho_CauHoi1.Where(x => x.Ma_Bai == exam.DanhGiaMucDo[i].noidung.Ma_Bai).ToList().Count);
                exam.DanhGiaMucDo[i].danh_Gia.TongCau= kho_CauHoi2.Where(x => x.Ma_Bai == exam.DanhGiaMucDo[i].noidung.Ma_Bai).ToList().Count;

              exam.DanhGiaMucDo[i].danh_Gia.NhanXet[0]= "Bạn làm đúng (" + (kho_CauHoi1.Where(x => x.Ma_Bai == exam.DanhGiaMucDo[i].noidung.Ma_Bai).ToList().Count)+"/"+ kho_CauHoi2.Where(x => x.Ma_Bai == exam.DanhGiaMucDo[i].noidung.Ma_Bai).ToList().Count + " đạt tỉ lệ " + tile + " %) \n";
                if (DG < 5)
                {
                    exam.DanhGiaMucDo[i].danh_Gia.DanhGia = 1;

                    exam.DanhGiaMucDo[i].danh_Gia.NhanXet[1]= "-Kiến thức phần này của bạn còn rất hạn chế điểm phần này bài test còn chưa cao.Bạn cần cố gắng cải thiện hơn nữa";
                }
              else if (DG >= 5 && DG< 7)
                {
                    exam.DanhGiaMucDo[i].danh_Gia.DanhGia = 2;
                    exam.DanhGiaMucDo[i].danh_Gia.NhanXet[1] = "Kiến thức của bạn ở phần này chỉ ở mức trung bình. Bạn cần cố gắng hơn để cải thiện thành tích của mình";
                }

               else if (DG >= 7 && DG < 8.5)
                {
                    exam.DanhGiaMucDo[i].danh_Gia.DanhGia = 3;
                    exam.DanhGiaMucDo[i].danh_Gia.NhanXet[1] = "Kiến thức của bạn ở phần này khá tốt. Bạn cố gắng thêm để đặt được số điểm cao hơn nữa";
                }
               else if (DG >= 8.5)
                {
                   exam.DanhGiaMucDo[i].danh_Gia.DanhGia= 4;
                    exam.DanhGiaMucDo[i].danh_Gia.NhanXet[1] = "Kiến thức của bạn ở phần bạn rất làm rất tốt. Bạn cố gắng duy trì phong độ nhé";
                }
                

            }

            db = new TracNghiemDB();
        
            double Hediem = (double)((double)10 / (double)(exam.ketQuaThi.Cau_Hoi.Count));
           
            exam.ketQuaThi.DiêmSo = Math.Round((double)((double)(socauDung) * (double)(Hediem)), 3);
            if (a == 1)
            {
                DeThi deThi = new DeThi();
                deThi.MaTK = exam.ketQuaThi.MaTK;
                deThi.NgayThi = exam.ketQuaThi.NgayThi;
                deThi.DiêmSo = Math.Round((double)((double)(socauDung) * (double)(Hediem)), 3);
                deThi.ThoiGianThi = exam.ketQuaThi.ThoiGianThi;
                db.DeThis.Add(deThi);
                db.SaveChanges();
                deThi.Ma_De = db.DeThis.Where(x => x.MaTK.Equals(deThi.MaTK)).ToList().Last().Ma_De;

                foreach (var item in exam.ketQuaThi.DanhGias)
                {
                    Models.DanhGia danhGia1 = new Models.DanhGia();
                    danhGia1.Diem = item.Diem;
                    danhGia1.Ma_De = deThi.Ma_De;
                    danhGia1.Ma_Bai = item.Ma_Bai;
                    db.DanhGias.Add(danhGia1);
                    db.SaveChanges();

                }

                foreach (var item in exam.ketQuaThi.Cau_Hoi)
                {
                    Cau_Hoi cau_Hoi = new Cau_Hoi();
                    cau_Hoi.MaDe = deThi.Ma_De;
                    cau_Hoi.Ma_CH = item.Ma_CH;
                    db.Cau_Hoi.Add(cau_Hoi);
                    db.SaveChanges();
                }
                foreach (var item in exam.ketQuaThi.Da_LuaChon)
                {
                    Da_LuaChon da_Lua = new Da_LuaChon();
                    da_Lua.Ma_De = deThi.Ma_De;
                    da_Lua.Ma_Dan = item.Ma_Dan;
                    db.Da_LuaChon.Add(da_Lua);
                    db.SaveChanges();
                }


            }


        }

        internal void TimKiem(DanhGia danhGia ,long id)
        {   danhGia.ketQuaThi = db.DeThis.Where(x => x.Ma_De == id).ToList().Last();
            List<NoiDungThi> noiDungs = new List<NoiDungThi>();
            foreach (var item in danhGia.ketQuaThi.Cau_Hoi)
            {
                foreach (var item1 in noiDungs)
                {
                    if (item1.noidung.Ma_Bai == item.KhoCauHoi.Ma_Bai)
                    {
                        item1.SoCau++;
                    }

                }
                if (!noiDungs.Exists(x => x.noidung.Ma_Bai == item.KhoCauHoi.Ma_Bai))
                {
                    noiDungs.Add(new NoiDungThi
                    {
                        noidung = new TracNghiemDB().Bai_Hoc.Find(item.KhoCauHoi.Ma_Bai),
                        SoCau = 0
                    }) ;
                }
                
            }
            danhGia.DanhGiaMucDo = noiDungs;

    

        }

        internal List<KhoCauHoi> Nuberofquestion(long ma_bai, int v)
        {
            List<KhoCauHoi> kho_CauHois = new TracNghiemDB().KhoCauHois.Where(x => x.Ma_Bai == ma_bai && x.MucDọ==v).ToList();
          
            return kho_CauHois;
        }
        private void Random(DeThi De1,long mabai, int sl,int v , string[] ListCH)
        {
            List<KhoCauHoi> kho_CauHois = Nuberofquestion(mabai, v);
            Random random = new Random();
            if (kho_CauHois.Count > 0)
            {
                for (int i = 0; i < sl; i++)
                {
                    while (true)
                    {
                        int dem = 0;
                        int vt = random.Next(kho_CauHois.Count);
                        try {
                        } catch
                        {
                            for (int j = 0; j < ListCH.Length - 1; j++)
                            {
                                if (kho_CauHois[vt].Ma_CH == long.Parse(ListCH[j]))
                                {
                                    dem++;
                                    break;
                                }
                            }
                        }
                        
                        if (dem == 0)
                        {
                            Cau_Hoi cauHoi = new Cau_Hoi();
                            cauHoi.MaDe = De1.Ma_De;
                            cauHoi.Ma_CH = kho_CauHois[vt].Ma_CH;
                            cauHoi.KhoCauHoi = kho_CauHois[vt];
                            De1.Cau_Hoi.Add(cauHoi);
                            kho_CauHois.RemoveAt(vt);
                            break;
                        }
                        kho_CauHois.RemoveAt(vt);

                    }
                  
                }
            }

        }

    

        internal void PhanLoai(DeThi bo_De1, long mabai, TaiKhoan tk)
        {

        }

        internal void CreateTopic(DeThi bo_De1, long mabai,TaiKhoan tk)
        {
            bo_De1.Cau_Hoi = new List<Cau_Hoi>();
            TracNghiemDB db= new TracNghiemDB();
            var danhgia =  db.DS_BaiHoc.SingleOrDefault(x => x.Ma_Bai == mabai && x.Ma_TK.Equals(tk.MaTK));
            string[] ListCH = danhgia.ListCauHoi.Split('/');
            if ((ListCH.Length) == 50)
            {
                danhgia.SoCauDung = 0;
                danhgia.SoCauSai = 0;
                danhgia.ListCauHoi = "";
                ListCH = danhgia.ListCauHoi.Split('/');
                db.SaveChanges();
            }
            
           else if (danhgia.SoCauSai - danhgia.SoCauDung > 40) {
                danhgia.SoCauDung = 0;
                danhgia.SoCauSai = 0;
                danhgia.ListCauHoi = "";
                ListCH = danhgia.ListCauHoi.Split('/');
                db.SaveChanges();

            }

                Random(bo_De1, mabai, 2, 1, ListCH);
                Random(bo_De1, mabai, 2, 2,ListCH);
                Random(bo_De1, mabai, 1, 3, ListCH);
                Random(bo_De1, mabai, 1, 4, ListCH);
        }

        internal void TaoDe(DanhGia da, int sl, int mucdo)
        {
            DeThi deThi = new DeThi();
            foreach (var item in da.DanhGiaMucDo)
            {
                item.SoCau = sl / da.DanhGiaMucDo.Count;
            }
            for (int i = 0; i < sl% da.DanhGiaMucDo.Count; i++)
            {
                da.DanhGiaMucDo[i].SoCau++;
              

            }
            foreach (var item in da.DanhGiaMucDo)
            {
                if (mucdo == 1)
                {
                    LuuDe(deThi,item, 1, 0);
                }
                if (mucdo == 2)
                {
                    LuuDe(deThi, item, 2, 1);
                }
                if (mucdo == 3)
                {
                    LuuDe(deThi, item, 3, 2);
                }
                if (mucdo == 4)
                {
                    LuuDe(deThi, item, 3, 4);
                }
            }
            foreach (var item1 in da.DanhGiaMucDo)
            {
                item1.noidung.KhoCauHois = new List<KhoCauHoi>();
                foreach (var item in deThi.Cau_Hoi)
                {
                    if (item.KhoCauHoi.Ma_Bai == item1.noidung.Ma_Bai)
                    {
                        item1.noidung.KhoCauHois.Add(item.KhoCauHoi);
                    }
                }

            }
            da.ketQuaThi=deThi; 
        }

        private void LuuDe(DeThi deThi, NoiDungThi noiDungThi ,int max,int min)
        {
            for (int i = min; i < max; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    
                    Random(deThi, (long)noiDungThi.noidung.Ma_Bai, noiDungThi.BanMucDo()[i,j],j+1, null);
                }

            }
        }
    }
}