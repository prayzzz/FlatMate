using FlatMate.Web.Common.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlatMate.Web.Areas.Lists.Controllers
{
    [Authorize]
    [Area("Lists")]
    public class ItemListController : MvcController
    {
        public IActionResult Create()
        {
            return View();
        }
    }
}