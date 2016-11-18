using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlatMate.Common.Repository;
using FlatMate.Module.Lists.Models;
using prayzzz.Common.Linq;
using prayzzz.Common.Mapping;
using prayzzz.Common.Result;

namespace FlatMate.Module.Lists.Services
{
    public interface IItemListService
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

    public class ItemListService : IItemListService
    {
        private readonly IMapper _mapper;
        private readonly ItemListPrivileger _privileger;
        private readonly IRepository<ItemListDbo> _repository;

        public ItemListService(IRepository<ItemListDbo> repository, IMapper mapper, ItemListPrivileger privileger)
        {
            _repository = repository;
            _mapper = mapper;
            _privileger = privileger;
        }

        public async Task<Result<ItemList>> Create(ItemList itemlist)
        {
            var listDbo = _mapper.Map<ItemListDbo>(itemlist);

            _repository.Add(listDbo);
            return await Save<ItemList>(listDbo);
        }

        public IEnumerable<ItemList> GetAll(ItemListQuery query)
        {
            var itemLists = _repository.GetAll();

            if (query != null)
            {
                if (query.IsPublic.HasValue) itemLists = itemLists.Where(x => x.IsPublic == query.IsPublic.Value);
                if (query.UserId.HasValue) itemLists = itemLists.Where(x => x.UserId == query.UserId.Value);
                if (query.Order == ItemListQueryOrder.LastModified) itemLists = itemLists.OrderBy(x => x.LastModified, query.Direction);
            }

            return itemLists.Select(itemList => _mapper.Map<ItemList>(itemList));
        }

        public Result<ItemList> GetById(int id)
        {
            var listDbo = _repository.GetById(id);
            if (!listDbo.IsSuccess)
            {
                return new ErrorResult<ItemList>(listDbo);
            }

            return new SuccessResult<ItemList>(_mapper.Map<ItemList>(listDbo.Data));
        }

        public async Task<Result<ItemList>> UpdateItemList(int listId, ItemList itemList)
        {
            var listDbo = _repository.GetById(listId);
            if (!listDbo.IsSuccess)
            {
                return new ErrorResult<ItemList>(listDbo);
            }

            if (!_privileger.IsOwned(listDbo.Data))
            {
                return new ErrorResult<ItemList>(ErrorType.Unauthorized, "Unauthorized");
            }

            return await Save<ItemList>(_mapper.Map(itemList, listDbo.Data));
        }

        public async Task<Result<Item>> UpdateItemInGroup(int listId, int groupId, int itemId, Item item)
        {
            var listDboResult = _repository.GetById(listId);
            if (!listDboResult.IsSuccess)
            {
                return new ErrorResult<Item>(listDboResult);
            }

            var listDbo = listDboResult.Data;
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

            if (!_privileger.IsEditable(itemDbo))
            {
                return new ErrorResult<Item>(ErrorType.Unauthorized, "Unauthorized");
            }

            itemDbo = _mapper.Map(item, itemDbo);
            return await Save<Item>(itemDbo);
        }

        public async Task<Result> DeleteItemFromGroup(int listId, int groupId, int itemId)
        {
            var listDboResult = _repository.GetById(listId);
            if (!listDboResult.IsSuccess)
            {
                return new ErrorResult<Item>(listDboResult);
            }

            var listDbo = listDboResult.Data;
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

            if (!_privileger.IsDeletable(itemDbo))
            {
                return new ErrorResult<Item>(ErrorType.Unauthorized, "Unauthorized");
            }

            _repository.Remove(itemDbo);
            return await _repository.Save();
        }

        public async Task<Result> DeleteGroupFromList(int listId, int groupId)
        {
            var listDboResult = _repository.GetById(listId);
            if (!listDboResult.IsSuccess)
            {
                return new ErrorResult<Item>(listDboResult);
            }

            var listDbo = listDboResult.Data;
            var groupDbo = listDbo.ListGroups.FirstOrDefault(x => x.Id == groupId);
            if (groupDbo == null)
            {
                return new ErrorResult(ErrorType.NotFound, $"ItemListGroup {groupId} not found in ItemList {listId}");
            }

            if (!_privileger.IsDeletable(groupDbo))
            {
                return new ErrorResult<Item>(ErrorType.Unauthorized, "Unauthorized");
            }

            _repository.Remove(groupDbo);
            return await _repository.Save();
        }

        public async Task<Result> DeletList(int listId)
        {
            var listDboResult = _repository.GetById(listId);
            if (!listDboResult.IsSuccess)
            {
                return new ErrorResult<Item>(listDboResult);
            }

            var listDbo = listDboResult.Data;
            if (!_privileger.IsOwned(listDbo))
            {
                return new ErrorResult<Item>(ErrorType.Unauthorized, "Unauthorized");
            }

            _repository.Remove(listDbo);
            return await _repository.Save();
        }

        public async Task<Result<Item>> AddItemToList(int listId, Item item)
        {
            var listDboResult = _repository.GetById(listId);
            if (!listDboResult.IsSuccess)
            {
                return new ErrorResult<Item>(listDboResult);
            }

            var listDbo = listDboResult.Data;
            if (!_privileger.IsEditable(listDbo))
            {
                return new ErrorResult<Item>(ErrorType.Unauthorized, "Unauthorized");
            }

            var itemDbo = _mapper.Map<ItemDbo>(item);
            listDbo.Items.Add(itemDbo);

            return await Save<Item>(itemDbo);
        }

        public async Task<Result<ItemListGroup>> AddGroupToList(int listId, ItemListGroup item)
        {
            var listDboResult = _repository.GetById(listId);
            if (!listDboResult.IsSuccess)
            {
                return new ErrorResult<ItemListGroup>(listDboResult);
            }

            var listDbo = listDboResult.Data;
            if (!_privileger.IsEditable(listDbo))
            {
                return new ErrorResult<ItemListGroup>(ErrorType.Unauthorized, "Unauthorized");
            }

            var groupDbo = _mapper.Map<ItemListGroupDbo>(item);
            listDbo.ListGroups.Add(groupDbo);

            return await Save<ItemListGroup>(groupDbo);
        }

        public async Task<Result<Item>> AddItemToGroup(int listId, int groupId, Item item)
        {
            var listDboResult = _repository.GetById(listId);
            if (!listDboResult.IsSuccess)
            {
                return new ErrorResult<Item>(listDboResult);
            }

            var listDbo = listDboResult.Data;
            var groupDbo = listDbo.ListGroups.FirstOrDefault(x => x.Id == groupId);
            if (groupDbo == null)
            {
                return new ErrorResult<Item>(ErrorType.NotFound, $"ItemListGroup {groupId} not found in ItemList {listId}");
            }

            if (!_privileger.IsEditable(groupDbo))
            {
                return new ErrorResult<Item>(ErrorType.Unauthorized, "Unauthorized");
            }

            var itemDbo = _mapper.Map<ItemDbo>(item);
            groupDbo.Items.Add(itemDbo);

            return await Save<Item>(itemDbo);
        }

        private async Task<Result<TModel>> Save<TModel>(object itemDbo) where TModel : class
        {
            var result = await _repository.Save();
            if (!result.IsSuccess)
            {
                return new ErrorResult<TModel>(result);
            }

            return new SuccessResult<TModel>(_mapper.Map<TModel>(itemDbo));
        }
    }
}