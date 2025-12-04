using Blackpool.Zengenti.CMS.Models.Canvas.Helpers;
using Microsoft.AspNetCore.Html;
using RazorPageCampaignsWebsite.Helpers.Interfaces;

namespace RazorPageCampaignsWebsite.Helpers.Wrappers
{
    public class FormHelperWrapper : IFormHelper
    {
        public IHtmlContent TagBuilder(string formType, string contentTypeId)
        {
            return new HtmlString(FormHelper.TagBuilder(formType, contentTypeId));
        }
    }
}
