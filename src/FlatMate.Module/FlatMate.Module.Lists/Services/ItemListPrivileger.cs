using FlatMate.Common;
using FlatMate.Module.Lists.Models;
using prayzzz.Common.Mvc.Services;

namespace FlatMate.Module.Lists.Services
{
    public class ItemListPrivileger
    {
        private readonly IRequestService _request;

        public ItemListPrivileger(IRequestService request)
        {
            _request = request;
        }

        public bool IsEditable(ItemListDbo itemList)
        {
            return itemList.IsPublic || itemList.UserId == _request.CurrentUserId;
        }

        public bool IsOwned(ItemListDbo itemList)
        {
            return itemList.UserId == _request.CurrentUserId;
        }

        public bool IsDeletable(ItemListDbo itemList)
        {
            return itemList.UserId == _request.CurrentUserId;
        }

        public ModelPrivileges GetPrivileges(ItemListDbo itemList)
        {
            return new ModelPrivileges(IsOwned(itemList), IsEditable(itemList), IsDeletable(itemList));
        }

        public bool IsOwned(ItemListGroupDbo itemListGroup)
        {
            return itemListGroup.UserId == _request.CurrentUserId;
        }

        public bool IsEditable(ItemListGroupDbo itemListGroup)
        {
            if (itemListGroup.ItemList.IsPublic)
            {
                return true;
            }
            
            if (itemListGroup.ItemList.UserId == _request.CurrentUserId)
            {
                return true;
            }
            
            if (itemListGroup.UserId == _request.CurrentUserId)
            {
                return true;
            }

            return false;
        }

        public bool IsDeletable(ItemListGroupDbo itemListGroup)
        {
            // current user owns this list
            if (itemListGroup.ItemList.UserId == _request.CurrentUserId)
            {
                return true;
            }

            return itemListGroup.ItemList.IsPublic;
        }

        public ModelPrivileges GetPrivileges(ItemListGroupDbo itemListGroup)
        {
            return new ModelPrivileges(IsOwned(itemListGroup), IsEditable(itemListGroup), IsDeletable(itemListGroup));
        }

        public bool IsOwned(ItemDbo item)
        {
            return item.UserId == _request.CurrentUserId;
        }

        public bool IsEditable(ItemDbo item)
        {
            if (item.ItemList.IsPublic)
            {
                return true;
            }

            if (item.ItemList.UserId == _request.CurrentUserId)
            {
                return true;
            }

            if (item.UserId == _request.CurrentUserId)
            {
                return true;
            }

            return false;
        }

        public bool IsDeletable(ItemDbo item)
        {
            // current user owns this list
            if (item.ItemList.UserId == _request.CurrentUserId)
            {
                return true;
            }

            return item.ItemList.IsPublic;
        }

        public ModelPrivileges GetPrivileges(ItemDbo item)
        {
            return new ModelPrivileges(IsOwned(item), IsEditable(item), IsDeletable(item));
        }
    }
}