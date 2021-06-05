namespace PhamTrongTruong_5951071113.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LienHe")]
    public partial class LienHe
    {
        [StringLength(50)]
        public string Ma_TK { get; set; }

        [StringLength(500)]
        public string NoiDung { get; set; }

        [Key]
        public long STT { get; set; }

        public bool? TrangThai { get; set; }

        [StringLength(50)]
        public string HoTen { get; set; }

        [StringLength(50)]
        public string NguoiNhan { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }
    }
}
