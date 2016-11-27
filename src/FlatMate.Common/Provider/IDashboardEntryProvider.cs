using System;
using System.Collections.Generic;

namespace FlatMate.Common.Provider
{
    public interface IDashboardEntryProvider
    {
        List<DashboardEntryType> GetEntryTypes();

        List<DashboardEntryValue> GetEntryValues(int currentUserId, Guid id);
    }

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

    public class DashboardEntryValue
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}