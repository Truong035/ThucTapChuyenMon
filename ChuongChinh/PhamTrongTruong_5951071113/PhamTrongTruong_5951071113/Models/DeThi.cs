namespace PhamTrongTruong_5951071113.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DeThi")]
    public partial class DeThi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DeThi()
        {
            Cau_Hoi = new HashSet<Cau_Hoi>();
            Da_LuaChon = new HashSet<Da_LuaChon>();
            DanhGias = new HashSet<DanhGia>();
        }

        [Key]
        public long Ma_De { get; set; }

        [StringLength(50)]
        public string MaTK { get; set; }

        public int? ThoiGianThi { get; set; }

        public DateTime NgayThi { get; set; }

        public double? DiêmSo { get; set; }

        public bool? TrangThai { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cau_Hoi> Cau_Hoi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Da_LuaChon> Da_LuaChon { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DanhGia> DanhGias { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }
    }
}
