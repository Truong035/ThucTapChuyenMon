namespace PhamTrongTruong_5951071113.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Bai_Hoc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Bai_Hoc()
        {
            DanhGias = new HashSet<DanhGia>();
            DS_BaiHoc = new HashSet<DS_BaiHoc>();
            KhoCauHois = new HashSet<KhoCauHoi>();
        }

        [Key]
        public int Ma_Bai { get; set; }

        [StringLength(200)]
        public string Tên_Bai { get; set; }

        public int? Ma_Chuong { get; set; }

        public bool? Xoa { get; set; }

        public virtual Chuong_Hoc Chuong_Hoc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DanhGia> DanhGias { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DS_BaiHoc> DS_BaiHoc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KhoCauHoi> KhoCauHois { get; set; }
    }
}
