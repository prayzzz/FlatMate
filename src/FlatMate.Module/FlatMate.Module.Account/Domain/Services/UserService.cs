using FlatMate.Common.Attributes;
using FlatMate.Module.Account.Domain.Entities;
using FlatMate.Module.Account.Persistence;
using FlatMate.Module.Account.Persistence.Repositories;
using prayzzz.Common.Mapping;
using prayzzz.Common.Mvc.Services;
using prayzzz.Common.Result;

namespace FlatMate.Module.Account.Domain.Services
{
    public interface IUserService
    {
        Result<User> GetCurrentUser();
        Result<User> GetUser(int id);
    }

    [Inject(DependencyLifetime.Scoped)]
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly UserRepository _repository;
        private readonly IRequestService _requestService;

        public UserService(UserRepository repository, IRequestService requestService, IMapper mapper)
        {
            _repository = repository;
            _requestService = requestService;
            _mapper = mapper;
        }

        public Result<User> GetCurrentUser()
        {
            var getById = _repository.GetById(_requestService.CurrentUserId);
            if (getById == null)
            {
                return new ErrorResult<User>($"No entity with id {_requestService.CurrentUserId}");
            }

            var user = _mapper.Map<User>(getById.Data);
            return new SuccessResult<User>(user);
        }

        public Result<User> GetUser(int id)
        {
            var getById = _repository.GetById(id);
            if (getById == null)
            {
                return new ErrorResult<User>($"No entity with id {id}");
            }

            var user = _mapper.Map<User>(getById.Data);
            return new SuccessResult<User>(user);
        }
    }
}