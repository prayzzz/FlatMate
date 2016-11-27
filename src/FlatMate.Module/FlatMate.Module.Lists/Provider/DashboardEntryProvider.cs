using System;
using System.Collections.Generic;
using System.Linq;
using FlatMate.Module.Home.Models;
using FlatMate.Module.Home.Provider;
using FlatMate.Module.Lists.Models;
using FlatMate.Module.Lists.Services;
using Microsoft.EntityFrameworkCore.Internal;

namespace FlatMate.Module.Lists.Provider
{
    public class DashboardEntryProvider : IDashboardEntryProvider
    {
        private readonly IItemListService _listService;
        private readonly List<DashboardEntryType> _dashboardEntries;

        public DashboardEntryProvider(IItemListService listService)
        {
            _listService = listService;
            _dashboardEntries = new List<DashboardEntryType>
            {
                new DashboardEntryType(Guid.Parse("{268C0364-6C72-4902-8F0F-9B0EA5651A64}")) {Name = "ItemList", ViewComponentName = "ItemListTile", Module = "Lists"},
                new DashboardEntryType(Guid.Parse("{268C0364-6C72-4902-8F0F-9B0EA5651A65}")) {Name = "ItemList2", ViewComponentName = "ItemListTile2", Module = "Lists2"}
            };
        }

        public List<DashboardEntryType> GetEntryTypes()
        {
            return _dashboardEntries;
        }

        public List<DashboardEntryValue> GetEntryValues(int currentUserId, Guid id)
        {
            var userLists = _listService.GetAll(new ItemListQuery { UserId = currentUserId });
            var publicLists = _listService.GetAll(new ItemListQuery { IsPublic = true });

            return userLists.Concat(publicLists)
                .Distinct((x, y) => x.Id == y.Id)
                .OrderByDescending(x => x.LastModified)
                .Select(x => new DashboardEntryValue { Name = x.Name, Value = x.Id.ToString() })
                .ToList();
        }
    }
}