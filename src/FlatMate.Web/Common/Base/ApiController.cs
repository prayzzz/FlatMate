using System;
using System.Security.Claims;
using FlatMate.Web.Common.Filter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlatMate.Web.Common.Base
{
    [ServiceFilter(typeof(ApiResultFilter))]
    public class ApiController
    {
        protected ApiController(IHttpContextAccessor context)
        {
            var userId = context.HttpContext.User?.FindFirstValue(ClaimTypes.Sid);

            if (userId != null)
            {
                CurrentUserId = Convert.ToInt32(userId);
            }
        }

        protected int CurrentUserId { get; }
    }
}