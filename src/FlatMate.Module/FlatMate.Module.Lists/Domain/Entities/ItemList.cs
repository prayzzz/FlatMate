using System;
using System.Collections.Generic;
using FlatMate.Common;
using FlatMate.Module.Account.Domain.Entities;
using prayzzz.Common.Result;

namespace FlatMate.Module.Lists.Domain.Entities
{
    public class ItemList : Entity
    {
        private readonly List<ItemListGroup> _groups = new List<ItemListGroup>();

        public ItemList(int id, string name, User owner) : base(id)
        {
            CreationDate = ModifiedDate = DateTime.Now;
            Owner = owner;
            Name = name;

            Description = string.Empty;
            IsPublic = false;
        }

        public DateTime CreationDate { get; set; }

        public string Description { get; set; }

        public IReadOnlyList<ItemListGroup> Groups => _groups;

        public bool IsPublic { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string Name { get; private set; }

        public User Owner { get; }

        public Result AddGroup(ItemListGroup group)
        {
            _groups.Add(group);
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