using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn_LTWeb.Models
{
    [Table("NhaTro")]
    public class NhaTro
    {
        [Key]
        public int MaNhaTro { get; set; }

        [StringLength(100)]
        public string TenNhaTro { get; set; }

        [StringLength(255)]
        public string DiaChi { get; set; }
        public ICollection<NhaXe>? NhaXe { get; set; }
        public ICollection<PhongTro>? PhongTro { get; set; }
        public ICollection<NhanVien>? NhanVien { get; set; }

        public string? ImageUrl { get; set; }

        public List<string>? ImageUrls { get; set; }
    }
}
