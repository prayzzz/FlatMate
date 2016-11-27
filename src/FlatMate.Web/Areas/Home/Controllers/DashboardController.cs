using System.Collections.Generic;
using System.Linq;
using FlatMate.Module.Home.Models;
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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Edit()
        {
            var model = new DashboardEditModel();
            model.DashboardEntryTypes = _dashboardApi.GetEntryTypes().ToList();
            model.DashboardEntries = _dashboardApi.GetAll().ToList();

            return View(model);
        }

        public IActionResult Create(DashboardEntry model)
        {
            _dashboardApi.CreateEntry(model);

            return RedirectToAction("Edit");
        }
    }

    public class DashboardEditModel : BaseViewModel
    {
        public List<DashboardEntryType> DashboardEntryTypes { get; set; } = new List<DashboardEntryType>();

        public List<DashboardEntry> DashboardEntries { get; set; } = new List<DashboardEntry>();
    }
}
