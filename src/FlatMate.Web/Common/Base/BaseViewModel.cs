using FlatMate.Module.Account.Models;
using FlatMate.Web.Filter;
using prayzzz.Common.Result;

namespace FlatMate.Web.Common.Base
{
    public class EmptyViewModel : BaseViewModel
    {}

    public abstract class BaseViewModel
    {
        public User CurrentUser { get; set; }

        /// <summary>
        ///     Set by <see cref="ErrorResult" /> if <see cref="MvcResultFilter" /> is filled
        /// </summary>
        public string ErrorMessage { get; set; }

        public Result ErrorResult { get; set; }

        public bool HasError => ErrorResult != null || !string.IsNullOrEmpty(ErrorMessage);
    }
}