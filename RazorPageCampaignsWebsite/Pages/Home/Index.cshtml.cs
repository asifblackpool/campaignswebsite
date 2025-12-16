using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageCampaignsWebsite.Services.Interfaces;
using RazorPageCampaignsWebsite.Services.Breadcrumb;
using RazorPageCampaignsWebsite.Core.Models;
using RazorPageCampaignsWebsite.Core.Interfaces;
using Blackpool.Zengenti.CMS.Models.Templates;

namespace RazorPageCampaignsWebsite.Pages;

public class IndexModel : BasePageModel<BGStandard>
{

    public IndexModel(ILogger<BasePageModel<BGStandard>> logger,
                      IDataService<BGStandard> dataService,
                      IContentRepository contentRepository, BreadcrumbService breadcrumb) : base(logger, dataService, contentRepository, breadcrumb) { }


    public override async Task OnGetAsync() // Default handler
    {
        ViewData["Title"] = "Homepage";
      

        await base.OnGetAsync();
        Items = Items.Take(1).ToList();
    
      LogAction("Getting Campaigns data loaded");
    }


}


