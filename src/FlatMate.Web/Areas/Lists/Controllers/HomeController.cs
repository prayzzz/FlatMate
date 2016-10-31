using System.Linq;
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
            model.OwnItemLists = _itemListApi.GetAll(userId: CurrentUserId).ToList();
            model.PublicItemLists = _itemListApi.GetAll(isPublic: true).Where(x => x.UserId != CurrentUserId).Take(10).ToList();

            return View(model);
        }
    }
}