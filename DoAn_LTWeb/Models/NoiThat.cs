using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn_LTWeb.Models
{
    [Table("NoiThat")]

    public class NoiThat
    {
        [Key]
        public int MaNoiThat { get; set; }
        public string TenNoiThat { get; set; }
        public string LoaiNoiThat { get; set; }
        public float? Gia { get; set; }
        public string? MoTa { get; set; }
        public string TinhTrang { get; set; }
        public DateTime NgayMua { get; set; }
        public virtual ICollection<ChiTietPhongTro>? chiTietPhongTro { get; set; }

        public string? ImageUrl { get; set; }

        public List<string>? ImageUrls { get; set; }
    }
}
