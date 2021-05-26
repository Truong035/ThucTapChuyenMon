namespace PhamTrongTruong_5951071113.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Cau_Hoi
    {
        public long ID { get; set; }

        public long? MaDe { get; set; }

        public long? Ma_CH { get; set; }

        public virtual DeThi DeThi { get; set; }

        public virtual KhoCauHoi KhoCauHoi { get; set; }
    }
}
