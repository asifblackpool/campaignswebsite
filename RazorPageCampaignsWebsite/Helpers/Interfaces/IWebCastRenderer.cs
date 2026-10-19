using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RazorPageCampaignsWebsite.Helpers.Interfaces
{
    public interface IWebCastRenderer
    {
        Task<IHtmlContent> RenderAsync(string? introOverride, ViewContext viewContext);
    }
}
