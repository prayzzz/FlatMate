using FlatMate.Web.Areas.Lists.Data;
using FlatMate.Web.Common.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlatMate.Web.Areas.Lists.Controllers
{
    [Authorize]
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
            var model = new HomeViewModel();

            var itemListResult = _itemListApi.GetAllByUser(CurrentUserId);
            if (!itemListResult.IsSuccess)
            {
                model.ErrorResult = itemListResult;
                return View(model);
            }

            model.ItemLists = itemListResult.Data;
            return View(model);
        }
    }
}