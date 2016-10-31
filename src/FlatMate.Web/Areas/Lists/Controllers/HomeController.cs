using FlatMate.Web.Areas.Lists.Data;
using FlatMate.Web.Common.Base;
using Microsoft.AspNetCore.Authorization;
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
            var model = new HomeIndexVm();

            var itemListResult = _itemListApi.GetAll(userId: CurrentUserId);
            var publicLists = _itemListApi.GetAll(isPublic: true, limit: 10);

            model.OwnItemLists = itemListResult;
            model.PublicItemLists = publicLists;

            return View(model);
        }
    }
}