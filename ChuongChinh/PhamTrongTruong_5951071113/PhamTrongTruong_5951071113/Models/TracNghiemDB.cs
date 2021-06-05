using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace PhamTrongTruong_5951071113.Models
{
    public partial class TracNghiemDB : DbContext
    {
        public TracNghiemDB()
            : base("name=TracNghiemDB")
        {
        }

        public virtual DbSet<Bai_Hoc> Bai_Hoc { get; set; }
        public virtual DbSet<Cau_Hoi> Cau_Hoi { get; set; }
        public virtual DbSet<Chuong_Hoc> Chuong_Hoc { get; set; }
        public virtual DbSet<D_An> D_An { get; set; }
        public virtual DbSet<Da_LuaChon> Da_LuaChon { get; set; }
        public virtual DbSet<DanhGia> DanhGias { get; set; }
        public virtual DbSet<DeThi> DeThis { get; set; }
        public virtual DbSet<DS_BaiHoc> DS_BaiHoc { get; set; }
        public virtual DbSet<KhoCauHoi> KhoCauHois { get; set; }
        public virtual DbSet<LienHe> LienHes { get; set; }
        public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<D_An>()
                .Property(e => e.HinhAnh)
                .IsUnicode(false);

            modelBuilder.Entity<DanhGia>()
                .Property(e => e.DanhDia)
                .IsUnicode(false);

            modelBuilder.Entity<DeThi>()
                .HasMany(e => e.Cau_Hoi)
                .WithOptional(e => e.DeThi)
                .HasForeignKey(e => e.MaDe);

            modelBuilder.Entity<DS_BaiHoc>()
                .Property(e => e.ListCauHoi)
                .IsFixedLength();

            modelBuilder.Entity<KhoCauHoi>()
                .HasMany(e => e.D_An)
                .WithRequired(e => e.KhoCauHoi)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.MatKhau)
                .IsUnicode(false);

            modelBuilder.Entity<TaiKhoan>()
                .HasMany(e => e.LienHes)
                .WithOptional(e => e.TaiKhoan)
                .HasForeignKey(e => e.Ma_TK);
        }
    }
}
