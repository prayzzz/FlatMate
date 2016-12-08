using System.ComponentModel.DataAnnotations.Schema;
using prayzzz.Common.Dbo;

namespace FlatMate.Module.Account.Persistence.Dbo
{
    [Table("User")]
    public class UserDbo : BaseDbo
    {
        public string Email { get; set; }

        public string LastName { get; set; }

        public string Name { get; set; }

        public string PasswordHash { get; set; }

        public string Salt { get; set; }

        public string UserName { get; set; }
    }
}