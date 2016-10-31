using System.Collections.Generic;
using FlatMate.Module.Lists.Models;
using FlatMate.Web.Common.Base;

namespace FlatMate.Web.Areas.Lists.Data
{
    public class HomeIndexVm : BaseViewModel
    {
        public List<ItemList> OwnItemLists { get; set; } = new List<ItemList>();

        public List<ItemList> PublicItemLists { get; set; } = new List<ItemList>();
    }
}