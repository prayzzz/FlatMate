using System;

namespace FlatMate.Module.Home.Models
{
    public class DashboardEntryTypeDbo
    {
        public DashboardEntryTypeDbo(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }

        public string Module { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string ViewComponentName { get; set; } = string.Empty;
    }
}