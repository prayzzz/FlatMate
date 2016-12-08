using FlatMate.Web.Common.Base;
using Microsoft.AspNetCore.Mvc;

namespace FlatMate.Web.Controllers
{
    public class ErrorController : MvcController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}