using FlatMate.Web.Common.Filter;
using prayzzz.Common.Result;

namespace FlatMate.Web.Common.Base
{
    public abstract class BaseViewModel
    {
        /// <summary>
        ///     Set by <see cref="ErrorResult" /> if <see cref="MvcResultFilter" /> is filled
        /// </summary>
        public string ErrorMessage { get; set; }

        public Result ErrorResult { get; set; }

        public bool HasError => ErrorResult != null || !string.IsNullOrEmpty(ErrorMessage);
    }
}