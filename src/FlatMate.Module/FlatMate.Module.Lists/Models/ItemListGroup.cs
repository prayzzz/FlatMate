using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FlatMate.Module.Account.Models;
using FlatMate.Module.Account.Repository;
using prayzzz.Common.Dbo;
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

        [Editable(false)]
        public User User { get; set; }

        [Editable(false)]
        public int UserId { get; set; }
    }

    public class ItemListGroupDbo : OwnedDbo
    {
        [ForeignKey("ItemListId")]
        public ItemListDbo ItemList { get; set; }

        public int ItemListId { get; set; }

        [InverseProperty("ItemListGroup")]
        public List<ItemDbo> Items { get; set; } = new List<ItemDbo>();

        public string Name { get; set; }
    }

    public class ItemListGroupMapper : IDboMapper
    {
        private readonly UserRepository _userRepository;

        public ItemListGroupMapper(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void Configure(IMapperConfiguration mapper)
        {
            mapper.Configure<ItemListGroupDbo, ItemListGroup>(MapToModel);
            mapper.Configure<ItemListGroup, ItemListGroupDbo>(MapToDbo);
        }

        private static ItemListGroupDbo MapToDbo(ItemListGroup model, MappingContext ctx)
        {
            var groupDbo = new ItemListGroupDbo();
            groupDbo.CreationDate = model.CreationDate;
            groupDbo.Id = model.Id;
            groupDbo.ItemListId = model.ItemListId;
            groupDbo.Items = model.Items.Select(item => ctx.Mapper.Map<ItemDbo>(item)).ToList();
            groupDbo.LastModified = model.LastModified;
            groupDbo.Name = model.Name;
            groupDbo.UserId = model.UserId;

            return groupDbo;
        }

        private ItemListGroup MapToModel(ItemListGroupDbo groupDbo, MappingContext ctx)
        {
            var group = new ItemListGroup();
            group.CreationDate = groupDbo.CreationDate;
            group.Id = groupDbo.Id;
            group.ItemListId = groupDbo.ItemListId;
            group.LastModified = groupDbo.LastModified;
            group.Items = groupDbo.Items.Select(itemDbo => ctx.Mapper.Map<Item>(itemDbo)).ToList();
            group.Name = groupDbo.Name;
            group.UserId = groupDbo.UserId;
            group.User = ctx.Mapper.Map<User>(_userRepository.GetById(groupDbo.UserId));

            return group;
        }
    }
}