using Microsoft.AspNetCore.Identity;

namespace DoAn_LTWeb.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public KhachHang KhachHang { get; set; }
    }
}
