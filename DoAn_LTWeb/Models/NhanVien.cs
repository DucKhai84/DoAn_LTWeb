using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DoAn_LTWeb.Models
{
    [Table("NhanVien")]
    public class NhanVien
    {
        [Key]
        public int MaNhanVien { get; set; }

        [StringLength(100)]
        public string TenNhanVien { get; set; }

        [StringLength(15)]
        public string SoDienThoai { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [StringLength(12)]
        public string? CMND { get; set; }

        [StringLength(10)]
        public string? GioiTinh { get; set; }

        public DateTime NgaySinh { get; set; } 

        public DateTime NgayVaoLam { get; set; } 

        public int MaNhaTro { get; set; }
        [ForeignKey(nameof(MaNhaTro))]
        public virtual NhaTro? NhaTro { get; set; }

        //public HoaDon? HoaDon { get; set; }
        //public DichVu? DichVu { get; set; }

        public virtual ICollection<HoaDon>? HoaDon { get; set; }

        public string? ImageUrl { get; set; }

        public List<string>? ImageUrls { get; set; }

    }
}
