namespace PhamTrongTruong_5951071113.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Chuong_Hoc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Chuong_Hoc()
        {
            Bai_Hoc = new HashSet<Bai_Hoc>();
        }

        [Key]
        public int Ma_Chuong { get; set; }

        [StringLength(50)]
        public string Ten_Chuong { get; set; }

        public int? SoBai { get; set; }

        public bool? Xóa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bai_Hoc> Bai_Hoc { get; set; }
    }
}
