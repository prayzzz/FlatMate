using FlatMate.Module.Lists.Models;
using FlatMate.Web.Common.Base;

namespace FlatMate.Web.Areas.Lists.Data
{
    public class ItemListEditVm : BaseViewModel
    {
        public bool IsEditable { get; set; }

        public ItemList ItemList { get; set; }
    }
}