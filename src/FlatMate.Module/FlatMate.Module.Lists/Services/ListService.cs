using System.Collections.Generic;
using System.Linq;
using FlatMate.Module.Lists.Models;
using prayzzz.Common.Result;

namespace FlatMate.Module.Lists.Services
{
    public interface IListService
    {}

    public class ListService : IListService
    {
        private readonly ListsContext _context;

        public ListService(ListsContext context)
        {
            _context = context;
        }

        public Result<List<ItemList>> GetListsByUser(int id)
        {
            var listDbos = _context.List.Where(x => x.UserId == id);

            return null;
        }
    }

}