using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using prayzzz.Common.Mvc.Services;

namespace FlatMate.Web.Services
{
    public class RequestService : IRequestService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public RequestService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public int CurrentUserId
        {
            get
            {
                var userId = _contextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.Sid);
                if (userId != null)
                {
                    return Convert.ToInt32(userId);
                }

                return 0;
            }
        }
    }
}