using System.Threading.Tasks;
using FlatMate.Web.Areas.Lists.Data;
using FlatMate.Web.Common.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlatMate.Web.Areas.Lists.Controllers
{
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

            return RedirectToAction("Edit", new { id = result.Data.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromForm]ItemListEditVm model)
        {
            var result = await _itemListApi.UpdateItemList(id, model.ItemList);

            if (!result.IsSuccess)
            {
                model.ErrorMessage = result.ErrorMessage;
                return View(model);
            }

            model.ItemList = result.Data;
            return View(model);
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
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _itemListApi.Delete(id);

            if (!result.IsSuccess)
            {
                return View("Error", new EmptyViewModel { ErrorResult = result });
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult View(int id)
        {
            var model = new ItemListViewVm();

            var result = _itemListApi.GetById(id);
            if (!result.IsSuccess)
            {
                return View("Error", new EmptyViewModel { ErrorResult = result });
            }

            model.ItemList = result.Data;
            return View(model);
        }
    }
}