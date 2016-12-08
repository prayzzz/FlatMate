using FlatMate.Common.Attributes;
using FlatMate.Module.Lists.Domain.Entities;
using FlatMate.Web.Areas.Account.Dto;
using FlatMate.Web.Areas.Lists.Dto;
using prayzzz.Common.Mapping;

namespace FlatMate.Web.Areas.Lists.Mapper
{
    [Inject]
    public class ItemMapper : IDboMapper
    {
        public void Configure(IMapperConfiguration mapper)
        {
            mapper.Configure<ItemDto, ItemListItem>(MapToEntity);
            mapper.Configure<ItemListItem, ItemDto>(MapToDto);
        }

        private ItemDto MapToDto(ItemListItem item, MappingContext ctx)
        {
            return new ItemDto
            {
                CreationDate = item.CreationDate,
                Id = item.Id,
                LastEditor = ctx.Mapper.Map<UserInfoDto>(item.LastEditor),
                Name = item.Name,
                LastModified = item.ModifiedDate,
                Order = item.Order,
                Owner = ctx.Mapper.Map<UserInfoDto>(item.Owner)
            };
        }

        private ItemListItem MapToEntity(ItemDto itemDto, ItemListItem item, MappingContext ctx)
        {
            item.Order = itemDto.Order;

            item.Rename(itemDto.Name);

            return item;
        }
    }
}