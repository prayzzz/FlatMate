using System.Collections.Generic;
using System.Threading.Tasks;
using FlatMate.Common.Attributes;
using FlatMate.Common.Repository;
using FlatMate.Module.Home.Models;
using prayzzz.Common.Result;

namespace FlatMate.Module.Home.Service
{
    public interface IDashboardService
    {
        Task<Result<DashboardEntryDbo>> CreateAsync(DashboardEntryDbo entry);
        IEnumerable<DashboardEntryDbo> GetAll();
    }

    [Inject(DependencyLifetime.Scoped)]
    public class DashboardService : IDashboardService
    {
        private readonly IRepository<DashboardEntryDbo> _repository;

        public DashboardService(IRepository<DashboardEntryDbo> repository)
        {
            _repository = repository;
        }

        public async Task<Result<DashboardEntryDbo>> CreateAsync(DashboardEntryDbo dbo)
        {
            _repository.Add(dbo);

            return await SaveAsync(dbo);
        }

        public IEnumerable<DashboardEntryDbo> GetAll()
        {
            return _repository.GetAll();
        }

        private async Task<Result<TDbo>> SaveAsync<TDbo>(TDbo itemDbo) where TDbo : class
        {
            var result = await _repository.SaveChanges();
            if (!result.IsSuccess)
            {
                return new ErrorResult<TDbo>(result);
            }

            return new SuccessResult<TDbo>(itemDbo);
        }
    }
}