using System.Linq;
using FlatMate.Common.Attributes;
using FlatMate.Module.Lists.Domain.Entities;
using FlatMate.Web.Areas.Account.Dto;
using FlatMate.Web.Areas.Lists.Dto;
using prayzzz.Common.Mapping;

namespace FlatMate.Web.Areas.Lists.Mapper
{
    [Inject]
    public class ListMapper : IDboMapper
    {
        public void Configure(IMapperConfiguration mapper)
        {
            mapper.Configure<ListUpdateDto, ItemList>(MapToEntity);
            mapper.Configure<ItemList, ListDto>(MapToDto);
        }

        private ListDto MapToDto(ItemList itemList, MappingContext ctx)
        {
            return new ListDto
            {
                CreationDate = itemList.CreationDate,
                Description = itemList.Description,
                Groups = itemList.Groups.Select(group => ctx.Mapper.Map<GroupDto>(group)).ToList(),
                Id = itemList.Id,
                IsPublic = itemList.IsPublic,
                ModifiedDate = itemList.ModifiedDate,
                Name = itemList.Name,
                Owner = ctx.Mapper.Map<UserInfoDto>(itemList.Owner)
            };
        }

        private ItemList MapToEntity(ListUpdateDto listDto, ItemList itemList, MappingContext ctx)
        {
            itemList.Rename(listDto.Name);

            if (listDto.IsPublic != null) itemList.IsPublic = listDto.IsPublic.Value;
            if (listDto.Description != null) itemList.Description = listDto.Description;

            return itemList;
        }
    }
}