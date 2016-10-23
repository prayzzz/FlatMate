using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        public int UserId { get; set; }

        public string Value { get; set; }
    }

    public class ItemDbo
    {
        public DateTime CreationDate { get; set; }

        [Key]
        public int Id { get; set; }

        [ForeignKey("ItemListId")]
        public ItemListDbo ItemList { get; set; }

        [ForeignKey("ItemListGroupId")]
        public ItemListGroupDbo ItemListGroup { get; set; }

        public int? ItemListGroupId { get; set; }

        public int? ItemListId { get; set; }

        [Editable(false)]
        public DateTime LastModified { get; set; }

        public int UserId { get; set; }

        [Required]
        public string Value { get; set; }
    }

    public class ItemMapper : IDboMapper
    {
        public void Configure(IMapperConfiguration mapper)
        {
            mapper.Configure<ItemDbo, Item>(MapToModel);
            mapper.Configure<Item, ItemDbo>(MapToDbo);
        }

        private static ItemDbo MapToDbo(Item model, ItemDbo itemDbo, MappingCtx ctx)
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

        private static Item MapToModel(ItemDbo itemDbo, MappingCtx ctx)
        {
            var group = new Item
            {
                CreationDate = itemDbo.CreationDate,
                Id = itemDbo.Id,
                ItemListId = itemDbo.ItemListId,
                ItemListGroupId = itemDbo.ItemListGroupId,
                LastModified = itemDbo.LastModified,
                UserId = itemDbo.UserId,
                Value = itemDbo.Value
            };

            return group;
        }
    }
}