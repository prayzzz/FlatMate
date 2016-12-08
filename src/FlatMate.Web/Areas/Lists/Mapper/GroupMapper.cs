using System.Linq;
using FlatMate.Common.Attributes;
using FlatMate.Module.Lists.Domain.Entities;
using FlatMate.Web.Areas.Account.Dto;
using FlatMate.Web.Areas.Lists.Dto;
using prayzzz.Common.Mapping;

namespace FlatMate.Web.Areas.Lists.Mapper
{
    [Inject]
    public class GroupMapper : IDboMapper
    {
        public void Configure(IMapperConfiguration mapper)
        {
            mapper.Configure<GroupUpdateDto, ItemListGroup>(MapToEntity);
            mapper.Configure<ItemListGroup, GroupDto>(MapToDto);
        }

        private GroupDto MapToDto(ItemListGroup group, MappingContext ctx)
        {
            return new GroupDto
            {
                CreationDate = group.CreationDate,
                Id = group.Id,
                Items = group.Items.Select(items => ctx.Mapper.Map<ItemDto>(items)).ToList(),
                LastEditor = ctx.Mapper.Map<UserInfoDto>(group.LastEditor),
                ModifiedDate = group.ModifiedDate,
                Name = group.Name,
                Order = group.Order,
                Owner = ctx.Mapper.Map<UserInfoDto>(group.Owner)
            };
        }

        private ItemListGroup MapToEntity(GroupUpdateDto groupDto, ItemListGroup group, MappingContext ctx)
        {
            group.Order = groupDto.Order;

            group.Rename(groupDto.Name);

            return group;
        }
    }
}