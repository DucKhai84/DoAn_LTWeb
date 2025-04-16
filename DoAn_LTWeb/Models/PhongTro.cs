using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn_LTWeb.Models
{
    [Table("PhongTro")]
    public class PhongTro
    {

        [Key]
        public int MaPhongTro { get; set; }
        public int MaNhaTro { get; set; }
        [ForeignKey(nameof(MaNhaTro))]
        public NhaTro? NhaTro { get; set; }

        //public ChiTietPhongTro? chiTietPhongTro { get; set; } => Khoi tao lazy loading
        public string TenPhongTro { get; set; }
        public string TrangThai { get; set; }  
        public int? DienTich { get; set; }
        public string? Mota { get; set; }   

        public virtual ICollection<ChiTietThuePhong> chiTietThuePhong { get; set; } = new List<ChiTietThuePhong>();

        public virtual ICollection<ChiTietPhongTro> chiTietPhongTro { get; set; } = new List<ChiTietPhongTro>();

        public string? ImageUrl { get; set; }
        public List<string>? ImageUrls { get; set; }
    }
}
