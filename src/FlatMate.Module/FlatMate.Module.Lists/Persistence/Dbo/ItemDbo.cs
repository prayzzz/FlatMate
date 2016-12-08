using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FlatMate.Common.Attributes;
using prayzzz.Common.Dbo;

namespace FlatMate.Module.Lists.Persistence.Dbo
{
    [Table("Item")]
    public class ItemDbo : BaseDbo
    {
        [ForeignKey("ItemListGroupId")]
        public ItemListGroupDbo Group { get; set; }

        [ForeignKeyField]
        public int? ItemListGroupId { get; set; }

        public int Order { get; set; }

        [Required]
        [Column("Value")]
        public string Name { get; set; }

        [Column("UserId")]
        public int OwnerUserId { get; set; }

        [NotMapped]
        public int LastEditorUserId { get; set; }
    }
}