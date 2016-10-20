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
        Task<Result<ItemList>> AddItemToList(int listId, Item item);
        Task<Result<ItemList>> AddGroupToList(int listId, ItemListGroup item);
        Task<Result<ItemList>> AddItemToGroup(int listId, int groupId, Item item);
        Result<List<ItemList>> GetAllPublicByUser(int userId);
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

        public async Task<Result<ItemList>> AddItemToList(int listId, Item item)
        {
            var listDbo = _context.ItemListsFull.FirstOrDefault(x => x.Id == listId);

            if (listDbo == null)
            {
                return new ErrorResult<ItemList>(ErrorType.NotFound, $"ItemList {listId} not found");
            }

            var itemDbo = _mapper.Map<ItemDbo>(item);
            listDbo.Items.Add(itemDbo);
            
            return await Save(listDbo);
        }

        public async Task<Result<ItemList>> AddGroupToList(int listId, ItemListGroup item)
        {
            var listDbo = _context.ItemListsFull.FirstOrDefault(x => x.Id == listId);

            if (listDbo == null)
            {
                return new ErrorResult<ItemList>(ErrorType.NotFound, $"ItemList {listId} not found");
            }

            var groupDbo = _mapper.Map<ItemListGroupDbo>(item);
            listDbo.ListGroups.Add(groupDbo);
            
            return await Save(listDbo);
        }

        public async Task<Result<ItemList>> AddItemToGroup(int listId, int groupId, Item item)
        {
            var listDbo = _context.ItemListsFull.FirstOrDefault(x => x.Id == listId);
            if (listDbo == null)
            {
                return new ErrorResult<ItemList>(ErrorType.NotFound, $"ItemList {listId} not found");
            }

            var groupDbo = listDbo.ListGroups.FirstOrDefault(x => x.Id == groupId);
            if (groupDbo == null)
            {
                return new ErrorResult<ItemList>(ErrorType.NotFound, $"ItemListGroup {groupId} not found in ItemList {listId}");
            }

            var itemDbo = _mapper.Map<ItemDbo>(item);
            groupDbo.Items.Add(itemDbo);

            return await Save(listDbo);
        }

        private async Task<Result<ItemList>> Save(ItemListDbo listDbo)
        {
            try
            {
                await _context.SaveChangesAsync();
                return new SuccessResult<ItemList>(_mapper.Map<ItemList>(listDbo));
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Failed saving entity.");
                return new ErrorResult<ItemList>(ex, "Failed saving entity.");
            }
        }
    }
}