using FlatMate.Common.Repository;
using FlatMate.Module.Account.Models;
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
        private readonly IMapper _mapper;
        private readonly IRepository<UserDbo> _repository;

        public UserService(IRepository<UserDbo> repository, IMapper mapper)
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