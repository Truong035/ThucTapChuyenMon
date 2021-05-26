using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;


namespace PhamTrongTruong_5951071113.Models.Dao
{

    public partial class Danh_Gia
    {
        
        public long? MaChuong { get; set; }
        public double SoCauDung { get; set; }
        public double TongCau { get; set; }
        public string[] NhanXet { get; set; }
        public int DanhGia { get; set; }

    }
}
