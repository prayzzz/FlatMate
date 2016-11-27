using System;
using System.Collections.Generic;
using System.Linq;
using FlatMate.Common.Provider;
using FlatMate.Web.Common.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlatMate.Web.Controllers
{
    [Route("api/v1/dashboard")]
    public class DashboardApiController : ApiController
    {
        private readonly IEnumerable<IDashboardEntryProvider> _entryProvider;

        public DashboardApiController(IHttpContextAccessor context, IEnumerable<IDashboardEntryProvider> entryProvider)
            : base(context)
        {
            _entryProvider = entryProvider;
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
    }
}
