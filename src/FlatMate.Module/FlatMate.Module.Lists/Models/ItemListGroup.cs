using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlatMate.Module.Lists.Models
{
    public class ItemListGroup
    {
        public DateTime CreationDate { get; set; }
        public int Id { get; set; }

        public ItemList ItemList { get; set; }

        public List<ItemListGroup> ItemListGroups { get; set; }

        public List<Item> Items { get; set; }

        public DateTime LastModified { get; set; }

        public string Name { get; set; }
    }

    public class ItemListGroupDbo
    {
        public DateTime CreationDate { get; set; }

        [Key]
        public int Id { get; set; }

        [ForeignKey("ItemListId")]
        public ItemListDbo ItemList { get; set; }

        [Column("ItemList")]
        public int ItemListId { get; set; }

        public List<ItemDbo> Items { get; set; }

        public DateTime LastModified { get; set; }

        public string Name { get; set; }

        [Column("User")]
        public int UserId { get; set; }
    }
}