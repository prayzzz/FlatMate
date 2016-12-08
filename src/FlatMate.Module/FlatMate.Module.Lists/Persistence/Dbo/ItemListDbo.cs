using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using prayzzz.Common.Dbo;

namespace FlatMate.Module.Lists.Persistence.Dbo
{
    [Table("ItemList")]
    public class ItemListDbo : BaseDbo
    {
        public string Description { get; set; }

        [InverseProperty("List")]
        public List<ItemListGroupDbo> Groups { get; set; } = new List<ItemListGroupDbo>();

        public bool IsPublic { get; set; }

        [NotMapped]
        public List<ItemDbo> Items { get; set; }

        public string Name { get; set; }
        
        [Column("UserId")]
        public int OwnerUserId { get; set; }
    }
}