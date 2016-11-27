using System;
using System.Collections.Generic;
using FlatMate.Module.Home.Models;

namespace FlatMate.Module.Home.Provider
{
    public interface IDashboardEntryProvider
    {
        List<DashboardEntryType> GetEntryTypes();

        List<DashboardEntryValue> GetEntryValues(int currentUserId, Guid id);
    }
}