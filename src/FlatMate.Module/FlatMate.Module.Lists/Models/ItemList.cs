using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using prayzzz.Common.Mapping;

namespace FlatMate.Module.Lists.Models
{
    public class ItemList
    {
        public DateTime CreationDate { get; set; }

        public int UserId { get; set; }

        public string Description { get; set; } = string.Empty;

        public int Id { get; set; }
        
        public bool IsPublic { get; set; }

        public List<Item> Items { get; set; } = new List<Item>();

        public DateTime LastModified { get; set; }

        public List<ItemListGroup> ListGroups { get; set; } = new List<ItemListGroup>();

        [Required]
        public string Name { get; set; }
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

        private static ItemListDbo MapToDbo(ItemList listModel, MappingCtx ctx)
        {
            var listDbo = new ItemListDbo
            {
                CreationDate = listModel.CreationDate,
                UserId = listModel.UserId,
                Description = listModel.Description,
                Id = listModel.Id,
                IsPublic = listModel.IsPublic,
                Items = listModel.Items.Select(item => ctx.Mapper.Map<ItemDbo>(item)).ToList(),
                LastModified = listModel.LastModified,
                ListGroups = listModel.ListGroups.Select(group => ctx.Mapper.Map<ItemListGroupDbo>(@group)).ToList(),
                Name = listModel.Name
            };
            
            return listDbo;
        }

       private static ItemList MapToModel(ItemListDbo dbo, MappingCtx ctx)
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
    }
}