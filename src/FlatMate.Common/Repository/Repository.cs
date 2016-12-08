using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using prayzzz.Common.Dbo;
using prayzzz.Common.Result;

namespace FlatMate.Common.Repository
{
    public interface IRepository<T> where T : BaseDbo
    {
        void Add(T dbo);
        IQueryable<T> GetAll();
        Result<T> GetById(int id);
        void Remove<TDbo>(TDbo dbo) where TDbo : BaseDbo;
        Task<Result> SaveChanges();
        void Update(T dbo);
    }

    public abstract class Repository<T> : IRepository<T> where T : BaseDbo
    {
        protected readonly string EntityName;

        protected Repository()
        {
            EntityName = typeof(T).Name;
        }

        protected abstract DbContext Context { get; }

        protected abstract ILogger Logger { get; }

        public void Add(T dbo)
        {
            Context.Add(dbo);
        }

        public abstract IQueryable<T> GetAll();

        public virtual Result<T> GetById(int id)
        {
            Logger.LogInformation("Get {0} with id {1}", EntityName, id);

            var dbo = Context.Find<T>(id);
            if (dbo == null)
            {
                return new ErrorResult<T>(ErrorType.NotFound, $"{EntityName} #{id} not found");
            }

            return new SuccessResult<T>(dbo);
        }

        public void Remove<TDbo>(TDbo dbo) where TDbo : BaseDbo
        {
            Context.Remove(dbo);
        }

        public async Task<Result> SaveChanges()
        {
            ApplyDates();

            using (var trans = Context.Database.BeginTransaction())
            {
                try
                {
                    await Context.SaveChangesAsync();
                    trans.Commit();

                    return new SuccessResult();
                }
                catch (Exception ex)
                {
                    trans.Rollback();

                    Logger.LogError(0, ex, "Save failed");
                    return new ErrorResult(ex, "Save failed");
                }
            }
        }

        public void Update(T dbo)
        {
            Context.Update(dbo);
        }

        private void ApplyDates()
        {
            var now = DateTime.Now;
            foreach (var entry in Context.ChangeTracker.Entries().Where(x => x.State == EntityState.Added))
            {
                ((BaseDbo) entry.Entity).CreationDate = now;
                ((BaseDbo) entry.Entity).LastModified = now;
            }

            foreach (var entry in Context.ChangeTracker.Entries().Where(x => x.State == EntityState.Modified))
            {
                ((BaseDbo) entry.Entity).LastModified = now;
            }
        }
    }
}