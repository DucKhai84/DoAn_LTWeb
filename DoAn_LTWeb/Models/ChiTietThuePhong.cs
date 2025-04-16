using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.DependencyInjection;

namespace DoAn_LTWeb.Models
{
    [Table("ChiTietThuePhong")]
    public class ChiTietThuePhong
    {
        [ActivatorUtilitiesConstructor]
        public ChiTietThuePhong() { }

        public ChiTietThuePhong(int maHopDong, int maPhongTro, int maKhachHang, int soLuong,string tenKhachHang)
        {
            this.MaHopDong = maHopDong;
            this.MaPhongTro = maPhongTro;
            this.MaKhachHang = maKhachHang;
            this.SoLuong = soLuong;
        }

        [Key]
        public int MaChiTietThuePhong { get; set; }

        public int MaHopDong { get; set; }
        public HopDong HopDong { get; set; } = null!;

        // Quan hệ với PhongTro
        public int MaPhongTro { get; set; }
        [ForeignKey(nameof(MaPhongTro))]
        public PhongTro? PhongTro { get; set; }

        // Quan hệ với KhachHang
        public int MaKhachHang { get; set; }
        [ForeignKey(nameof(MaKhachHang))]
        public KhachHang? KhachHang { get; set; }

        public int SoLuong { get; set; }
    }
}
