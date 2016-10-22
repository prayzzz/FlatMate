using System.Collections.Generic;
using FlatMate.Module.Lists.Models;
using FlatMate.Web.Common.Base;

namespace FlatMate.Web.Areas.Lists.Data
{
    public class HomeIndexVm : BaseViewModel
    {
        public List<ItemList> ItemLists { get; set; } = new List<ItemList>();
    }
}