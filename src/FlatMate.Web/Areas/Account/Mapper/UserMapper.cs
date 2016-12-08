using FlatMate.Common.Attributes;
using FlatMate.Module.Account.Domain.Entities;
using FlatMate.Web.Areas.Account.Dto;
using prayzzz.Common.Mapping;

namespace FlatMate.Web.Areas.Account.Mapper
{
    [Inject]
    public class UserMapper : IDboMapper
    {
        public void Configure(IMapperConfiguration mapper)
        {
            mapper.Configure<User, UserInfoDto>(MapToDto);
        }

        private static UserInfoDto MapToDto(User userDbo, MappingContext ctx)
        {
            return new UserInfoDto
            {
                Id = userDbo.Id,
                UserName = userDbo.UserName
            };
        }
    }
}