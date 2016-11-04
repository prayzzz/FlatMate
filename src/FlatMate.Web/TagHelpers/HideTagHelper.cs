using Microsoft.AspNetCore.Razor.TagHelpers;

namespace FlatMate.Web.TagHelpers
{
    [HtmlTargetElement(Attributes = nameof(Hide))]
    public class HideTagHelper : TagHelper
    {
        public bool Hide { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Hide)
            {
                output.SuppressOutput();
            }
        }
    }
}