using System;
using prayzzz.Common.Dbo;

namespace FlatMate.Module.Home.Models
{
    public class DashboardEntryDbo : OwnedDbo
    {
        public Guid TypeId { get; set; } = Guid.Empty;

        public string Value { get; set; } = string.Empty;
    }
}