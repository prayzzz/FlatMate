using System;
using System.Collections.Generic;
using FlatMate.Common;
using FlatMate.Module.Account.Domain.Entities;
using prayzzz.Common.Result;

namespace FlatMate.Module.Lists.Domain.Entities
{
    public class ItemListGroup : Entity
    {
        private readonly List<ItemListItem> _items = new List<ItemListItem>();

        public ItemListGroup(int id, string name, User owner) : base(id)
        {
            CreationDate = ModifiedDate = DateTime.Now;
            Owner = LastEditor = owner;
            Name = name;

            Order = 0;
        }

        public IReadOnlyList<ItemListItem> Items => _items;

        public User LastEditor { get; set; }

        public string Name { get; private set; }

        public User Owner { get; }

        public DateTime CreationDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public int Order { get; set; }

        public Result AddItem(ItemListItem item)
        {
            _items.Add(item);
            return new SuccessResult();
        }

        public void Rename(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            Name = name;
        }
    }
}