using System;
using FlatMate.Common;
using FlatMate.Module.Account.Domain.Entities;

namespace FlatMate.Module.Lists.Domain.Entities
{
    public class ItemListItem : Entity
    {
        public ItemListItem(int id, string name, User owner) : base(id)
        {
            CreationDate = ModifiedDate = DateTime.Now;
            Owner = LastEditor = owner;
            Name = name;

            Order = 0;
        }

        public User LastEditor { get; set; }

        public string Name { get; private set; }

        public User Owner { get; }

        public DateTime CreationDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public int Order { get; set; }

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