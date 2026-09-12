using Microsoft.AspNetCore.Mvc;
using RazorPageCampaignsWebsite.Constants;
using RazorPageCampaignsWebsite.Controllers.Base;
using RazorPageCampaignsWebsite.Services.Interfaces;

namespace RazorPageCampaignsWebsite.Controllers
{
    public class YourCouncilController : DynamicCmsController
    {
        public YourCouncilController(IZengentiClient cmsClient, ICmsViewModelFactory viewModelFactory, ILogger<YourCouncilController> logger)
            : base(cmsClient, viewModelFactory, logger) { }

        [HttpGet]
        public async Task<IActionResult> Dynamic(string slug)
        {
            slug ??= "";
         return await RenderDynamicPageAsync(WebsiteConstants.VIEW_FOLDER, slug);
        }
    }
}
