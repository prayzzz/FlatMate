using FlatMate.Common;
using FlatMate.Common.Attributes;
using FlatMate.Module.Lists.Persistence.Dbo;
using prayzzz.Common.Mvc.Services;

namespace FlatMate.Module.Lists.Domain.Services
{
    [Inject(DependencyLifetime.Singleton, typeof(ItemListPrivileger))]
    public class ItemListPrivileger
    {
        private readonly IRequestService _request;

        public ItemListPrivileger(IRequestService request)
        {
            _request = request;
        }

        public ModelPrivileges GetPrivileges(ItemListDbo itemList)
        {
            return new ModelPrivileges(IsOwned(itemList), IsEditable(itemList), IsDeletable(itemList));
        }

        public ModelPrivileges GetPrivileges(ItemListGroupDbo itemListGroup)
        {
            return new ModelPrivileges(IsOwned(itemListGroup), IsEditable(itemListGroup), IsDeletable(itemListGroup));
        }

        public ModelPrivileges GetPrivileges(ItemDbo item)
        {
            return new ModelPrivileges(IsOwned(item), IsEditable(item), IsDeletable(item));
        }

        public bool IsDeletable(ItemListDbo itemList)
        {
            return itemList.OwnerUserId == _request.CurrentUserId;
        }

        public bool IsDeletable(ItemListGroupDbo itemListGroup)
        {
            // current user owns this list
            if (itemListGroup.List.OwnerUserId == _request.CurrentUserId)
            {
                return true;
            }

            return itemListGroup.List.IsPublic;
        }

        public bool IsDeletable(ItemDbo item)
        {
            // current user owns this list
            //if (item.List.UserId == _request.CurrentUserId)
            //{
            //    return true;
            //}

            //return item.List.IsPublic;

            return true;
        }

        public bool IsEditable(ItemListDbo itemList)
        {
            return itemList.IsPublic || itemList.OwnerUserId == _request.CurrentUserId;
        }

        public bool IsEditable(ItemListGroupDbo itemListGroup)
        {
            if (itemListGroup.List.IsPublic)
            {
                return true;
            }

            if (itemListGroup.List.OwnerUserId == _request.CurrentUserId)
            {
                return true;
            }

            if (itemListGroup.OwnerUserId == _request.CurrentUserId)
            {
                return true;
            }

            return false;
        }

        public bool IsEditable(ItemDbo item)
        {
            //if (item.List.IsPublic)
            //{
            //    return true;
            //}

            //if (item.List.UserId == _request.CurrentUserId)
            //{
            //    return true;
            //}

            if (item.OwnerUserId == _request.CurrentUserId)
            {
                return true;
            }

            return false;
        }

        public bool IsOwned(ItemListDbo itemList)
        {
            return itemList.OwnerUserId == _request.CurrentUserId;
        }

        public bool IsOwned(ItemListGroupDbo itemListGroup)
        {
            return itemListGroup.OwnerUserId == _request.CurrentUserId;
        }

        public bool IsOwned(ItemDbo item)
        {
            return item.OwnerUserId == _request.CurrentUserId;
        }
    }
}