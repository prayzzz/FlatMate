using System.Linq;
using FlatMate.Module.Account.Models;

namespace FlatMate.Module.Account.Repository
{
    public class UserRepository
    {
        private readonly AccountContext _context;

        public UserRepository(AccountContext context)
        {
            _context = context;
        }

        public UserDbo GetById(int id)
        {
            return _context.User.FirstOrDefault(x => x.Id == id);
        }
    }
}