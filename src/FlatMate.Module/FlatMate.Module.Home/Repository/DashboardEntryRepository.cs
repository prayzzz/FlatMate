﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlatMate.Common.Repository;
using FlatMate.Module.Home.Models;
using prayzzz.Common.Dbo;
using prayzzz.Common.Result;

namespace FlatMate.Module.Home.Repository
{
    public class DashboardEntryRepository : IRepository<DashboardEntryDbo>
    {
        private readonly List<DashboardEntryDbo> _entries;

        public DashboardEntryRepository()
        {
            _entries = new List<DashboardEntryDbo>();
        }

        public Result<DashboardEntryDbo> GetById(int id)
        {
            var dbo = _entries.FirstOrDefault(x => x.Id == id);
            if (dbo == null)
            {
                return new ErrorResult<DashboardEntryDbo>(ErrorType.NotFound, "Entity not found");
            }

            return new SuccessResult<DashboardEntryDbo>(dbo);
        }

        public void Add(DashboardEntryDbo dbo)
        {
            _entries.Add(dbo);
        }

        public void Remove<TDbo>(TDbo dbo) where TDbo : BaseDbo
        {
            throw new System.NotImplementedException();
        }

        public Task<Result> Save()
        {
            return Task.FromResult((Result)new SuccessResult());
        }

        public IQueryable<DashboardEntryDbo> GetAll()
        {
            return _entries.AsQueryable();
        }
    }
}