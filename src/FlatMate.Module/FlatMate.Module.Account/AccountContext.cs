using FlatMate.Module.Account.Persistence.Dbo;
using Microsoft.EntityFrameworkCore;

namespace FlatMate.Module.Account
{
    public class AccountContext : DbContext
    {
        public AccountContext(DbContextOptions<AccountContext> options)
            : base(options)
        {
        }

        public DbSet<UserDbo> User { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("Account");
        }
    }
}