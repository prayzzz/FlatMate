using System.Linq;
using System.Threading.Tasks;
using FlatMate.Module.Account.Models;
using prayzzz.Common.Result;

namespace FlatMate.Module.Account.Services
{
    public class LoginService : ILoginService
    {
        private readonly AccountContext _context;
        private readonly IPasswordService _passwordService;

        public LoginService(AccountContext context, IPasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }


        public async Task<Result<int>> LoginAllowed(LoginUser model)
        {
            var user = _context.User.FirstOrDefault(x => x.UserName == model.UserName);
            if (user == null)
            {
                return new ErrorResult<int>("Wrong Login");
            }

            if (!_passwordService.VerifyPassword(user.Salt, model.Password, user.PasswordHash))
            {
                return new ErrorResult<int>("Wrong Login");
            }

            return new SuccessResult<int>(user.Id);
        }
    }

    public interface ILoginService
    {
        Task<Result<int>> LoginAllowed(LoginUser model);
    }
}