using Microsoft.AspNetCore.Mvc;
using RazorPageCampaignsWebsite.Constants;
using RazorPageCampaignsWebsite.Controllers.Base;
using RazorPageCampaignsWebsite.Services.Interfaces;

namespace RazorPageCampaignsWebsite.Controllers
{
    public class CampaignsController : DynamicCmsController
    {
        public CampaignsController(IZengentiClient cmsClient, ICmsViewModelFactory viewModelFactory, ILogger<CampaignsController> logger)
          : base(cmsClient, viewModelFactory, logger) { }

        [HttpGet]
        public async Task<IActionResult> Dynamic(string slug)
        {
            slug ??= "";
            return await RenderDynamicPageAsync(WebsiteConstants.VIEW_FOLDER, slug);
        }
    }
}
