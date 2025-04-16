using DoAn_LTWeb.Models;

namespace DoAn_LTWeb.ViewModels
{
    public class PhongTroViewModel
    {
        public IEnumerable<PhongTro> PhongTro { get; set; }
        public IEnumerable<ChiTietPhongTro> ChiTietPhongTro { get; set; }
    }
}
