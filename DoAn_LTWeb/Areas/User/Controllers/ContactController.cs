using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAn_LTWeb.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    public class ContactController : Controller
    {
        public IActionResult Contact()
        {
            return View();
        }
    }
}
