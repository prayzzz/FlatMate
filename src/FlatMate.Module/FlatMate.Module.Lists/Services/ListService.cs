using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlatMate.Module.Lists.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using prayzzz.Common.Dbo;
using prayzzz.Common.Linq;
using prayzzz.Common.Mapping;
using prayzzz.Common.Result;

namespace FlatMate.Module.Lists.Services
{
    public interface IListService
    {
        Task<Result<ItemList>> Create(ItemList itemlist);
        IEnumerable<ItemList> GetAll(ItemListQuery query);
        Task<Result<Item>> AddItemToList(int listId, Item item);
        Task<Result<ItemListGroup>> AddGroupToList(int listId, ItemListGroup item);
        Task<Result<Item>> AddItemToGroup(int listId, int groupId, Item item);
        Result<ItemList> GetById(int id);
        Task<Result<Item>> UpdateItemInGroup(int listId, int groupId, int itemId, Item item);
        Task<Result> DeleteItemFromGroup(int listId, int groupId, int itemId);
        Task<Result> DeleteGroupFromList(int listId, int groupId);
        Task<Result> DeletList(int listId);
        Task<Result<ItemList>> UpdateItemList(int listId, ItemList itemList);
    }

    public class ListService : IListService
    {
        private readonly ListsContext _context;
        private readonly ILogger<ListService> _logger;
        private readonly IMapper _mapper;
        private readonly IOwnerCheck _ownerCheck;

        public ListService(ILoggerFactory loggerFactory, ListsContext context, IMapper mapper, IOwnerCheck ownerCheck)
        {
            _logger = loggerFactory.CreateLogger<ListService>();
            _context = context;
            _mapper = mapper;
            _ownerCheck = ownerCheck;
        }

        public async Task<Result<ItemList>> Create(ItemList itemlist)
        {
            var listDbo = _mapper.Map<ItemListDbo>(itemlist);

            _context.Add(listDbo);

            return await Save<ItemList, ItemListDbo>(listDbo);
        }

        public IEnumerable<ItemList> GetAll(ItemListQuery query)
        {
            var all = _context.ItemListsFull;

            if (query != null)
            {
                if (query.IsPublic.HasValue) all = all.Where(x => x.IsPublic == query.IsPublic.Value);
                if (query.UserId.HasValue) all = all.Where(x => x.UserId == query.UserId.Value);
                if (query.Order == ItemListQueryOrder.LastModified) all = all.OrderBy(x => x.LastModified, query.Direction);
                //if (query.Limit.HasValue) all = all.Take(query.Limit.Value); // TODO add if supported by mysql
            }

            return all.Select(itemList => _mapper.Map<ItemList>(itemList));
        }

        public Result<ItemList> GetById(int id)
        {
            var dbo = _context.ItemListsFull.FirstOrDefault(x => x.Id == id);

            if (dbo == null)
            {
                return new ErrorResult<ItemList>(ErrorType.NotFound, $"Entity {id} not found");
            }

            return new SuccessResult<ItemList>(_mapper.Map<ItemList>(dbo));
        }

        public async Task<Result<ItemList>> UpdateItemList(int listId, ItemList itemList)
        {
            var listDbo = _context.ItemListsFull.FirstOrDefault(x => x.Id == listId);
            if (listDbo == null)
            {
                return new ErrorResult<ItemList>(ErrorType.NotFound, $"ItemList {listId} not found");
            }

            if (!_ownerCheck.IsOwnedByCurrentUser(listDbo))
            {
                return new ErrorResult<ItemList>(ErrorType.Unauthorized, "Unauthorized");
            }

            listDbo = _mapper.Map(itemList, listDbo);

            return await Save<ItemList, ItemListDbo>(listDbo);
        }

        public async Task<Result<Item>> UpdateItemInGroup(int listId, int groupId, int itemId, Item item)
        {
            var listDbo = _context.ItemListsFull.FirstOrDefault(x => x.Id == listId);
            if (listDbo == null)
            {
                return new ErrorResult<Item>(ErrorType.NotFound, $"ItemList {listId} not found");
            }

            var groupDbo = listDbo.ListGroups.FirstOrDefault(x => x.Id == groupId);
            if (groupDbo == null)
            {
                return new ErrorResult<Item>(ErrorType.NotFound, $"ItemListGroup {groupId} not found in ItemList {listId}");
            }

            var itemDbo = listDbo.Items.FirstOrDefault(x => x.Id == itemId);
            if (itemDbo == null)
            {
                return new ErrorResult<Item>(ErrorType.NotFound, $"Item {itemId} not found in ItemListGroup {groupId} (ItemList {listId})");
            }

            if (item.UserId != itemDbo.UserId)
            {
                return new ErrorResult<Item>(ErrorType.Unauthorized, "Unauthorized");
            }

            itemDbo = _mapper.Map(item, itemDbo);
            return await Save<Item, ItemDbo>(itemDbo);
        }

        public async Task<Result> DeleteItemFromGroup(int listId, int groupId, int itemId)
        {
            var listDbo = _context.ItemListsFull.FirstOrDefault(x => x.Id == listId);
            if (listDbo == null)
            {
                return new ErrorResult(ErrorType.NotFound, $"ItemList {listId} not found");
            }

            var groupDbo = listDbo.ListGroups.FirstOrDefault(x => x.Id == groupId);
            if (groupDbo == null)
            {
                return new ErrorResult(ErrorType.NotFound, $"ItemListGroup {groupId} not found in ItemList {listId}");
            }

            var itemDbo = listDbo.Items.FirstOrDefault(x => x.Id == itemId);
            if (itemDbo == null)
            {
                return new ErrorResult(ErrorType.NotFound, $"Item {itemId} not found in ItemListGroup {groupId} (ItemList {listId})");
            }

            if (!_ownerCheck.IsOwnedByCurrentUser(itemDbo) && !_ownerCheck.IsOwnedByCurrentUser(listDbo))
            {
                return new ErrorResult<Item>(ErrorType.Unauthorized, "Unauthorized");
            }

            _context.Remove(itemDbo);
            return await Save();
        }

        public async Task<Result> DeleteGroupFromList(int listId, int groupId)
        {
            var listDbo = _context.ItemListsFull.FirstOrDefault(x => x.Id == listId);
            if (listDbo == null)
            {
                return new ErrorResult(ErrorType.NotFound, $"ItemList {listId} not found");
            }

            var groupDbo = listDbo.ListGroups.FirstOrDefault(x => x.Id == groupId);
            if (groupDbo == null)
            {
                return new ErrorResult(ErrorType.NotFound, $"ItemListGroup {groupId} not found in ItemList {listId}");
            }

            if (!_ownerCheck.IsOwnedByCurrentUser(groupDbo) && !_ownerCheck.IsOwnedByCurrentUser(listDbo))
            {
                return new ErrorResult<Item>(ErrorType.Unauthorized, "Unauthorized");
            }

            listDbo.LastModified = DateTime.Now;

            _context.Remove(groupDbo);
            return await Save();
        }

        public async Task<Result> DeletList(int listId)
        {
            var listDbo = _context.ItemListsFull.FirstOrDefault(x => x.Id == listId);
            if (listDbo == null)
            {
                return new ErrorResult(ErrorType.NotFound, $"ItemList {listId} not found");
            }

            if (!_ownerCheck.IsOwnedByCurrentUser(listDbo))
            {
                return new ErrorResult<Item>(ErrorType.Unauthorized, "Unauthorized");
            }

            _context.Remove(listDbo);
            return await Save();
        }

        public async Task<Result<Item>> AddItemToList(int listId, Item item)
        {
            var listDbo = _context.ItemListsFull.FirstOrDefault(x => x.Id == listId);
            if (listDbo == null)
            {
                return new ErrorResult<Item>(ErrorType.NotFound, $"ItemList {listId} not found");
            }

            if (!_ownerCheck.IsOwnedByCurrentUser(listDbo) && !listDbo.IsPublic)
            {
                return new ErrorResult<Item>(ErrorType.Unauthorized, "Unauthorized");
            }

            var itemDbo = _mapper.Map<ItemDbo>(item);
            listDbo.Items.Add(itemDbo);

            return await Save<Item, ItemDbo>(itemDbo);
        }

        public async Task<Result<ItemListGroup>> AddGroupToList(int listId, ItemListGroup item)
        {
            var listDbo = _context.ItemListsFull.FirstOrDefault(x => x.Id == listId);
            if (listDbo == null)
            {
                return new ErrorResult<ItemListGroup>(ErrorType.NotFound, $"ItemList {listId} not found");
            }

            if (!_ownerCheck.IsOwnedByCurrentUser(listDbo) && !listDbo.IsPublic)
            {
                return new ErrorResult<ItemListGroup>(ErrorType.Unauthorized, "Unauthorized");
            }

            var groupDbo = _mapper.Map<ItemListGroupDbo>(item);
            listDbo.ListGroups.Add(groupDbo);

            return await Save<ItemListGroup, ItemListGroupDbo>(groupDbo);
        }

        public async Task<Result<Item>> AddItemToGroup(int listId, int groupId, Item item)
        {
            var listDbo = _context.ItemListsFull.FirstOrDefault(x => x.Id == listId);
            if (listDbo == null)
            {
                return new ErrorResult<Item>(ErrorType.NotFound, $"ItemList {listId} not found");
            }

            var groupDbo = listDbo.ListGroups.FirstOrDefault(x => x.Id == groupId);
            if (groupDbo == null)
            {
                return new ErrorResult<Item>(ErrorType.NotFound, $"ItemListGroup {groupId} not found in ItemList {listId}");
            }

            if (!_ownerCheck.IsOwnedByCurrentUser(listDbo) && !listDbo.IsPublic)
            {
                return new ErrorResult<Item>(ErrorType.Unauthorized, "Unauthorized");
            }

            var itemDbo = _mapper.Map<ItemDbo>(item);
            groupDbo.Items.Add(itemDbo);

            return await Save<Item, ItemDbo>(itemDbo);
        }

        private void ApplyDates()
        {
            var now = DateTime.Now;
            foreach (var entry in _context.ChangeTracker.Entries().Where(x => x.State == EntityState.Added))
            {
                ((BaseDbo)entry.Entity).CreationDate = now;
                ((BaseDbo)entry.Entity).LastModified = now;
            }

            foreach (var entry in _context.ChangeTracker.Entries().Where(x => x.State == EntityState.Modified))
            {
                ((BaseDbo)entry.Entity).LastModified = now;
            }
        }

        private async Task<Result<TModel>> Save<TModel, TDbo>(TDbo dboToReturn) where TModel : class
        {
            var result = await Save();
            if (result.IsSuccess)
            {
                return new SuccessResult<TModel>(_mapper.Map<TModel>(dboToReturn));
            }

            return new ErrorResult<TModel>(result);
        }

        private async Task<Result> Save()
        {
            ApplyDates();

            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync();
                    trans.Commit();
                    return new SuccessResult();
                }
                catch (Exception ex)
                {
                    trans.Rollback();

                    _logger.LogError(0, ex, "Failed saving entity.");
                    return new ErrorResult(ex, "Failed saving entity.");
                }
            }
        }
    }
}