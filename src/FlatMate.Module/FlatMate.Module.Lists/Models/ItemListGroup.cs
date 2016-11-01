using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using prayzzz.Common.Mapping;

namespace FlatMate.Module.Lists.Models
{
    public class ItemListGroup
    {
        [Editable(false)]
        public DateTime CreationDate { get; set; }

        public int Id { get; set; }

        [Required]
        public int ItemListId { get; set; }

        public List<Item> Items { get; set; } = new List<Item>();


        [Editable(false)]
        public DateTime LastModified { get; set; }

        [Required]
        public string Name { get; set; }

        public int UserId { get; set; }
    }

    public class ItemListGroupDbo
    {
        public DateTime CreationDate { get; set; }

        [Key]
        public int Id { get; set; }

        [ForeignKey("ItemListId")]
        public ItemListDbo ItemList { get; set; }

        public int ItemListId { get; set; }

        [InverseProperty("ItemListGroup")]
        public List<ItemDbo> Items { get; set; } = new List<ItemDbo>();

        public DateTime LastModified { get; set; }

        public string Name { get; set; }

        public int UserId { get; set; }
    }

    public class ItemListGroupMapper : IDboMapper
    {
        public void Configure(IMapperConfiguration mapper)
        {
            mapper.Configure<ItemListGroupDbo, ItemListGroup>(MapToModel);
            mapper.Configure<ItemListGroup, ItemListGroupDbo>(MapToDbo);
        }

        private static ItemListGroupDbo MapToDbo(ItemListGroup model, MappingContext ctx)
        {
            var groupDbo = new ItemListGroupDbo
            {
                CreationDate = model.CreationDate,
                Id = model.Id,
                ItemListId = model.ItemListId,
                Items = model.Items.Select(item => ctx.Mapper.Map<ItemDbo>(item)).ToList(),
                LastModified = model.LastModified,
                Name = model.Name,
                UserId = model.UserId
            };

            return groupDbo;
        }

        private static ItemListGroup MapToModel(ItemListGroupDbo groupDbo, MappingContext ctx)
        {
            var group = new ItemListGroup
            {
                CreationDate = groupDbo.CreationDate,
                Id = groupDbo.Id,
                ItemListId = groupDbo.ItemListId,
                LastModified = groupDbo.LastModified,
                Items = groupDbo.Items.Select(itemDbo => ctx.Mapper.Map<Item>(itemDbo)).ToList(),
                Name = groupDbo.Name,
                UserId = groupDbo.UserId
            };

            return group;
        }
    }
}