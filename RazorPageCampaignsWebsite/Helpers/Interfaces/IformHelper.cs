using Microsoft.AspNetCore.Html;

namespace RazorPageCampaignsWebsite.Helpers.Interfaces
{
    public interface IFormHelper
    {
        IHtmlContent TagBuilder(string formType, string contentTypeId);
    }
}
