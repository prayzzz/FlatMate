using System;
using System.Collections.Generic;
using System.Linq;
using FlatMate.Module.Home.Models;
using FlatMate.Module.Home.Provider;
using FlatMate.Module.Lists.Common;
using FlatMate.Module.Lists.Domain.Services;
using Microsoft.EntityFrameworkCore.Internal;

namespace FlatMate.Module.Lists.Domain.Provider
{
    public class DashboardEntryProvider : IDashboardEntryProvider
    {
        private readonly List<DashboardEntryTypeDbo> _dashboardEntries;
        private readonly IItemListService _listService;

        public DashboardEntryProvider(IItemListService listService)
        {
            _listService = listService;
            _dashboardEntries = new List<DashboardEntryTypeDbo>
            {
                new DashboardEntryTypeDbo(Guid.Parse("{268C0364-6C72-4902-8F0F-9B0EA5651A64}")) {Name = "ItemList", ViewComponentName = "ItemListTile", Module = "Lists"},
                new DashboardEntryTypeDbo(Guid.Parse("{268C0364-6C72-4902-8F0F-9B0EA5651A65}")) {Name = "ItemList2", ViewComponentName = "ItemListTile2", Module = "Lists2"}
            };
        }

        public IEnumerable<DashboardEntryTypeDbo> GetEntryTypes()
        {
            return _dashboardEntries;
        }

        public IEnumerable<DashboardEntryTypeValueDbo> GetEntryValues(int currentUserId, Guid id)
        {
            //var userLists = _listService.GetAll(new ItemListQuery {UserId = currentUserId});
            //var publicLists = _listService.GetAll(new ItemListQuery {IsPublic = true});

            //return userLists.Concat(publicLists)
            //                .Distinct((x, y) => x.Id == y.Id)
            //                .OrderByDescending(x => x.LastModified)
            //                .Select(x => new DashboardEntryTypeValueDbo {Name = x.Name, Value = x.Id.ToString()})
            //                .ToList();

            return Enumerable.Empty<DashboardEntryTypeValueDbo>();
        }
    }
}