using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlatMate.Common.Repository;
using FlatMate.Module.Home.Models;
using prayzzz.Common.Mapping;
using prayzzz.Common.Result;

namespace FlatMate.Module.Home.Service
{
    public interface IDashboardService
    {
        Task<Result<DashboardEntry>> CreateAsync(DashboardEntry entry);
        IEnumerable<DashboardEntry> GetAll();
    }

    public class DashboardService : IDashboardService
    {
        private readonly IRepository<DashboardEntryDbo> _repository;
        private readonly IMapper _mapper;

        public DashboardService(IRepository<DashboardEntryDbo> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<DashboardEntry>> CreateAsync(DashboardEntry entry)
        {
            var entryDbo = _mapper.Map<DashboardEntryDbo>(entry);

            _repository.Add(entryDbo);
            return await SaveAsync<DashboardEntry>(entryDbo);
        }

        private async Task<Result<TModel>> SaveAsync<TModel>(object itemDbo) where TModel : class
        {
            var result = await _repository.Save();
            if (!result.IsSuccess)
            {
                return new ErrorResult<TModel>(result);
            }

            return new SuccessResult<TModel>(_mapper.Map<TModel>(itemDbo));
        }

        public IEnumerable<DashboardEntry> GetAll()
        {
            return _repository.GetAll().Select(x => _mapper.Map<DashboardEntry>(x));
        }
    }
}