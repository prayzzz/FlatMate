using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlatMate.Common.Extensions;
using FlatMate.Module.Home.Models;
using FlatMate.Module.Home.Persistence.Dbo;
using FlatMate.Module.Home.Provider;
using FlatMate.Module.Home.Service;
using FlatMate.Web.Areas.Home.Dto;
using FlatMate.Web.Common.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prayzzz.Common.Mapping;
using prayzzz.Common.Result;

namespace FlatMate.Web.Areas.Home.Controllers
{
    [Route("api/v1/home/dashboard")]
    public class DashboardApiController : ApiController
    {
        private readonly IEnumerable<IDashboardEntryProvider> _entryProvider;
        private readonly IMapper _mapper;
        private readonly IDashboardService _service;

        public DashboardApiController(IHttpContextAccessor context, IEnumerable<IDashboardEntryProvider> entryProvider, IDashboardService service, IMapper mapper)
            : base(context)
        {
            _entryProvider = entryProvider;
            _service = service;
            _mapper = mapper;
        }

        [HttpPost("entry")]
        public Result<DashboardEntryDto> CreateEntry([FromBody] DashboardEntryDto model)
        {
            model.Id = 0;
            model.UserId = CurrentUserId;

            var dbo = _mapper.Map<DashboardEntryDbo>(model);
            var saveResult = _service.Create(dbo);

            return saveResult.WithDataAs(x => _mapper.Map<DashboardEntryDto>(x));
        }

        [HttpGet("entry")]
        public IEnumerable<DashboardEntryDto> GetAll()
        {
            return _service.GetAll()
                           .Where(x => x.UserId == CurrentUserId)
                           .Select(x => _mapper.Map<DashboardEntryDto>(x));
        }

        [HttpGet("entrytype")]
        [Produces(typeof(IEnumerable<DashboardEntryTypeDbo>))]
        public IEnumerable<DashboardEntryTypeDbo> GetEntryTypes()
        {
            return _entryProvider.SelectMany(x => x.GetEntryTypes());
        }

        [HttpGet("entrytype/{id}")]
        [Produces(typeof(IEnumerable<DashboardEntryTypeValueDbo>))]
        public IEnumerable<DashboardEntryTypeValueDbo> GetEntryTypeValues(Guid id)
        {
            return _entryProvider.SelectMany(x => x.GetEntryValues(CurrentUserId, id));
        }
    }
}