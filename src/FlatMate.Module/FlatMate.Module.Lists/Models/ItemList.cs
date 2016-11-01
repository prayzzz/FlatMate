using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using prayzzz.Common.Enums;
using prayzzz.Common.Linq;
using prayzzz.Common.Mapping;

namespace FlatMate.Module.Lists.Models
{
    public class ItemListQuery
    {
        public bool? IsPublic { get; set; }

        public int? UserId { get; set; }

        public ItemListQueryOrder Order { get; set; } = ItemListQueryOrder.None;

        public OrderingDirection Direction { get; set; } = OrderingDirection.Asc;
    }

    public enum ItemListQueryOrder
    {
        None,
        LastModified
    }

    public class ItemList
    {
        [Editable(false)]
        public DateTime? CreationDate { get; set; }

        [Display(Name = "Beschreibung")]
        public string Description { get; set; }

        [Editable(false)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Öffentlich?")]
        public bool IsPublic { get; set; }

        /// <summary>
        /// Is ignored, if empty
        /// </summary>
        public List<Item> Items { get; set; } = new List<Item>();

        [Editable(false)]
        public DateTime? LastModified { get; set; }

        /// <summary>
        /// Is ignored, if empty
        /// </summary>
        public List<ItemListGroup> ListGroups { get; set; } = new List<ItemListGroup>();

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Editable(false)]
        public int UserId { get; set; }
    }

    public class ItemListDbo
    {
        public DateTime CreationDate { get; set; }

        public string Description { get; set; }

        [Key]
        public int Id { get; set; }

        public bool IsPublic { get; set; }

        [InverseProperty("ItemList")]
        public List<ItemDbo> Items { get; set; } = new List<ItemDbo>();

        public DateTime LastModified { get; set; }

        [InverseProperty("ItemList")]
        public List<ItemListGroupDbo> ListGroups { get; set; } = new List<ItemListGroupDbo>();

        public string Name { get; set; }

        public int UserId { get; set; }
    }

    public class ItemListMapper : IDboMapper
    {
        public void Configure(IMapperConfiguration mapper)
        {
            mapper.Configure<ItemListDbo, ItemList>(MapToModel);
            mapper.Configure<ItemList, ItemListDbo>(MapToDbo);
        }

        private static ItemListDbo MapToDbo(ItemList listModel, ItemListDbo listDbo, MappingContext ctx)
        {
            listDbo.UserId = listModel.UserId;
            listDbo.Id = listModel.Id;
            listDbo.IsPublic = listModel.IsPublic;
            listDbo.Name = listModel.Name;

            if (listModel.CreationDate.HasValue) listDbo.CreationDate = listModel.CreationDate.Value;
            if (listModel.Description != null) listDbo.Description = listModel.Description;
            if (listModel.Items.Any()) listDbo.Items = listModel.Items.Select(item => ctx.Mapper.Map<ItemDbo>(item)).ToList();
            if (listModel.LastModified.HasValue) listDbo.LastModified = listModel.LastModified.Value;
            if (listModel.ListGroups.Any()) listDbo.ListGroups = listModel.ListGroups.Select(group => ctx.Mapper.Map<ItemListGroupDbo>(group)).ToList();

            return listDbo;
        }

        private static ItemList MapToModel(ItemListDbo dbo, MappingContext ctx)
        {
            var itemList = new ItemList
            {
                CreationDate = dbo.CreationDate,
                UserId = dbo.UserId,
                Description = dbo.Description,
                Id = dbo.Id,
                IsPublic = dbo.IsPublic,
                Items = dbo.Items.Select(itemDbo => ctx.Mapper.Map<Item>(itemDbo)).ToList(),
                LastModified = dbo.LastModified,
                ListGroups = dbo.ListGroups.Select(groupDbo => ctx.Mapper.Map<ItemListGroup>(groupDbo)).ToList(),
                Name = dbo.Name
            };

            return itemList;
        }

        private static void MapIf(Func<bool> condition, Action action)
        {
            if (condition())
            {
                action();
            }
        }
    }
}