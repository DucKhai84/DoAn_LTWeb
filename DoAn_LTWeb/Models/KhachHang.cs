using DoAn_LTWeb.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn_LTWeb.Models
{
    [Table("KhachHang")]
    public class KhachHang
    {
        [Key]
        public int MaKhachHang { get; set; }

        [Required(ErrorMessage = "Tên không được để trống")]
        public string TenKhachHang { get; set; }
        public string SoDienThoai { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }
        public string? CMND { get; set; }
        public string? GioiTinh { get; set; }
        public DateTime? NgaySinh { get; set; }
        public DateTime NgayDangKy { get; set; } = DateTime.Now;
        public Xe? Xe { get; set; }

        public ICollection<ChiTietThuePhong>? ChiTietThuePhong { get; set; }
        public ICollection<HoaDon>? HoaDon { get; set; }

        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual IdentityUser? User { get; set; } // Thêm dấu '?' để nullable

    }
}
