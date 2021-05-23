namespace PhamTrongTruong_5951071113.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KhoCauHoi")]
    public partial class KhoCauHoi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KhoCauHoi()
        {
            Cau_Hoi = new HashSet<Cau_Hoi>();
            D_An = new HashSet<D_An>();
        }

        [Key]
        public long Ma_CH { get; set; }

        public string NoiDung { get; set; }

        [StringLength(100)]
        public string HinhAnh { get; set; }

        public int? MucDọ { get; set; }

        public bool? Xoa { get; set; }

        public int? Ma_Bai { get; set; }

        public virtual Bai_Hoc Bai_Hoc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cau_Hoi> Cau_Hoi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<D_An> D_An { get; set; }
    }
}
