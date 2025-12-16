

using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageCampaignsWebsite.Helpers;
using RazorPageCampaignsWebsite.Core.Models;
using RazorPageCampaignsWebsite.Services.Interfaces;
using RazorPageCampaignsWebsite.Services.Breadcrumb;
using RazorPageCampaignsWebsite.Core.Interfaces;
using Blackpool.Zengenti.CMS.Models.Templates;

namespace RazorPageCampaignsWebsite.Pages.Home
{
    public class DetailsModel : BasePageModel<BGStandard>
    {

        public DetailsModel(ILogger<BasePageModel<BGStandard>> logger,
                            IDataService<BGStandard> dataService,
                            IContentRepository contentRepository, BreadcrumbService breadcrumb) : base(logger, dataService, contentRepository, breadcrumb) { }

        public override async Task OnGetAsync()
        {
            ViewData["Title"] = "Campaign details page";
            ViewData["Model"] = null;

            string? path = HttpContext.Request.Path;
            path = (path == null) ? string.Empty : path.RemoveFileExtension(FILE_Extension.ASPX);
            if (path != null)
            {
                await base.OnGetByPathAsync(path);
            }
            else
            {
                await base.OnGetAsync();
            }
            Items = Items.Take(1).ToList();
            LogAction("Getting Campaign details specific data loaded");
        }



    }
}
