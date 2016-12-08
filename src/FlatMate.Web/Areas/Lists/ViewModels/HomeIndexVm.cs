using System.Collections.Generic;
using FlatMate.Web.Areas.Lists.Dto;
using FlatMate.Web.Common.Base;

namespace FlatMate.Web.Areas.Lists.ViewModels
{
    public class HomeIndexVm : BaseViewModel
    {
        public List<ListDto> OwnItemLists { get; set; } = new List<ListDto>();

        public List<ListDto> PublicItemLists { get; set; } = new List<ListDto>();
    }
}