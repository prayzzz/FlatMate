using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FlatMate.Common.Attributes;
using FlatMate.Module.Home.Models;
using FlatMate.Module.Home.Persistence.Dbo;
using FlatMate.Module.Home.Provider;
using prayzzz.Common.Mapping;

namespace FlatMate.Web.Areas.Home.Dto
{
    public class DashboardEntryDto
    {
        public DashboardEntryTypeDbo EntryType { get; set; }

        public Guid EntryTypeId { get; set; }
        public int Id { get; set; }

        [Editable(false)]
        public int UserId { get; set; }

        public string Value { get; set; }
    }

    [Inject(DependencyLifetime.Scoped)]
    public class DashboardEntryMapper : IDboMapper
    {
        private readonly IEnumerable<IDashboardEntryProvider> _providers;

        public DashboardEntryMapper(IEnumerable<IDashboardEntryProvider> providers)
        {
            _providers = providers;
        }

        public void Configure(IMapperConfiguration mapper)
        {
            mapper.Configure<DashboardEntryDbo, DashboardEntryDto>(MapToModel);
            mapper.Configure<DashboardEntryDto, DashboardEntryDbo>(MapToDbo);
        }

        private static DashboardEntryDbo MapToDbo(DashboardEntryDto model, DashboardEntryDbo dbo, MappingContext ctx)
        {
            dbo.TypeId = model.EntryTypeId;
            dbo.Value = model.Value;
            dbo.UserId = model.UserId;

            return dbo;
        }

        private DashboardEntryDto MapToModel(DashboardEntryDbo dbo, MappingContext ctx)
        {
            var model = new DashboardEntryDto
            {
                EntryTypeId = dbo.TypeId,
                EntryType = _providers.SelectMany(x => x.GetEntryTypes()).FirstOrDefault(x => x.Id == dbo.TypeId),
                Value = dbo.Value,
                UserId = dbo.UserId
            };

            return model;
        }
    }
}