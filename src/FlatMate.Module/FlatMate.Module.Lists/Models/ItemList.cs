using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FlatMate.Module.Account.Models;
using prayzzz.Common.Mapping;

namespace FlatMate.Module.Lists.Models
{
    public class ItemList
    {
        public DateTime CreationDate { get; set; }

        public User Creator { get; set; }

        public string Description { get; set; }
        public int Id { get; set; }

        public bool IsPublic { get; set; }

        public List<Item> Items { get; set; }

        public DateTime LastModified { get; set; }

        public List<ItemListGroup> ListGroups { get; set; }

        public string Name { get; set; }
    }

    public class ItemListDbo
    {
        public DateTime CreationDate { get; set; }

        public string Description { get; set; }

        [Key]
        public int Id { get; set; }

        public bool IsPublic { get; set; }

        public List<ItemDbo> Items { get; set; }

        public DateTime LastModified { get; set; }

        public List<ItemListGroupDbo> ListGroups { get; set; }

        public string Name { get; set; }

        [Column("User")]
        public int UserId { get; set; }
    }

    public class ItemListMapper : IDboMapper
    {
        public void Configure(IMapperConfiguration mapper)
        {
            mapper.Configure<ItemListDbo, ItemList>(ConvertToModel);
            mapper.Configure<ItemList, ItemListDbo>(ConvertToDbo);
        }

        private ItemListDbo ConvertToDbo(ItemList listModel, ItemListDbo listDbo, MappingCtx ctx)
        {
            listDbo.CreationDate = listModel.CreationDate;
            listDbo.UserId = listModel.Creator.Id;
            listDbo.Description = listModel.Description;
            listDbo.Id = listModel.Id;
            listDbo.IsPublic = listModel.IsPublic;
            listDbo.LastModified = listModel.LastModified;
            listDbo.ListGroups = listModel.ListGroups.Select(group => ConvertToDbo(new ItemListGroupDbo(), group, listDbo, ctx)).ToList();
            listDbo.Items = listModel.Items.Select(item => ConvertToDbo(new ItemDbo(), item, listDbo, null, ctx)).ToList();
            listDbo.Name = listModel.Name;

            return listDbo;
        }

        private ItemListGroupDbo ConvertToDbo(ItemListGroupDbo groupDbo, ItemListGroup groupModel, ItemListDbo listDbo, MappingCtx ctx)
        {
            groupDbo.CreationDate = groupModel.CreationDate;
            groupDbo.Id = groupModel.Id;
            groupDbo.ItemList = listDbo;
            groupDbo.Items = groupModel.Items.Select(item => ConvertToDbo(new ItemDbo(), item, listDbo, groupDbo, ctx)).ToList();
            groupDbo.LastModified = groupModel.LastModified;
            groupDbo.Name = groupModel.Name;

            return groupDbo;
        }

        private ItemDbo ConvertToDbo(ItemDbo itemDbo, Item model, ItemListDbo listDbo, ItemListGroupDbo groupDbo, MappingCtx ctx)
        {
            itemDbo.CreationDate = model.CreationDate;
            itemDbo.UserId = model.Creator.Id;
            itemDbo.Id = model.Id;
            itemDbo.ItemList = listDbo;

            if (groupDbo != null)
            {
                itemDbo.ItemListGroup = groupDbo;
            }

            itemDbo.LastModified = model.LastModified;
            itemDbo.Value = model.Value;

            return itemDbo;
        }

        private ItemList ConvertToModel(ItemListDbo dbo, ItemList listModel, MappingCtx ctx)
        {
            listModel.CreationDate = dbo.CreationDate;
            listModel.Creator.Id = dbo.UserId;
            listModel.Description = dbo.Description;
            listModel.Id = dbo.Id;
            listModel.IsPublic = dbo.IsPublic;
            listModel.LastModified = dbo.LastModified;
            listModel.Items = dbo.Items.Select(itemDbo => ConvertToModel(new Item(), itemDbo, listModel, null, ctx)).ToList();
            listModel.ListGroups = dbo.ListGroups.Select(groupDbo => ConvertToModel(new ItemListGroup(), groupDbo, listModel, ctx)).ToList();
            listModel.Name = dbo.Name;

            return listModel;
        }

        private ItemListGroup ConvertToModel(ItemListGroup groupModel, ItemListGroupDbo groupDbo, ItemList listModel, MappingCtx ctx)
        {
            groupModel.CreationDate = groupDbo.CreationDate;
            groupModel.Id = groupDbo.Id;
            groupModel.ItemList = listModel;
            groupModel.Items = groupDbo.Items.Select(itemDbo => ConvertToModel(new Item(), itemDbo, listModel, groupModel, ctx)).ToList();
            groupModel.LastModified = groupDbo.LastModified;
            groupModel.Name = groupDbo.Name;

            return groupModel;
        }

        private Item ConvertToModel(Item item, ItemDbo itemDbo, ItemList listModel, ItemListGroup groupModel, MappingCtx ctx)
        {
            item.CreationDate = itemDbo.CreationDate;
            item.Creator.Id = itemDbo.UserId;
            item.Id = itemDbo.Id;
            item.ItemList = listModel;

            if (groupModel != null)
            {
                item.ItemListGroup = groupModel;
            }

            item.LastModified = itemDbo.LastModified;
            item.Value = itemDbo.Value;

            return item;
        }
    }
}