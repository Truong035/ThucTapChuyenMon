namespace PhamTrongTruong_5951071113.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TaiKhoan")]
    public partial class TaiKhoan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TaiKhoan()
        {
            DeThis = new HashSet<DeThi>();
            LienHes = new HashSet<LienHe>();
        }

        [Key]
     
        [Required(ErrorMessage = "Bạn chưa nhập tài khoản")]
        [StringLength(50)]
        public string MaTK { get; set; }

        
        [StringLength(50)]
        
        public string MatKhau { get; set; }

        [StringLength(50)]
        public string Ten { get; set; }

        public bool? Quyen { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayTao { get; set; }

        public bool TrangThai { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeThi> DeThis { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LienHe> LienHes { get; set; }
    }
}
