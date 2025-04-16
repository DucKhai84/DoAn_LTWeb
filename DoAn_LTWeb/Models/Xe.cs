using DoAn_LTWeb.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn_LTWeb.Models
{
    public class Xe
    {
        [Key]
        public int MaXe { get; set; }
        public string BienSoXe { get; set; }
        public string CMND { get; set; }
        public int MaNhaXe { get; set; }
        [ForeignKey(nameof(MaNhaXe))]
        public NhaXe? NhaXe { get; set; }
        public int MaKhachHang { get; set; }
        [ForeignKey(nameof(MaKhachHang))]
        public KhachHang? KhachHang { get; set; }
    }
}
