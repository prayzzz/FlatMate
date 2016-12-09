using System.Linq;
using System.Threading.Tasks;
using FlatMate.Web.Areas.Home.Dto;
using FlatMate.Web.Common.Base;
using Microsoft.AspNetCore.Mvc;

namespace FlatMate.Web.Areas.Home.Controllers
{
    [Area("Home")]
    public class DashboardController : MvcController
    {
        private readonly DashboardApiController _dashboardApi;

        public DashboardController(DashboardApiController dashboardApi)
        {
            _dashboardApi = dashboardApi;
        }

        public IActionResult Create(DashboardEntryDto model)
        {
            var result = _dashboardApi.CreateEntry(model);

            if (!result.IsSuccess)
            {
                return View("Error", new EmptyViewModel {ErrorResult = result});
            }

            return RedirectToAction("Edit");
        }

        public IActionResult Edit()
        {
            var model = new DashboardEditVm();
            model.DashboardEntryTypes = _dashboardApi.GetEntryTypes().ToList();
            model.DashboardEntries = _dashboardApi.GetAll().ToList();

            return View(model);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}