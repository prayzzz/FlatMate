using System.Linq;
using FlatMate.Common.Extensions;
using FlatMate.Module.Account.Domain.Services;
using FlatMate.Module.Lists.Domain.Entities;
using FlatMate.Module.Lists.Domain.Services;
using FlatMate.Web.Areas.Lists.Dto;
using FlatMate.Web.Common.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prayzzz.Common.Mapping;
using prayzzz.Common.Result;

namespace FlatMate.Web.Areas.Lists.Controllers
{
    [Route("api/v1/lists/itemlist")]
    public class ItemListApiController : ApiController
    {
        private readonly IItemListService _listService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public ItemListApiController(IHttpContextAccessor context, IItemListService listService, IUserService userService, IMapper mapper) : base(context)
        {
            _listService = listService;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost]
        [Produces(typeof(ListDto))]
        public Result<ListDto> CreateList([FromBody] ListUpdateDto listDto)
        {
            var currentUser = _userService.GetCurrentUser();
            if (!currentUser.IsSuccess)
            {
                return new ErrorResult<ListDto>(ErrorType.Unauthorized, "Unauthorized");
            }

            var itemList = new ItemList(0, listDto.Name, currentUser.Data);
            itemList = _mapper.Map(listDto, itemList, new MappingContext().PutParam(nameof(IUserService), _userService));

            return _listService.Create(itemList).WithDataAs(entity => _mapper.Map<ListDto>(entity));
        }

        [HttpPost("{listId}/group")]
        [Produces(typeof(ListDto))]
        public Result<ListDto> AddGroupToList(int listId, [FromBody] GroupUpdateDto groupDto)
        {
            var currentUser = _userService.GetCurrentUser();
            if (!currentUser.IsSuccess)
            {
                return new ErrorResult<ListDto>(ErrorType.Unauthorized, "Unauthorized");
            }

            var getById = _listService.GetById(listId);
            if (!getById.IsSuccess)
            {
                return new ErrorResult<ListDto>(getById);
            }

            var list = getById.Data;
            var group = _mapper.Map(groupDto, new ItemListGroup(0, groupDto.Name, currentUser.Data));

            var addGroup = list.AddGroup(group);
            if (!addGroup.IsSuccess)
            {
                return new ErrorResult<ListDto>(addGroup);
            }

            var update = _listService.Update(list);
            if (!update.IsSuccess)
            {
                return new ErrorResult<ListDto>(update);
            }

            return new SuccessResult<ListDto>(_mapper.Map<ListDto>(update.Data));
        }

        [HttpDelete("{listId}")]
        [Produces(typeof(IActionResult))]
        public Result DeleteList(int listId)
        {
            return _listService.Delete(listId);
        }

        [HttpGet("{id}")]
        [Produces(typeof(ListDto))]
        public Result<ListDto> GetList(int id)
        {
            return _listService.GetById(id).WithDataAs(entity => _mapper.Map<ListDto>(entity));
        }

        [HttpPut("{listId}")]
        [Produces(typeof(ListDto))]
        public Result<ListDto> UpdateList(int listId, [FromBody] ListUpdateDto listDto)
        {
            var getById = _listService.GetById(listId);
            if (!getById.IsSuccess)
            {
                return new ErrorResult<ListDto>(getById);
            }

            var itemList = _mapper.Map(listDto, getById.Data, new MappingContext().PutParam(nameof(IUserService), _userService));
            return _listService.Update(itemList).WithDataAs(entity => _mapper.Map<ListDto>(entity));
        }
    }
}