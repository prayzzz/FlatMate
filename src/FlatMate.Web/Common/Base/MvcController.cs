using FlatMate.Web.Common.Filter;
using Microsoft.AspNetCore.Mvc;

namespace FlatMate.Web.Common.Base
{
    [ServiceFilter(typeof(MvcResultFilter))]
    public class MvcController : Controller
    {
    }
}