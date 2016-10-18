using System.Linq;
using FlatMate.Module.Account.Models;
using prayzzz.Common.Result;

namespace FlatMate.Module.Account.Services
{
    public interface IUserService
    {}

    public class UserService : IUserService
    {
        private readonly AccountContext _context;

        public UserService(AccountContext context)
        {
            _context = context;
        }

        public Result<User> GetUser(int id)
        {
            var userDbo = _context.User.FirstOrDefault(x => x.Id == id);

            if (userDbo == null)
            {
                return new ErrorResult<User>($"No entity with id {id}");
            }

            var user = new User
            {
                UserName = userDbo.UserName,
                Name = userDbo.Name,
                LastName = userDbo.LastName,
                Email = userDbo.Email,
            };

            return new SuccessResult<User>(user);
        }
    }
}