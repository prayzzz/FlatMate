using System;
using System.Security.Claims;
using FlatMate.Web.Common.Filter;
using Microsoft.AspNetCore.Mvc;

namespace FlatMate.Web.Common.Base
{
    [ServiceFilter(typeof(MvcResultFilter))]
    public class MvcController : Controller
    {
        protected int CurrentUserId
        {
            get
            {
                var userId = User?.FindFirst(ClaimTypes.Sid).Value;
                if (userId == null)
                {
                    return 0;
                }

                return Convert.ToInt32(userId);
            }
        }
    }
}