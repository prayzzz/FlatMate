using System.Collections.Generic;
using FlatMate.Module.Home.Models;
using FlatMate.Web.Areas.Home.Dto;
using FlatMate.Web.Common.Base;

namespace FlatMate.Web.Areas.Home.Controllers
{
    public class DashboardEditVm : BaseViewModel
    {
        public List<DashboardEntryDto> DashboardEntries { get; set; } = new List<DashboardEntryDto>();
        public List<DashboardEntryTypeDbo> DashboardEntryTypes { get; set; } = new List<DashboardEntryTypeDbo>();
    }
}