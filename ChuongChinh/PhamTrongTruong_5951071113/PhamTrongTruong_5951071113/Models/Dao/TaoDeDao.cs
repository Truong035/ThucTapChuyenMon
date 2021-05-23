using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhamTrongTruong_5951071113.Models.Dao
{
    public class TaoDeDao
    {
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
            if ((ListCH.Length) == 7)
            {
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

        internal DeThi TaoDe(List<NoiDungThi> bai_Hocs, int sl, int mucdo)
        {
            DeThi deThi = new DeThi();
            foreach (var item in bai_Hocs)
            {
                item.SoCau = sl / bai_Hocs.Count;
            }
            for (int i = 0; i < sl%bai_Hocs.Count; i++)
            {
                bai_Hocs[i].SoCau++;
              

            }
            foreach (var item in bai_Hocs)
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
            

            return deThi; 
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