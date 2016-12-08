using FlatMate.Web.Areas.Lists.ViewModels;
using FlatMate.Web.Common.Base;
using Microsoft.AspNetCore.Mvc;

namespace FlatMate.Web.Areas.Lists.Controllers
{
    [Area("Lists")]
    public class HomeController : MvcController
    {
        private readonly ItemListApiController _itemListApi;

        public HomeController(ItemListApiController itemListApi)
        {
            _itemListApi = itemListApi;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new HomeIndexVm();

            return View(model);
        }
    }
}