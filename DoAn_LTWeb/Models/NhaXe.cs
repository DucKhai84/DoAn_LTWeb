using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn_LTWeb.Models
{
    [Table("NhaXe")]

    public class NhaXe
    {
        [Key]
        public int MaNhaXe { get; set; }
        public int MaNhaTro { get; set; }
        [ForeignKey(nameof(MaNhaTro))]
        public NhaTro? NhaTro { get; set; }
        public int SucChua { get; set; }
        public string? ImageUrl { get; set; }
        public List<string>? ImageUrls { get; set; }
        public ICollection<Xe>? Xe { get; set; }
    }
}
