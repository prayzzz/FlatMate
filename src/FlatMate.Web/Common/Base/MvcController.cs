using System;
using System.Security.Claims;
using FlatMate.Module.Account.Models;
using FlatMate.Web.Filter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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
                return userId == null ? 0 : Convert.ToInt32(userId);
            }
        }

        protected string CurrentUserName
        {
            get
            {
                var userId = User?.FindFirst(ClaimTypes.Name).Value;
                return userId ?? "";
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            var viewResult = context.Result as ViewResult;
            if (viewResult == null)
            {
                return;
            }

            var model = viewResult.Model as BaseViewModel;
            if (model == null)
            {
                return;
            }

            model.CurrentUser = new User();
            model.CurrentUser.Id = CurrentUserId;
            model.CurrentUser.UserName = CurrentUserName;
        }
    }
}