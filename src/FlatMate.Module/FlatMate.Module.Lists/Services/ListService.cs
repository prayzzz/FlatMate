using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlatMate.Module.Lists.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using prayzzz.Common.Mapping;
using prayzzz.Common.Result;

namespace FlatMate.Module.Lists.Services
{
    public interface IListService
    {
        Task<Result<ItemList>> Create(ItemList itemlist);
        Result<List<ItemList>> GetAll();
        Task<Result<ItemList>> AddItemToList(int listId, Item item);
        Task<Result<ItemList>> AddGroupToList(int listId, ItemListGroup item);
        Task<Result<ItemList>> AddItemToGroup(int listId, int groupId, Item item);
    }

    public class ListService : IListService
    {
        private readonly ListsContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ListService> _logger;

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

        public async Task<Result<ItemList>> AddItemToList(int listId, Item item)
        {
            var listDbo = _context.ItemListsFull.FirstOrDefault(x => x.Id == listId);

            if (listDbo == null)
            {
                return new ErrorResult<ItemList>(ErrorType.NotFound, $"ItemList {listId} not found");
            }

            var itemDbo = _mapper.Map<ItemDbo>(item);
            listDbo.Items.Add(itemDbo);

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

        public async Task<Result<ItemList>> AddGroupToList(int listId, ItemListGroup item)
        {
            var listDbo = _context.ItemListsFull.FirstOrDefault(x => x.Id == listId);

            if (listDbo == null)
            {
                return new ErrorResult<ItemList>(ErrorType.NotFound, $"ItemList {listId} not found");
            }

            var groupDbo = _mapper.Map<ItemListGroupDbo>(item);
            listDbo.ListGroups.Add(groupDbo);

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