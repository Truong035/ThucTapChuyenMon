namespace PhamTrongTruong_5951071113.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DanhGia")]
    public partial class DanhGia
    {
        [Key]
        public long MaCT { get; set; }

        public int? Ma_Bai { get; set; }

        public long? Ma_De { get; set; }

        public double Diem { get; set; }

        [StringLength(200)]
        public string DanhDia { get; set; }

        public virtual Bai_Hoc Bai_Hoc { get; set; }

        public virtual DeThi DeThi { get; set; }
    }
}
