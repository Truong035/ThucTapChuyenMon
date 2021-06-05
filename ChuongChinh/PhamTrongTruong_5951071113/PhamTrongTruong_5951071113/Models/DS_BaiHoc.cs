namespace PhamTrongTruong_5951071113.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DS_BaiHoc
    {
        [StringLength(50)]
        public string Ma_TK { get; set; }

        public int? Ma_Bai { get; set; }

        [Key]
        public int Ma_CT { get; set; }

        [StringLength(500)]
        public string ListCauHoi { get; set; }

        public int? SoCauDung { get; set; }

        public int? SoCauSai { get; set; }

        public virtual Bai_Hoc Bai_Hoc { get; set; }
    }
}
