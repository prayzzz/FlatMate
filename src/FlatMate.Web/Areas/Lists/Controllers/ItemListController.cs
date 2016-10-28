using System.Threading.Tasks;
using FlatMate.Web.Areas.Lists.Data;
using FlatMate.Web.Common.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlatMate.Web.Areas.Lists.Controllers
{
    [Authorize]
    [Area("Lists")]
    public class ItemListController : MvcController
    {
        private readonly ItemListApiController _itemListApi;

        public ItemListController(ItemListApiController itemListApi)
        {
            _itemListApi = itemListApi;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ItemListCreateVm model)
        {
            var result = await _itemListApi.CreateList(model.ItemList);

            if (!result.IsSuccess)
            {
                model.ErrorMessage = result.ErrorMessage;
                return View(model);
            }

            return RedirectToAction("Edit", result.Data.Id);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = new ItemListEditVm();

            var result = _itemListApi.GetById(id);
            if (!result.IsSuccess)
            {
                model.ErrorMessage = result.ErrorMessage;
                return View(model);
            }

            model.ItemList = result.Data;
            return View(model);
        }

        [HttpGet]
        public IActionResult View(int id)
        {
            var model = new ItemListViewVm();

            var result = _itemListApi.GetById(id);
            if (!result.IsSuccess)
            {
                model.ErrorMessage = result.ErrorMessage;
                return View(model); // TODO NullPointer in View
            }

            model.ItemList = result.Data;
            return View(model);
        }
    }
}