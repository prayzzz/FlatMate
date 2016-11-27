using System;
using System.ComponentModel.DataAnnotations;
using prayzzz.Common.Dbo;
using prayzzz.Common.Mapping;

namespace FlatMate.Module.Home.Models
{
    public class DashboardEntry
    {
        public int Id { get; set; }

        public Guid TypeId { get; set; }

        public string Value { get; set; }

        [Editable(false)]
        public int UserId { get; set; }
    }

    public class DashboardEntryDbo : OwnedDbo
    {
        public Guid TypeId { get; set; }

        public string Value { get; set; }
    }

    public class DashboardEntryMapper : IDboMapper
    {
        public void Configure(IMapperConfiguration mapper)
        {
            mapper.Configure<DashboardEntryDbo, DashboardEntry>(MapToModel);
            mapper.Configure<DashboardEntry, DashboardEntryDbo>(MapToDbo);
        }

        private static DashboardEntry MapToModel(DashboardEntryDbo dbo, MappingContext ctx)
        {
            var model = new DashboardEntry
            {
                TypeId = dbo.TypeId,
                Value = dbo.Value,
                UserId = dbo.UserId
            };

            return model;
        }

        private static DashboardEntryDbo MapToDbo(DashboardEntry model, DashboardEntryDbo dbo, MappingContext ctx)
        {
            dbo.TypeId = model.TypeId;
            dbo.Value = model.Value;
            dbo.UserId = model.UserId;

            return dbo;
        }
    }
}