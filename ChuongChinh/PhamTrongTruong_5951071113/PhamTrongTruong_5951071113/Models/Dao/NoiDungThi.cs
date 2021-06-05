using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhamTrongTruong_5951071113.Models.Dao
{
    public class NoiDungThi
    {
        public Bai_Hoc noidung { get; set; }
       public int SoCau { get; set; }
       public Danh_Gia danh_Gia { get; set; }
        public string nhanbiet { get; set; }
        public string Thonghieu { get; set; }
        public string vandung { get; set; }
        public string vandungcao { get; set; }
 
        public int[,] BanMucDo()
        {
            int[,] mang = new int[4,4];
            //de
            mang[0,3] = kiemtra((double)(SoCau) * ((double)10 / (double)100));
             
            mang[0,2] = kiemtra((double)(SoCau) * ((double)20 / (double)100));
            mang[0,1] = kiemtra((double)(SoCau) * ((double)30 / (double)100));
            mang[0,0] = kiemtra((double)SoCau - (double)(mang[0,1] + mang[0,2] + mang[0,3]));
            //trung binh
            mang[1,3] = kiemtra((double)(SoCau) * ((double)25 / (double)100));
            mang[1,2] = kiemtra((double)(SoCau) * ((double)25 / (double)100));
            mang[1,1] = kiemtra((double)(SoCau) * ((double)25 / (double)100));
            mang[1,0] = kiemtra((double)SoCau - (double)(mang[1,1] + mang[1,2] + mang[1,3]));
            //kho
            mang[2,3] = kiemtra((double)(SoCau) * ((double)40 / (double)100));
            mang[2,2] = kiemtra((double)(SoCau) * ((double)30 / (double)100));
            mang[2,1] = kiemtra((double)(SoCau) * ((double)20 / (double)100));
            mang[2,0] = kiemtra((double)SoCau - (double)(mang[2,1] + mang[2,2] + mang[2,3]));

            return mang;
        }
        public int kiemtra(double a)
        {
            int b =(int)a;
            if (a - b >= 0.5)
            {
                return b + 1;
            }
            else
            {
                return b;
            }
           
        }
    }

   
}