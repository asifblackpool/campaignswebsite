using Microsoft.AspNetCore.Mvc;
using RazorPageCampaignsWebsite.Controllers.Base;
using RazorPageCampaignsWebsite.Services.Interfaces;

namespace RazorPageCampaignsWebsite.Controllers
{
    public class CmsFallbackController : DynamicCmsController
    {
        public CmsFallbackController(IZengentiClient cmsClient, ICmsViewModelFactory viewModelFactory, ILogger<CampaignsController> logger)
            : base(cmsClient, viewModelFactory, logger) { }

        public async Task<IActionResult> Dynamic(string section, string slug)
        {
            return await RenderDynamicPageAsync(section, slug);
        }
    }
}
