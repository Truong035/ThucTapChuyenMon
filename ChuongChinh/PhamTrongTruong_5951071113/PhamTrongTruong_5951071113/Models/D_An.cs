namespace PhamTrongTruong_5951071113.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class D_An
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public D_An()
        {
            Da_LuaChon = new HashSet<Da_LuaChon>();
        }

        [Key]
        public long Ma_Dan { get; set; }

        [StringLength(500)]
        public string NoiDung { get; set; }

        [StringLength(100)]
        public string HinhAnh { get; set; }

        public bool? TrangThai { get; set; }

        public long Ma_CH { get; set; }

        public bool? Xoa { get; set; }

        public virtual KhoCauHoi KhoCauHoi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Da_LuaChon> Da_LuaChon { get; set; }
    }
}
