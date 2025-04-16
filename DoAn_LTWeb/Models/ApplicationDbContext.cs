using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DoAn_LTWeb.Models;

namespace DoAn_LTWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<DoAn_LTWeb.Models.DieuKhoan> DieuKhoan { get; set; }
        public DbSet<DoAn_LTWeb.Models.ChiTietThuePhong> ChiTietThuePhong { get; set; }
        public DbSet<DoAn_LTWeb.Models.ChiTietPhongTro> ChiTietPhongTro { get; set; }
        public DbSet<DoAn_LTWeb.Models.HopDong> HopDong { get; set; }
        public DbSet<DoAn_LTWeb.Models.KhachHang> KhachHang { get; set; }
        public DbSet<DoAn_LTWeb.Models.PhongTro> PhongTro { get; set; }
        public DbSet<DoAn_LTWeb.Models.NhaTro> NhaTro { get; set; }
        public DbSet<DoAn_LTWeb.Models.NhanVien> NhanVien { get; set; }
        public DbSet<DoAn_LTWeb.Models.NhaXe> NhaXe { get; set; }
        public DbSet<DoAn_LTWeb.Models.NoiThat> NoiThat { get; set; }
        public DbSet<DoAn_LTWeb.Models.HoaDon> HoaDon { get; set; }
        public DbSet<DoAn_LTWeb.Models.Xe> Xe { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<HoaDon>().ToTable("HoaDon");
            modelBuilder.Entity<DieuKhoan>().ToTable("DieuKhoan");

            modelBuilder.Entity<NhaXe>()
            .HasOne(n => n.NhaTro)
            .WithMany(n => n.NhaXe)
            .HasForeignKey(n => n.MaNhaTro);

            modelBuilder.Entity<HoaDon>()
                .HasOne(hd => hd.KhachHang)
                .WithMany(kh => kh.HoaDon)  // Thêm danh sách hóa đơn vào KhachHang
                .HasForeignKey(hd => hd.MaKhachHang)
                .OnDelete(DeleteBehavior.Restrict);  // Không cho xóa khách hàng nếu có hóa đơn

            modelBuilder.Entity<HoaDon>()
                .HasOne(hd => hd.NhanVien)
                .WithMany(nv => nv.HoaDon)
                .HasForeignKey(hd => hd.MaNhanVien)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<PhongTro>()
                .HasOne(p => p.NhaTro)
                .WithMany(h => h.PhongTro)
                .HasForeignKey(p => p.MaNhaTro);

            modelBuilder.Entity<NhanVien>()
                .HasOne(p => p.NhaTro)
                .WithMany(h => h.NhanVien)
                .HasForeignKey(p => p.MaNhaTro);

            modelBuilder.Entity<Xe>()
                .Property(x => x.MaXe)
                .ValueGeneratedOnAdd();  

            // Thiết lập quan hệ giữa Xe và NhaXe
            modelBuilder.Entity<Xe>()
                .HasOne(x => x.NhaXe)
                .WithMany(n => n.Xe)
                .HasForeignKey(x => x.MaNhaXe); // Sửa MaXe thành MaNhaXe (chính xác)

            // Thiết lập quan hệ giữa Xe và KhachHang
            modelBuilder.Entity<Xe>()
                .HasOne(x => x.KhachHang)
                .WithOne(kh => kh.Xe)
                .HasForeignKey<Xe>(x => x.MaKhachHang);

            modelBuilder.Entity<KhachHang>()
              .HasOne(kh => kh.User)
              .WithMany()
              .HasForeignKey(kh => kh.UserId)
              .IsRequired(false) // Cho phép NULL
              .OnDelete(DeleteBehavior.SetNull); // Xóa User -> Giữ lại KhachHang

            modelBuilder.Entity<HopDong>()
                .HasOne(h => h.ChiTietThuePhong)
                .WithOne(c => c.HopDong)
                .HasForeignKey<ChiTietThuePhong>(c => c.MaHopDong)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
