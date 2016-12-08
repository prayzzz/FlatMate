using FlatMate.Common;

namespace FlatMate.Module.Account.Domain.Entities
{
    public class User : Entity
    {
        public User(int id) : base(id)
        {
        }

        public string EMail { get; set; }

        public string LastName { get; set; }

        public string Name { get; set; }

        public string UserName { get; set; }
    }
}