using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using prayzzz.Common.Dbo;

namespace FlatMate.Web.Services
{
    public class OwnerCheck : IOwnerCheck
    {
        private readonly int _currentUserId;

        public OwnerCheck(IHttpContextAccessor context)
        {
            var userId = context.HttpContext.User?.FindFirstValue(ClaimTypes.Sid);

            if (userId != null)
            {
                _currentUserId = Convert.ToInt32(userId);
            }
        }

        public bool IsOwnedByCurrentUser(OwnedDbo dbo)
        {
            return dbo.UserId == _currentUserId;
        }
    }
}