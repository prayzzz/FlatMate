using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Razor;

namespace FlatMate.Web.Common
{
    public class RazorViewLocationExpander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            var locations = viewLocations.ToList();

            locations.Add("/Areas/Lists/Views/Dashboard/{0}.cshtml");

            return locations;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
        }
    }
}