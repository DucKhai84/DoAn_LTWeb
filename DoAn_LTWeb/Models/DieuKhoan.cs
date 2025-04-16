using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn_LTWeb.Models
{
    [Table("DieuKhoan")]
    public class DieuKhoan
    {
        [Key]
        public int MaDieuKhoan { get; set; }
        public string NoiDung { get; set; }
    }
}
