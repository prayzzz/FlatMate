using System.Collections.Generic;
using FlatMate.Common.Attributes;
using FlatMate.Module.Home.Persistence.Dbo;
using FlatMate.Module.Home.Persistence.Repositories;
using prayzzz.Common.Result;

namespace FlatMate.Module.Home.Service
{
    public interface IDashboardService
    {
        Result<DashboardEntryDbo> Create(DashboardEntryDbo dbo);
        IEnumerable<DashboardEntryDbo> GetAll();
    }

    [Inject(DependencyLifetime.Scoped)]
    public class DashboardService : IDashboardService
    {
        private readonly DashboardEntryRepository _repository;

        public DashboardService(DashboardEntryRepository repository)
        {
            _repository = repository;
        }

        public Result<DashboardEntryDbo> Create(DashboardEntryDbo dbo)
        {
            return _repository.Add(dbo);
        }

        public IEnumerable<DashboardEntryDbo> GetAll()
        {
            return _repository.GetAll();
        }
    }
}