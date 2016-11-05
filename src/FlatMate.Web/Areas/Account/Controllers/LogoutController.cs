using System.Threading.Tasks;
using FlatMate.Web.Common.Base;
using Microsoft.AspNetCore.Mvc;

namespace FlatMate.Web.Areas.Account.Controllers
{
    [Area("Account")]
    public class LogoutController : MvcController
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await HttpContext.Authentication.SignOutAsync("FlatMate");
            return LocalRedirectPermanent("/");
        }
    }
}