using FlatMate.Common.Attributes;
using FlatMate.Module.Account.Domain.Entities;
using FlatMate.Module.Account.Persistence.Dbo;
using prayzzz.Common.Mapping;

namespace FlatMate.Module.Account.Persistence.Mapper
{
    [Inject]
    public class UserMapper : IDboMapper
    {
        public void Configure(IMapperConfiguration mapper)
        {
            mapper.Configure<UserDbo, User>(MapToEntity);
            mapper.Configure<User, UserDbo>(MapToDbo);
        }

        private UserDbo MapToDbo(User user, MappingContext ctx)
        {
            return new UserDbo
            {
                Id = user.Id,
                UserName = user.UserName
            };
        }

        private User MapToEntity(UserDbo userDbo, MappingContext ctx)
        {
            return new User(userDbo.Id)
            {
                EMail = userDbo.Email,
                LastName = userDbo.LastName,
                Name = userDbo.Name,
                UserName = userDbo.UserName
            };
        }
    }
}