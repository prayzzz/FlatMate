using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlatMate.Module.Lists.Models;
using FlatMate.Module.Lists.Services;
using FlatMate.Web.Common.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prayzzz.Common.Result;

namespace FlatMate.Web.Areas.Lists.Controllers
{
    [Authorize]
    [Route("api/v1/lists/itemlist")]
    public class ItemListApiController : ApiController
    {
        private readonly IListService _listService;

        public ItemListApiController(IHttpContextAccessor context, IListService listService) : base(context)
        {
            _listService = listService;
        }

        [HttpPost]
        [Produces(typeof(ItemList))]
        public Task<Result<ItemList>> CreateList([FromBody] ItemList itemlist)
        {
            itemlist.Id = 0;
            itemlist.UserId = CurrentUserId;

            var now = DateTime.Now;
            itemlist.CreationDate = now;
            itemlist.LastModified = now;
            itemlist.ListGroups.ForEach(group =>
            {
                group.CreationDate = now;
                group.LastModified = now;

                group.Items.ForEach(item =>
                {
                    item.CreationDate = now;
                    item.LastModified = now;
                });
            });
            itemlist.Items.ForEach(item =>
            {
                item.CreationDate = now;
                item.LastModified = now;
            });

            return _listService.Create(itemlist);
        }

        [HttpPost("{listId}/item")]
        [Produces(typeof(ItemList))]
        public Task<Result<ItemList>> AddItemToList(int listId, [FromBody] Item item)
        {
            item.UserId = CurrentUserId;
            item.ItemListId = listId;
            item.ItemListGroupId = null;

            var now = DateTime.Now;
            item.CreationDate = now;
            item.LastModified = now;

            return _listService.AddItemToList(listId, item);
        }

        [HttpPost("{listId}/group")]
        [Produces(typeof(ItemList))]
        public Task<Result<ItemList>> AddGroupToList(int listId, [FromBody] ItemListGroup group)
        {
            group.UserId = CurrentUserId;
            group.ItemListId = listId;

            var now = DateTime.Now;
            group.CreationDate = now;
            group.LastModified = now;

            return _listService.AddGroupToList(listId, group);
        }

        [HttpPost("{listId}/group/{groupId}/item")]
        [Produces(typeof(ItemList))]
        public Task<Result<ItemList>> AddItemToGroup(int listId, int groupId, [FromBody] Item item)
        {
            item.UserId = CurrentUserId;
            item.ItemListId = listId;
            item.ItemListGroupId = groupId;

            var now = DateTime.Now;
            item.CreationDate = now;
            item.LastModified = now;

            return _listService.AddItemToGroup(listId, groupId, item);
        }

        [HttpGet]
        [Produces(typeof(List<ItemList>))]
        public Result<List<ItemList>> GetAll()
        {
            return _listService.GetAll();
        }

        [HttpGet]
        [Produces(typeof(ItemList))]
        public Result<ItemList> GetById(int id)
        {
            var result = _listService.GetById(id);

            if (!result.IsSuccess)
            {
                return result;
            }

            if (!result.Data.IsPublic && result.Data.UserId != CurrentUserId)
            {
                return new ErrorResult<ItemList>(ErrorType.Unauthorized, "You are now allowed to view this.");
            }

            return result;
        }

        [HttpGet]
        [Produces(typeof(List<ItemList>))]
        public Result<List<ItemList>> GetAllByUser(int userId)
        {
            if (CurrentUserId == userId)
            {
                return _listService.GetAllByUser(userId);
            }

            return _listService.GetAllPublicByUser(userId);
        }
    }
}