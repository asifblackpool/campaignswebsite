
using Blackpool.Zengenti.CMS.Models.Canvas.Lists;
using Microsoft.AspNetCore.Html;
using RazorPageCampaignsWebsite.Helpers.Html;

namespace RazorPageCampaignsWebsite.Helpers.Interfaces
{
    public interface IContentFragmentHelper
    {
        Task<IHtmlContent> BuildHtmlFragmentAsync(List<ContentFragment> fragments, string wrapperFormat);
        IHtmlContent BuildHtmlFragment(List<ContentFragment> fragments, string wrapperFormat);
    }
}
