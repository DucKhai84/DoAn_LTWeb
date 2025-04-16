using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn_LTWeb.Models
{
    [Table("HoaDon")]
    public class HoaDon
    {
        [Key]
        public int MaHoaDon { get; set; }
        public int? MaKhachHang { get; set; }

        [ForeignKey(nameof(MaKhachHang))]  // Dùng nameof để tránh lỗi typo
        public KhachHang? KhachHang { get; set; }
        public int? MaNhanVien { get; set; }

        [ForeignKey(nameof(MaNhanVien))]
        public NhanVien? NhanVien { get; set; }
        public DateTime NgayLap { get; set; } = DateTime.Now;

        public float TienPhong { get; set; }

        public float SoDien { get; set; }
        public float SoNuoc { get; set; }
    }
}