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
        public virtual DbSet<DeThi> DeThis { get; set; }
        public virtual DbSet<DS_BaiHoc> DS_BaiHoc { get; set; }
        public virtual DbSet<KhoCauHoi> KhoCauHois { get; set; }
        public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cau_Hoi>()
                .Property(e => e.MaDe)
                .IsFixedLength();

            modelBuilder.Entity<D_An>()
                .Property(e => e.HinhAnh)
                .IsUnicode(false);

            modelBuilder.Entity<Da_LuaChon>()
                .Property(e => e.MaTk)
                .IsFixedLength();

            modelBuilder.Entity<Da_LuaChon>()
                .Property(e => e.Ma_De)
                .IsFixedLength();

            modelBuilder.Entity<DeThi>()
                .Property(e => e.Ma_De)
                .IsFixedLength();

            modelBuilder.Entity<DeThi>()
                .Property(e => e.MaTK)
                .IsFixedLength();

            modelBuilder.Entity<DeThi>()
                .HasMany(e => e.Cau_Hoi)
                .WithOptional(e => e.DeThi)
                .HasForeignKey(e => e.MaDe);

            modelBuilder.Entity<DS_BaiHoc>()
                .Property(e => e.Ma_TK)
                .IsFixedLength();

            modelBuilder.Entity<DS_BaiHoc>()
                .Property(e => e.ListCauHoi)
                .IsFixedLength();

            modelBuilder.Entity<KhoCauHoi>()
                .HasMany(e => e.D_An)
                .WithRequired(e => e.KhoCauHoi)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.MaTK)
                .IsFixedLength();

            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.MatKhau)
                .IsFixedLength();

            modelBuilder.Entity<TaiKhoan>()
                .HasMany(e => e.DS_BaiHoc)
                .WithOptional(e => e.TaiKhoan)
                .HasForeignKey(e => e.Ma_TK);
        }
    }
}
