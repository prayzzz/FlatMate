using FlatMate.Module.Account.Models;
using FlatMate.Module.Account.Repository;
using prayzzz.Common.Mapping;
using prayzzz.Common.Result;

namespace FlatMate.Module.Account.Services
{
    public interface IUserService
    {
        Result<User> GetUser(int id);
    }

    public class UserService : IUserService
    {
        private readonly UserRepository _repository;
        private readonly IMapper _mapper;

        public UserService(UserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Result<User> GetUser(int id)
        {
            var userDbo = _repository.GetById(id);
            if (userDbo == null)
            {
                return new ErrorResult<User>($"No entity with id {id}");
            }

            var user = _mapper.Map<User>(userDbo);
            return new SuccessResult<User>(user);
        }
    }
}