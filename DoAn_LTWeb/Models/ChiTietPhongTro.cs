using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn_LTWeb.Models
{
    [Table("ChiTietPhongTro")]
    public class ChiTietPhongTro
    {
        [Key]
        public int MaChiTietPhongTro { get; set; }
        public int MaNoiThat { get; set; }
        [ForeignKey(nameof(MaNoiThat))]
        public NoiThat? NoiThat { get; set; }
        public int MaPhongTro { get; set; }
        [ForeignKey(nameof(MaPhongTro))]
        public virtual PhongTro? PhongTro { get; set; }
        public float GiaPhong { get; set; }
    }
}
