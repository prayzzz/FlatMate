using System.Security.Claims;
using System.Threading.Tasks;
using FlatMate.Module.Account.Services;
using FlatMate.Web.Areas.Account.Data;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using prayzzz.Common.Mvc;

namespace FlatMate.Web.Areas.Account.Controllers
{
    [Area("Account")]
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidationFilter]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel model)
        {

            var result = await _loginService.LoginAllowed(model.User);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return View(model);
            }

            var identity = new ClaimsIdentity("Basic");
            identity.AddClaim(new Claim(ClaimTypes.Name, model.User.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Sid, result.Data.ToString()));

            var principal = new ClaimsPrincipal();
            principal.AddIdentity(identity);

            await HttpContext.Authentication.SignInAsync("FlatMate", principal, new AuthenticationProperties { IsPersistent = true });

            return LocalRedirectPermanent("/");
        }
    }
}