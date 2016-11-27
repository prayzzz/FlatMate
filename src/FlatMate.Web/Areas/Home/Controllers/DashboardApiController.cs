using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlatMate.Module.Home.Models;
using FlatMate.Module.Home.Provider;
using FlatMate.Module.Home.Service;
using FlatMate.Web.Common.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prayzzz.Common.Result;

namespace FlatMate.Web.Areas.Home.Controllers
{
    [Route("api/v1/home/dashboard")]
    public class DashboardApiController : ApiController
    {
        private readonly IEnumerable<IDashboardEntryProvider> _entryProvider;
        private readonly IDashboardService _service;

        public DashboardApiController(IHttpContextAccessor context, IEnumerable<IDashboardEntryProvider> entryProvider, IDashboardService service)
            : base(context)
        {
            _entryProvider = entryProvider;
            _service = service;
        }

        [HttpGet("entrytype")]
        [Produces(typeof(IEnumerable<DashboardEntryType>))]
        public IEnumerable<DashboardEntryType> GetEntryTypes()
        {
            return _entryProvider.SelectMany(x => x.GetEntryTypes());
        }

        [HttpGet("entrytype/{id}")]
        [Produces(typeof(IEnumerable<DashboardEntryValue>))]
        public IEnumerable<DashboardEntryValue> GetEntryTypeValues(Guid id)
        {
            return _entryProvider.SelectMany(x => x.GetEntryValues(CurrentUserId, id));
        }

        [HttpPost("entry")]
        public Task<Result<DashboardEntry>> CreateEntry([FromBody] DashboardEntry model)
        {
            model.Id = 0;
            model.UserId = CurrentUserId;

            return _service.CreateAsync(model);
        }

        [HttpGet("entry")]
        public IEnumerable<DashboardEntry> GetAll()
        {
            return _service.GetAll().Where(x => x.UserId == CurrentUserId).ToList();
        }
    }
}
