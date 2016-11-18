using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using prayzzz.Common.Dbo;
using prayzzz.Common.Mapping;

namespace FlatMate.Module.Account.Models
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; }
    }

    public class LoginUser
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string UserName { get; set; }
    }

    public class RegisterUser
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string LastName { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string PasswordConfirmation { get; set; }

        [Required]
        public string UserName { get; set; }
    }

    [Table("User")]
    public class UserDbo : BaseDbo
    {
        public string Email { get; set; }

        public string LastName { get; set; }

        public string Name { get; set; }

        public string PasswordHash { get; set; }

        public string Salt { get; set; }

        public string UserName { get; set; }
    }

    public class UserMapper : IDboMapper
    {
        public void Configure(IMapperConfiguration mapper)
        {
            mapper.Configure<UserDbo, User>(MapToUserModel);
        }

        private static User MapToUserModel(UserDbo userDbo, MappingContext ctx)
        {
            var model = new User
            {
                Id = userDbo.Id,
                UserName = userDbo.UserName
            };

            return model;
        }
    }
}
