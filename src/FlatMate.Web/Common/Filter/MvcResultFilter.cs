using FlatMate.Web.Common.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;

namespace FlatMate.Web.Common.Filter
{
    public class MvcResultFilter : ActionFilterAttribute
    {
        private readonly IStringLocalizer _localizer;

        public MvcResultFilter(IStringLocalizer localizer)
        {
            _localizer = localizer;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var viewResult = context.Result as ViewResult;

            if (viewResult == null)
            {
                return;
            }

            var model = viewResult.Model as BaseViewModel;

            if (model == null || !model.HasError || model.ErrorResult == null)
            {
                return;
            }

            model.ErrorMessage = _localizer[model.ErrorResult.ErrorMessage, model.ErrorResult.ErrorMessageArgs];
        }
    }
}