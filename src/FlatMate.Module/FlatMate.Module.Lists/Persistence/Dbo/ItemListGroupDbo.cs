using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using prayzzz.Common.Dbo;

namespace FlatMate.Module.Lists.Persistence.Dbo
{
    [Table("ItemListGroup")]
    public class ItemListGroupDbo : BaseDbo
    {
        public int ItemListId { get; set; }

        [InverseProperty("Group")]
        public List<ItemDbo> Items { get; set; } = new List<ItemDbo>();

        [ForeignKey("ItemListId")]
        public ItemListDbo List { get; set; }

        public string Name { get; set; }

        [Column("UserId")]
        public int OwnerUserId { get; set; }

        [NotMapped]
        public int LastEditorUserId { get; set; }

        [NotMapped]
        public int Order { get; set; }
    }
}