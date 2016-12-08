using System.Linq;
using FlatMate.Common.Attributes;
using FlatMate.Module.Account.Domain.Entities;
using FlatMate.Module.Account.Persistence.Dbo;
using FlatMate.Module.Account.Repository;
using FlatMate.Module.Lists.Domain.Entities;
using FlatMate.Module.Lists.Persistence.Dbo;
using prayzzz.Common.Mapping;

namespace FlatMate.Module.Lists.Persistence.Mapper
{
    [Inject]
    public class ItemListGroupMapper : IDboMapper
    {
        private readonly AccountRepository _accountRepository;

        public ItemListGroupMapper(AccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public void Configure(IMapperConfiguration mapper)
        {
            mapper.Configure<ItemListGroupDbo, ItemListGroup>(MapToEntity);
            mapper.Configure<ItemListGroup, ItemListGroupDbo>(MapToDbo);
        }

        private ItemListGroupDbo MapToDbo(ItemListGroup group, ItemListGroupDbo dbo, MappingContext ctx)
        {
            dbo.Id = group.Id;
            dbo.Name = group.Name;
            dbo.OwnerUserId = group.Owner.Id;
            dbo.Order = group.Order;

            foreach (var listItems in group.Items)
            {
                var itemDbo = dbo.Items.FirstOrDefault(x => x.Id == listItems.Id);
                if (itemDbo == null)
                {
                    itemDbo = new ItemDbo();
                    dbo.Items.Add(itemDbo);
                }

                ctx.Mapper.Map(listItems, itemDbo);
            }

            return dbo;
        }

        private ItemListGroup MapToEntity(ItemListGroupDbo groupDbo, MappingContext ctx)
        {
            var owner = ctx.Mapper.Map<User>(_accountRepository.GetById<UserDbo>(groupDbo.OwnerUserId).Data);

            var itemListGroup = new ItemListGroup(groupDbo.Id, groupDbo.Name, owner)
            {
                CreationDate = groupDbo.CreationDate,
                LastEditor = ctx.Mapper.Map<User>(_accountRepository.GetById<UserDbo>(groupDbo.LastEditorUserId).Data),
                ModifiedDate = groupDbo.LastModified,
                Order = groupDbo.Order
        };

            if (groupDbo.Items != null)
            {
                foreach (var itemDbo in groupDbo.Items)
                {
                    itemListGroup.AddItem(ctx.Mapper.Map<ItemListItem>(itemDbo));
                }
}

            return itemListGroup;
        }
    }
}