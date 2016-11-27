using System;

namespace FlatMate.Module.Home.Models
{
    public class DashboardEntryType
    {
        public DashboardEntryType(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }

        public string Name { get; set; }

        public string ViewComponentName { get; set; }

        public string Module { get; set; }
    }
}