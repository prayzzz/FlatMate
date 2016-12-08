using System.Linq;
using FlatMate.Common.Attributes;
using FlatMate.Common.Extensions;
using FlatMate.Module.Lists.Domain.Entities;
using FlatMate.Module.Lists.Persistence.Dbo;
using FlatMate.Module.Lists.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using prayzzz.Common.Mapping;
using prayzzz.Common.Result;

namespace FlatMate.Module.Lists.Persistence
{
    [Inject(typeof(ItemListPersistence))]
    public class ItemListPersistence
    {
        private readonly IMapper _mapper;
        private readonly ListRepository _repository;

        public ItemListPersistence(ListRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Result<ItemList> Add(ItemList itemList)
        {
            var listDbo = _mapper.Map<ItemListDbo>(itemList);
            return _repository.Add(listDbo).WithDataAs(dbo => _mapper.Map<ItemList>(dbo));
        }

        public Result<ItemList> GetById(int id)
        {
            var listDbo = _repository.GetSet<ItemListDbo>()
                                     .Include(x => x.Groups).ThenInclude(x => x.Items)
                                     .FirstOrDefault(x => x.Id == id);

            if (listDbo == null)
            {
                return new ErrorResult<ItemList>(ErrorType.NotFound, "Entity not found");
            }

            return new SuccessResult<ItemList>(_mapper.Map<ItemList>(listDbo));
        }

        public Result Remove(int id)
        {
            var listDbo = _repository.GetSet<ItemListDbo>()
                                     .Include(x => x.Groups).ThenInclude(x => x.Items)
                                     .FirstOrDefault(x => x.Id == id);

            if (listDbo == null)
            {
                return new ErrorResult(ErrorType.NotFound, "Entity not found");
            }

            return _repository.Remove(listDbo);
        }

        public Result<ItemList> Update(ItemList itemList)
        {
            var listDboResult = _repository.GetById<ItemListDbo>(itemList.Id);
            if (!listDboResult.IsSuccess)
            {
                return new ErrorResult<ItemList>(ErrorType.NotFound, "Entity not found");
            }

            var listDbo = _mapper.Map(itemList, listDboResult.Data);

            var update = _repository.Update(listDbo);
            if (!update.IsSuccess)
            {
                return new ErrorResult<ItemList>(update);
            }

            return GetById(listDbo.Id);
        }
    }
}