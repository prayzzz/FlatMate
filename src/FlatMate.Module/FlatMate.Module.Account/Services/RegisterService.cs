using System;
using System.Linq;
using System.Threading.Tasks;
using FlatMate.Common.Attributes;
using FlatMate.Module.Account.Models;
using FlatMate.Module.Account.Persistence.Dbo;
using prayzzz.Common.Result;

namespace FlatMate.Module.Account.Services
{
    public interface IRegisterService
    {
        Task<Result> CreateUserAsync(RegisterUser model);
    }

    [Inject(DependencyLifetime.Scoped)]
    public class RegisterService : IRegisterService
    {
        private readonly AccountContext _context;
        private readonly IPasswordService _passwordService;

        public RegisterService(AccountContext context, IPasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        public async Task<Result> CreateUserAsync(RegisterUser model)
        {
            if (model.Password != model.PasswordConfirmation)
            {
                return new ErrorResult("Passwords do not match");
            }

            var userExists = _context.User.Any(x => x.UserName == model.UserName || x.Email == model.Email);
            if (userExists)
            {
                return new ErrorResult("User already exists.");
            }

            var dbo = new UserDbo();
            dbo.Email = model.Email;
            dbo.LastName = model.LastName;
            dbo.Name = model.Name;
            dbo.UserName = model.UserName;
            dbo.Salt = _passwordService.CreateSalt();
            dbo.PasswordHash = _passwordService.HashPassword(dbo.Salt, model.Password);

            _context.Add(dbo);

            try
            {
                await _context.SaveChangesAsync();
                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex, "Saving failed.");
            }
        }
    }
}