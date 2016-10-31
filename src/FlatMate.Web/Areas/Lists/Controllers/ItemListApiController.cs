using System;
using System.Collections.Generic;
using System.Linq;
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
        public Task<Result<Item>> AddItemToList(int listId, [FromBody] Item item)
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
        public Task<Result<ItemListGroup>> AddGroupToList(int listId, [FromBody] ItemListGroup group)
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
        public Task<Result<Item>> AddItemToGroup(int listId, int groupId, [FromBody] Item item)
        {
            item.UserId = CurrentUserId;
            item.ItemListId = listId;
            item.ItemListGroupId = groupId;

            var now = DateTime.Now;
            item.CreationDate = now;
            item.LastModified = now;

            return _listService.AddItemToGroup(listId, groupId, item);
        }

        [HttpPut("{listId}")]
        [Produces(typeof(ItemList))]
        public Task<Result<ItemList>> UpdateItemList(int listId, [FromBody]ItemList itemList)
        {
            itemList.UserId = CurrentUserId;
            itemList.Id = listId;
            itemList.LastModified = DateTime.Now;

            return _listService.UpdateItemList(listId, itemList);
        }

        [HttpPut("{listId}/group/{groupId}/item/{itemId}")]
        [Produces(typeof(ItemList))]
        public Task<Result<Item>> UpdateItemInGroup(int listId, int groupId, int itemId, [FromBody] Item item)
        {
            item.UserId = CurrentUserId;
            item.Id = itemId;
            item.ItemListGroupId = groupId;
            item.ItemListId = listId;

            item.LastModified = DateTime.Now;

            return _listService.UpdateItemInGroup(listId, groupId, itemId, item);
        }

        [HttpDelete("{listId}/group/{groupId}/item/{itemId}")]
        public Task<Result> DeleteItemFromGroup(int listId, int groupId, int itemId)
        {
            return _listService.DeleteItemFromGroup(listId, groupId, itemId);
        }

        [HttpDelete("{listId}/group/{groupId}")]
        public Task<Result> DeleteGroupFromList(int listId, int groupId)
        {
            return _listService.DeleteGroupFromList(listId, groupId);
        }

        [HttpDelete("{listId}")]
        public Task<Result> UpdateItemInGroup(int listId)
        {
            return _listService.DeletList(listId);
        }

        [HttpGet("{id}")]
        [Produces(typeof(ItemList))]
        public Result<ItemList> GetById(int id, [FromQuery]int user = 0)
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

        /// <summary> 
        /// </summary>
        /// <param name="isPublic">Can only be set, if called from code</param>
        /// <param name="userId"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(List<ItemList>))]
        public List<ItemList> GetAll(bool? isPublic = null, [FromQuery]int? userId = null, [FromQuery]int? limit = null)
        {
            // show all, if requesting own lists
            if (isPublic == null && CurrentUserId != userId)
            {
                isPublic = true;
            }

            var query = new ItemListQuery
            {
                IsPublic = isPublic,
                UserId = userId,
                Limit = limit
            };

            return _listService.GetAll(query);
        }

        public Task<Result> Delete(int id)
        {
            return _listService.DeleteItemList(id);
        }
    }
}