using FlatMate.Module.Lists.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlatMate.Web.Areas.Lists.Controllers
{
    [Authorize]
    public class OverviewController : Controller
    {
        private readonly IListService _listService;

        public OverviewController(IListService listService)
        {
            _listService = listService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}