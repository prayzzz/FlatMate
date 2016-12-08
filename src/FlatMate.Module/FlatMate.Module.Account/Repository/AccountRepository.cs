using System;
using System.Collections.Generic;
using System.Linq;
using FlatMate.Common.Attributes;
using FlatMate.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using prayzzz.Common.Dbo;
using prayzzz.Common.Result;

namespace FlatMate.Module.Account.Repository
{
    [Inject(typeof(AccountRepository))]
    public class AccountRepository
    {
        private readonly AccountContext _context;

        public AccountRepository(AccountContext context)
        {
            _context = context;
        }

        public Result<T> Add<T>(T dbo, bool skipSave = false) where T : BaseDbo
        {
            _context.Add(dbo);
            return SaveChanges(skipSave).WithData(dbo);
        }

        public Result AddMany<T>(IEnumerable<T> dbos, bool skipSave = false) where T : BaseDbo
        {
            foreach (var dbo in dbos)
            {
                Add(dbo, true);
            }

            return SaveChanges(skipSave);
        }

        private void ApplyDates()
        {
            var now = DateTime.Now;
            foreach (var entry in _context.ChangeTracker.Entries().Where(x => x.State == EntityState.Added))
            {
                ((BaseDbo) entry.Entity).CreationDate = now;
                ((BaseDbo) entry.Entity).LastModified = now;
            }

            foreach (var entry in _context.ChangeTracker.Entries().Where(x => x.State == EntityState.Modified))
            {
                ((BaseDbo) entry.Entity).LastModified = now;
            }
        }

        public Result<T> GetById<T>(int listId) where T : BaseDbo
        {
            var itemListDbo = _context.Find<T>(listId);

            if (itemListDbo == null)
            {
                return new ErrorResult<T>(ErrorType.NotFound, "Entity not found");
            }

            return new SuccessResult<T>(itemListDbo);
        }

        public DbSet<T> GetSet<T>() where T : BaseDbo
        {
            return _context.Set<T>();
        }

        public Result Remove<T>(T dbo, bool skipSave = false) where T : BaseDbo
        {
            _context.Remove(dbo);
            return SaveChanges(skipSave);
        }

        public Result RemoveMany<T>(IEnumerable<T> dbos, bool skipSave = false) where T : BaseDbo
        {
            foreach (var dbo in dbos)
            {
                Remove(dbo, true);
            }

            return SaveChanges(skipSave);
        }

        public Result SaveChanges(bool skipSave = false)
        {
            if (skipSave)
            {
                return new SuccessResult();
            }

            ApplyDates();

            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.SaveChanges();
                    trans.Commit();

                    return new SuccessResult();
                }
                catch (Exception ex)
                {
                    trans.Rollback();

                    return new ErrorResult(ex, "Save failed");
                }
            }
        }

        public Result<T> Update<T>(T dbo, bool skipSave = false) where T : BaseDbo
        {
            _context.Update(dbo);
            return SaveChanges(skipSave).WithData(dbo);
        }

        public Result UpdateMany<T>(IEnumerable<T> dbos, bool skipSave = false) where T : BaseDbo
        {
            foreach (var dbo in dbos)
            {
                Update(dbo, true);
            }

            return SaveChanges(skipSave);
        }
    }
}