using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlatMate.Module.Lists.Models;
using Microsoft.Extensions.Logging;
using prayzzz.Common.Mapping;
using prayzzz.Common.Result;

namespace FlatMate.Module.Lists.Services
{
    public interface IListService
    {
        Task<Result<ItemList>> Create(ItemList itemlist);
        Result<List<ItemList>> GetAll();
        Result<List<ItemList>> GetAllByUser(int userId);
        Task<Result<Item>> AddItemToList(int listId, Item item);
        Task<Result<ItemListGroup>> AddGroupToList(int listId, ItemListGroup item);
        Task<Result<Item>> AddItemToGroup(int listId, int groupId, Item item);
        Result<List<ItemList>> GetAllPublicByUser(int userId);
        Result<ItemList> GetById(int id);
        Task<Result<Item>> UpdateItemInGroup(int listId, int groupId, int itemId, Item item);
        Task<Result> DeleteItemFromGroup(int listId, int groupId, int itemId);
        Task<Result> DeleteGroupFromList(int listId, int groupId);
        Task<Result> DeletList(int listId);
    }

    public class ListService : IListService
    {
        private readonly ListsContext _context;
        private readonly ILogger<ListService> _logger;
        private readonly IMapper _mapper;

        public ListService(ILoggerFactory loggerFactory, ListsContext context, IMapper mapper)
        {
            _logger = loggerFactory.CreateLogger<ListService>();
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<ItemList>> Create(ItemList itemlist)
        {
            var dbo = _mapper.Map<ItemListDbo>(itemlist);

            _context.Add(dbo);

            try
            {
                await _context.SaveChangesAsync();
                itemlist.Id = dbo.Id;
                return new SuccessResult<ItemList>(itemlist);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Failed saving entity.");
                return new ErrorResult<ItemList>(ex, "Failed saving entity.");
            }
        }

        public Result<List<ItemList>> GetAll()
        {
            return new SuccessResult<List<ItemList>>(_context.ItemListsFull.Select(itemList => _mapper.Map<ItemList>(itemList)).ToList());
        }

        public Result<List<ItemList>> GetAllByUser(int userId)
        {
            return new SuccessResult<List<ItemList>>(_context.ItemListsFull.Where(x => x.UserId == userId).Select(itemList => _mapper.Map<ItemList>(itemList)).ToList());
        }

        public Result<List<ItemList>> GetAllPublicByUser(int userId)
        {
            return new SuccessResult<List<ItemList>>(_context.ItemListsFull.Where(x => x.UserId == userId && x.IsPublic).Select(itemList => _mapper.Map<ItemList>(itemList)).ToList());
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

            var itemDbo = _mapper.Map<ItemDbo>(item);
            groupDbo.Items.Add(itemDbo);

            return await Save<Item, ItemDbo>(itemDbo);
        }

        private async Task<Result<TModel>> Save<TModel, TDbo>(TDbo dboToReturn) where TModel : class
        {
            try
            {
                await _context.SaveChangesAsync();
                return new SuccessResult<TModel>(_mapper.Map<TModel>(dboToReturn));
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Failed saving entity.");
                return new ErrorResult<TModel>(ex, "Failed saving entity.");
            }
        }

        private async Task<Result> Save()
        {
            try
            {
                await _context.SaveChangesAsync();
                return new SuccessResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Failed saving entity.");
                return new ErrorResult(ex, "Failed saving entity.");
            }
        }
    }
}