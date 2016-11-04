using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FlatMate.Module.Account.Models;
using FlatMate.Module.Account.Repository;
using prayzzz.Common.Dbo;
using prayzzz.Common.Enums;
using prayzzz.Common.Mapping;

namespace FlatMate.Module.Lists.Models
{
    public class ItemListQuery
    {
        public OrderingDirection Direction { get; set; } = OrderingDirection.Asc;
        public bool? IsPublic { get; set; }

        public ItemListQueryOrder Order { get; set; } = ItemListQueryOrder.None;

        public int? UserId { get; set; }
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
        public User User { get; set; }
    }

    public class ItemListDbo : OwnedDbo
    {
        public string Description { get; set; }

        public bool IsPublic { get; set; }

        [InverseProperty("ItemList")]
        public List<ItemDbo> Items { get; set; } = new List<ItemDbo>();


        [InverseProperty("ItemList")]
        public List<ItemListGroupDbo> ListGroups { get; set; } = new List<ItemListGroupDbo>();

        public string Name { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }

    public class ItemListMapper : IDboMapper
    {
        private readonly UserRepository _userRepository;

        public ItemListMapper(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

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

        private ItemList MapToModel(ItemListDbo dbo, MappingContext ctx)
        {
            var itemList = new ItemList();
            itemList.CreationDate = dbo.CreationDate;
            itemList.Description = dbo.Description;
            itemList.Id = dbo.Id;
            itemList.IsPublic = dbo.IsPublic;
            itemList.Items = dbo.Items.Select(itemDbo => ctx.Mapper.Map<Item>(itemDbo)).ToList();
            itemList.LastModified = dbo.LastModified;
            itemList.ListGroups = dbo.ListGroups.Select(groupDbo => ctx.Mapper.Map<ItemListGroup>(groupDbo)).ToList();
            itemList.Name = dbo.Name;
            itemList.UserId = dbo.UserId;
            itemList.User = ctx.Mapper.Map<User>(_userRepository.GetById(dbo.UserId));

            return itemList;
        }
    }
}