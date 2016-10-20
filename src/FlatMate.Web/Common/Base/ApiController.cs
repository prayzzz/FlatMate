using System;
using System.Security.Claims;
using FlatMate.Web.Common.Filter;
using Microsoft.AspNetCore.Mvc;
using prayzzz.Common.Mvc;

namespace FlatMate.Web.Common.Base
{
    [ValidationFilter]
    [ServiceFilter(typeof(ApiResultFilter))]
    public class ApiController : Controller
    {
        protected int UserId => Convert.ToInt32(User.FindFirstValue(ClaimTypes.Sid));
    }
}