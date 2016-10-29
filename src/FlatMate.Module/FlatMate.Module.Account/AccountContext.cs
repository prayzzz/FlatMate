using FlatMate.Module.Account.Models;
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

            builder.Entity<UserDbo>().ToTable("Account_User");
        }
    }
}
