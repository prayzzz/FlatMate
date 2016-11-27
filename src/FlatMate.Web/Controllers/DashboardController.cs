using System.Collections.Generic;
using System.Linq;
using FlatMate.Common.Provider;
using FlatMate.Web.Common.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlatMate.Web.Controllers
{
    [Authorize]
    public class DashboardController : MvcController
    {
        private readonly IEnumerable<IDashboardEntryProvider> _entryProvider;

        public DashboardController(IEnumerable<IDashboardEntryProvider> entryProvider)
        {
            _entryProvider = entryProvider;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Edit()
        {
            var model = new DashboardEditModel();
            model.DashboardEntryTypes = _entryProvider.SelectMany(x => x.GetEntryTypes()).ToList();

            return View(model);
        }
    }

    public class DashboardEditModel : BaseViewModel
    {
        public List<DashboardEntryType> DashboardEntryTypes { get; set; } = new List<DashboardEntryType>();
    }
}
