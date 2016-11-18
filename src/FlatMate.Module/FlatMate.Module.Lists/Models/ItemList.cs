using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FlatMate.Common;
using FlatMate.Common.Repository;
using FlatMate.Module.Account.Models;
using FlatMate.Module.Lists.Services;
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

    public class ItemList : PrivilegedModel
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
        public User User { get; set; }

        [Editable(false)]
        public int UserId { get; set; }
    }

    [Table("ItemList")]
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
        private readonly ItemListPrivileger _privileger;
        private readonly IRepository<UserDbo> _userRepository;

        public ItemListMapper(IRepository<UserDbo> userRepository, ItemListPrivileger privileger)
        {
            _userRepository = userRepository;
            _privileger = privileger;
        }

        public void Configure(IMapperConfiguration mapper)
        {
            mapper.Configure<ItemListDbo, ItemList>(MapToModel);
            mapper.Configure<ItemList, ItemListDbo>(MapToDbo);
        }

        private static ItemListDbo MapToDbo(ItemList model, ItemListDbo dbo, MappingContext ctx)
        {
            dbo.UserId = model.UserId;
            dbo.Id = model.Id;
            dbo.IsPublic = model.IsPublic;
            dbo.Name = model.Name;

            if (model.CreationDate.HasValue) dbo.CreationDate = model.CreationDate.Value;
            if (model.Description != null) dbo.Description = model.Description;
            if (model.Items.Any()) dbo.Items = model.Items.Select(item => ctx.Mapper.Map<ItemDbo>(item)).ToList();
            if (model.LastModified.HasValue) dbo.LastModified = model.LastModified.Value;
            if (model.ListGroups.Any()) dbo.ListGroups = model.ListGroups.Select(group => ctx.Mapper.Map<ItemListGroupDbo>(group)).ToList();

            return dbo;
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
            itemList.User = ctx.Mapper.Map<User>(_userRepository.GetById(dbo.UserId).Data);
            itemList.Privileges = _privileger.GetPrivileges(dbo);

            return itemList;
        }
    }
}