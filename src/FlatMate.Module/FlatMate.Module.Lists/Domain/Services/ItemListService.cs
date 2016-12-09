using FlatMate.Common.Attributes;
using FlatMate.Module.Account.Domain.Services;
using FlatMate.Module.Lists.Domain.Entities;
using FlatMate.Module.Lists.Persistence;
using prayzzz.Common.Result;

namespace FlatMate.Module.Lists.Domain.Services
{
    public interface IItemListService
    {
        Result<ItemList> Create(ItemList itemList);
        Result Delete(int id);
        Result<ItemList> GetById(int id);
        Result<ItemList> Update(ItemList itemList);
    }

    [Inject]
    public class ItemListService : IItemListService
    {
        private readonly ItemListRepository _persistence;
        private readonly IUserService _userService;

        public ItemListService(ItemListRepository persistence, IUserService userService)
        {
            _persistence = persistence;
            _userService = userService;
        }

        public Result<ItemList> Create(ItemList itemList)
        {
            var getCurrentUser = _userService.GetCurrentUser();
            if (!getCurrentUser.IsSuccess)
            {
                return new ErrorResult<ItemList>(ErrorType.Unauthorized, "Unauthorized");
            }

            // TODO Is allowed to create?

            return _persistence.Add(itemList);
        }

        public Result Delete(int id)
        {
            return _persistence.Remove(id);
        }

        public Result<ItemList> GetById(int id)
        {
            return _persistence.GetById(id);
        }

        public Result<ItemList> Update(ItemList itemList)
        {
            return _persistence.Update(itemList);
        }
    }
}