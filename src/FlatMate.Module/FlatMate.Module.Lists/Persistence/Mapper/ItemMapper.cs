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
    public class ItemMapper : IDboMapper
    {
        private readonly AccountRepository _accountRepository;

        public ItemMapper(AccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public void Configure(IMapperConfiguration mapper)
        {
            mapper.Configure<ItemDbo, ItemListItem>(MapToEntity);
            mapper.Configure<ItemListItem, ItemDbo>(MapToDbo);
        }

        private ItemDbo MapToDbo(ItemListItem item, ItemDbo dbo, MappingContext ctx)
        {
            dbo.Id = item.Id;
            dbo.LastEditorUserId = item.LastEditor.Id;
            dbo.Name = item.Name;
            dbo.Order = item.Order;
            dbo.OwnerUserId = item.Owner.Id;

            return dbo;
        }

        private ItemListItem MapToEntity(ItemDbo itemDbo, MappingContext ctx)
        {
            var owner = ctx.Mapper.Map<User>(_accountRepository.GetById<UserDbo>(itemDbo.OwnerUserId).Data);

            return new ItemListItem(itemDbo.Id, itemDbo.Name, owner)
            {
                CreationDate = itemDbo.CreationDate,
                LastEditor = ctx.Mapper.Map<User>(_accountRepository.GetById<UserDbo>(itemDbo.LastEditorUserId).Data),
                ModifiedDate = itemDbo.LastModified,
                Order = itemDbo.Order,
            };
        }
    }
}