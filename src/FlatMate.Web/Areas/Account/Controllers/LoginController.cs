using System.Security.Claims;
using System.Threading.Tasks;
using FlatMate.Module.Account.Services;
using FlatMate.Web.Areas.Account.Data;
using FlatMate.Web.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using prayzzz.Common.Mvc;

namespace FlatMate.Web.Areas.Account.Controllers
{
    [Area("Account")]
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly CookieAuthenticationOptions _cookieOptions;
        private readonly ILogger _logger;

        public LoginController(ILoggerFactory loggerFactory, ILoginService loginService, IOptions<CookieAuthenticationOptions> cookieOptions)
        {
            _loginService = loginService;
            _cookieOptions = cookieOptions.Value;
            _logger = loggerFactory.CreateLogger<LoginController>();
        }

        [HttpGet]
        public IActionResult Index(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidationFilter]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

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
            return RedirectToLocal(returnUrl);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Dashboard");
        }
    }
}