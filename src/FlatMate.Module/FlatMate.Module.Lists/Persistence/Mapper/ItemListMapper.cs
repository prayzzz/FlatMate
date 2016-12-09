using System.Linq;
using FlatMate.Common.Attributes;
using FlatMate.Module.Account.Domain.Entities;
using FlatMate.Module.Account.Persistence;
using FlatMate.Module.Account.Persistence.Dbo;
using FlatMate.Module.Account.Persistence.Repositories;
using FlatMate.Module.Lists.Domain.Entities;
using FlatMate.Module.Lists.Persistence.Dbo;
using prayzzz.Common.Mapping;

namespace FlatMate.Module.Lists.Persistence.Mapper
{
    [Inject]
    public class ItemListMapper : IDboMapper
    {
        private readonly UserRepository _userRepository;

        public ItemListMapper(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void Configure(IMapperConfiguration mapper)
        {
            mapper.Configure<ItemListDbo, ItemList>(MapToEntity);
            mapper.Configure<ItemList, ItemListDbo>(MapToDbo);
        }

        private ItemListDbo MapToDbo(ItemList itemList, ItemListDbo listDbo, MappingContext ctx)
        {
            listDbo.Id = itemList.Id;
            listDbo.IsPublic = itemList.IsPublic;
            listDbo.Name = itemList.Name;

            if (itemList.Description != null) listDbo.Description = itemList.Description;
            if (itemList.Owner != null) listDbo.OwnerUserId = itemList.Owner.Id;
            
            listDbo.Groups.RemoveAll(groupDbo => itemList.Groups.All(g => g.Id != listDbo.Id));

            foreach (var listGroup in itemList.Groups)
            {
                var groupDbo = listDbo.Groups.FirstOrDefault(x => x.Id == listGroup.Id);
                if (groupDbo == null)
                {
                    groupDbo = new ItemListGroupDbo();
                    listDbo.Groups.Add(groupDbo);
                }

                ctx.Mapper.Map(listGroup, groupDbo);
            }

            return listDbo;
        }

        private ItemList MapToEntity(ItemListDbo listDbo, MappingContext ctx)
        {
            var owner = ctx.Mapper.Map<User>(_userRepository.GetById(listDbo.OwnerUserId).Data);

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