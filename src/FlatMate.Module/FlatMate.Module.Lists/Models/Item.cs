using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FlatMate.Common;
using FlatMate.Common.Repository;
using FlatMate.Module.Account.Models;
using FlatMate.Module.Lists.Services;
using prayzzz.Common.Dbo;
using prayzzz.Common.Mapping;

namespace FlatMate.Module.Lists.Models
{
    public class Item : PrivilegedModel
    {
        [Editable(false)]
        public DateTime CreationDate { get; set; }

        public int Id { get; set; }

        public int? ItemListGroupId { get; set; }

        public int? ItemListId { get; set; }

        [Editable(false)]
        public DateTime LastModified { get; set; }

        [Editable(false)]
        public User User { get; set; }

        [Editable(false)]
        public int UserId { get; set; }

        [Required]
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
        private readonly ItemListPrivileger _privileger;
        private readonly IRepository<UserDbo> _userRepository;

        public ItemMapper(IRepository<UserDbo> userRepository, ItemListPrivileger privileger)
        {
            _userRepository = userRepository;
            _privileger = privileger;
        }

        public void Configure(IMapperConfiguration mapper)
        {
            mapper.Configure<ItemDbo, Item>(MapToModel);
            mapper.Configure<Item, ItemDbo>(MapToDbo);
        }

        private static ItemDbo MapToDbo(Item model, ItemDbo dbo, MappingContext ctx)
        {
            dbo.CreationDate = model.CreationDate;
            dbo.Id = model.Id;
            dbo.ItemListId = model.ItemListId;
            dbo.ItemListGroupId = model.ItemListGroupId;
            dbo.LastModified = model.LastModified;
            dbo.UserId = model.UserId;
            dbo.Value = model.Value;

            return dbo;
        }

        private Item MapToModel(ItemDbo dbo, MappingContext ctx)
        {
            var item = new Item();
            item.CreationDate = dbo.CreationDate;
            item.Id = dbo.Id;
            item.ItemListId = dbo.ItemListId;
            item.ItemListGroupId = dbo.ItemListGroupId;
            item.LastModified = dbo.LastModified;
            item.User = ctx.Mapper.Map<User>(_userRepository.GetById(dbo.UserId).Data);
            item.UserId = dbo.UserId;
            item.Value = dbo.Value;
            item.Privileges = _privileger.GetPrivileges(dbo);

            return item;
        }
    }
}