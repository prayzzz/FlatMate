using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FlatMate.Module.Account.Models;

namespace FlatMate.Module.Lists.Models
{
    public class Item
    {
        public DateTime CreationDate { get; set; }

        public User Creator { get; set; }
        public int Id { get; set; }

        public ItemList ItemList { get; set; }

        public ItemListGroup ItemListGroup { get; set; }

        public DateTime LastModified { get; set; }

        public string Value { get; set; }
    }

    public class ItemDbo
    {
        public DateTime CreationDate { get; set; }

        [Column("User")]
        public int UserId { get; set; }

        [Key]
        public int Id { get; set; }

        [ForeignKey("ItemListId")]
        public ItemListDbo ItemList { get; set; }

        [ForeignKey("ItemListGroupId")]
        public ItemListGroupDbo ItemListGroup { get; set; }

        [Column("ItemListGroup")]
        public int ItemListGroupId { get; set; }

        [Column("ItemList")]
        public int ItemListId { get; set; }

        public DateTime LastModified { get; set; }

        public string Value { get; set; }
    }
}