namespace PhamTrongTruong_5951071113.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Da_LuaChon
    {
        [StringLength(20)]
        public string MaTk { get; set; }

        [StringLength(10)]
        public string Ma_De { get; set; }

        public long? Ma_Dan { get; set; }

        [Key]
        public long Ma_CT { get; set; }

        public virtual D_An D_An { get; set; }

        public virtual DeThi DeThi { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }
    }
}
