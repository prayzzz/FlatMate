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
    public class ItemListMapper : IDboMapper
    {
        private readonly AccountRepository _accountRepository;

        public ItemListMapper(AccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public void Configure(IMapperConfiguration mapper)
        {
            mapper.Configure<ItemListDbo, ItemList>(MapToEntity);
            mapper.Configure<ItemList, ItemListDbo>(MapToDbo);
        }

        private ItemListDbo MapToDbo(ItemList itemList, ItemListDbo dbo, MappingContext ctx)
        {
            dbo.Id = itemList.Id;
            dbo.IsPublic = itemList.IsPublic;
            dbo.Name = itemList.Name;

            if (itemList.Description != null) dbo.Description = itemList.Description;
            if (itemList.Owner != null) dbo.OwnerUserId = itemList.Owner.Id;

            foreach (var listGroup in itemList.Groups)
            {
                var groupDbo = dbo.Groups.FirstOrDefault(x => x.Id == listGroup.Id);
                if (groupDbo == null)
                {
                    groupDbo = new ItemListGroupDbo();
                    dbo.Groups.Add(groupDbo);
                }

                ctx.Mapper.Map(listGroup, groupDbo);
            }

            return dbo;
        }

        private ItemList MapToEntity(ItemListDbo listDbo, MappingContext ctx)
        {
            var owner = ctx.Mapper.Map<User>(_accountRepository.GetById<UserDbo>(listDbo.OwnerUserId).Data);

            var itemList = new ItemList(listDbo.Id, listDbo.Name, owner)
            {
                CreationDate = listDbo.CreationDate,
                Description = listDbo.Description,
                IsPublic = listDbo.IsPublic,
                ModifiedDate = listDbo.LastModified
            };

            if (listDbo.Groups != null)
            {
                foreach (var groupDbo in listDbo.Groups)
                {
                    itemList.AddGroup(ctx.Mapper.Map<ItemListGroup>(groupDbo));
                }
            }

            return itemList;
        }
    }
}