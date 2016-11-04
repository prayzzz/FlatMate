using FlatMate.Web.Common.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FlatMate.Web.Filter
{
    public class MvcResultFilter : ActionFilterAttribute
    {
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

            model.ErrorMessage = string.Format(model.ErrorResult.ErrorMessage, model.ErrorResult.ErrorMessageArgs);
        }
    }
}