using System;
using System.Collections.Generic;
using FlatMate.Module.Home.Models;

namespace FlatMate.Module.Home.Provider
{
    public interface IDashboardEntryProvider
    {
        IEnumerable<DashboardEntryTypeDbo> GetEntryTypes();

        IEnumerable<DashboardEntryTypeValueDbo> GetEntryValues(int currentUserId, Guid id);
    }
}