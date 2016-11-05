using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FlatMate.Common;
using FlatMate.Module.Account.Models;
using FlatMate.Module.Account.Repository;
using FlatMate.Module.Lists.Services;
using prayzzz.Common.Dbo;
using prayzzz.Common.Mapping;

namespace FlatMate.Module.Lists.Models
{
    public class ItemListGroup : PrivilegedModel
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
        private readonly ItemListPrivileger _privileger;

        public ItemListGroupMapper(UserRepository userRepository, ItemListPrivileger privileger)
        {
            _userRepository = userRepository;
            _privileger = privileger;
        }

        public void Configure(IMapperConfiguration mapper)
        {
            mapper.Configure<ItemListGroupDbo, ItemListGroup>(MapToModel);
            mapper.Configure<ItemListGroup, ItemListGroupDbo>(MapToDbo);
        }

        private static ItemListGroupDbo MapToDbo(ItemListGroup model, MappingContext ctx)
        {
            var dbo = new ItemListGroupDbo();
            dbo.CreationDate = model.CreationDate;
            dbo.Id = model.Id;
            dbo.ItemListId = model.ItemListId;
            dbo.Items = model.Items.Select(item => ctx.Mapper.Map<ItemDbo>(item)).ToList();
            dbo.LastModified = model.LastModified;
            dbo.Name = model.Name;
            dbo.UserId = model.UserId;

            return dbo;
        }

        private ItemListGroup MapToModel(ItemListGroupDbo dbo, MappingContext ctx)
        {
            var model = new ItemListGroup();
            model.CreationDate = dbo.CreationDate;
            model.Id = dbo.Id;
            model.ItemListId = dbo.ItemListId;
            model.LastModified = dbo.LastModified;
            model.Items = dbo.Items.Select(itemDbo => ctx.Mapper.Map<Item>(itemDbo)).ToList();
            model.Name = dbo.Name;
            model.UserId = dbo.UserId;
            model.User = ctx.Mapper.Map<User>(_userRepository.GetById(dbo.UserId));
            model.Privileges = _privileger.GetPrivileges(dbo);

            return model;
        }
    }
}