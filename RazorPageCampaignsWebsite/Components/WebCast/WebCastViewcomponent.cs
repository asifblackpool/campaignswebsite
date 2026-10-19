using Microsoft.AspNetCore.Mvc;
using RazorPageCampaignsWebsite.Components.Extensions;

namespace RazorPageCampaignsWebsite.Components.Webcast
{
    public class WebCastViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string? introOverride)
        {
            return View(ViewComponentExtensions.GetViewPath("WebCast"), introOverride);
        }
    }
}