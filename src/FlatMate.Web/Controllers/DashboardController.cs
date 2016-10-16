using Microsoft.AspNetCore.Mvc;

namespace FlatMate.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
