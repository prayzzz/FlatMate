using System;

namespace FlatMate.Module.Home.Models
{
    public class DashboardEntryTypeValueDbo
    {
        public string Name { get; set; } = string.Empty;
        public DashboardEntryTypeDbo Type { get; set; } = new DashboardEntryTypeDbo(Guid.Empty);

        public string Value { get; set; } = string.Empty;
    }
}