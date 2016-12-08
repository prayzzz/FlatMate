using prayzzz.Common.Enums;

namespace FlatMate.Module.Lists.Common
{
    public class ItemListQuery
    {
        public OrderingDirection Direction { get; set; } = OrderingDirection.Asc;

        public bool? IsPublic { get; set; }

        public ItemListQueryOrder Order { get; set; } = ItemListQueryOrder.None;

        public int? UserId { get; set; }
    }
}