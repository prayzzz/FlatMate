using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FlatMate.Module.Account.Models;
using FlatMate.Module.Account.Repository;
using prayzzz.Common.Dbo;
using prayzzz.Common.Mapping;

namespace FlatMate.Module.Lists.Models
{
    public class Item
    {
        [Editable(false)]
        public DateTime CreationDate { get; set; }

        public int Id { get; set; }

        public int? ItemListGroupId { get; set; }

        public int? ItemListId { get; set; }

        [Editable(false)]
        public DateTime LastModified { get; set; }

        [Editable(false)]
        public int UserId { get; set; }

        [Editable(false)]
        public User User { get; set; }

        public string Value { get; set; }
    }

    public class ItemDbo : OwnedDbo
    {
        [ForeignKey("ItemListId")]
        public ItemListDbo ItemList { get; set; }

        [ForeignKey("ItemListGroupId")]
        public ItemListGroupDbo ItemListGroup { get; set; }

        public int? ItemListGroupId { get; set; }

        public int? ItemListId { get; set; }

        [Required]
        public string Value { get; set; }
    }

    public class ItemMapper : IDboMapper
    {
        private readonly UserRepository _userRepository;

        public ItemMapper(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void Configure(IMapperConfiguration mapper)
        {
            mapper.Configure<ItemDbo, Item>(MapToModel);
            mapper.Configure<Item, ItemDbo>(MapToDbo);
        }

        private static ItemDbo MapToDbo(Item model, ItemDbo itemDbo, MappingContext ctx)
        {
            itemDbo.CreationDate = model.CreationDate;
            itemDbo.Id = model.Id;
            itemDbo.ItemListId = model.ItemListId;
            itemDbo.ItemListGroupId = model.ItemListGroupId;
            itemDbo.LastModified = model.LastModified;
            itemDbo.UserId = model.UserId;
            itemDbo.Value = model.Value;

            return itemDbo;
        }

        private Item MapToModel(ItemDbo itemDbo, MappingContext ctx)
        {
            var item = new Item();
            item.CreationDate = itemDbo.CreationDate;
            item.Id = itemDbo.Id;
            item.ItemListId = itemDbo.ItemListId;
            item.ItemListGroupId = itemDbo.ItemListGroupId;
            item.LastModified = itemDbo.LastModified;
            item.User = ctx.Mapper.Map<User>(_userRepository.GetById(itemDbo.UserId));
            item.UserId = itemDbo.UserId;
            item.Value = itemDbo.Value;

            return item;
        }
    }
}