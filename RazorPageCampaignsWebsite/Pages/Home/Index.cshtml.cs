using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageCampaignsWebsite.Services.Interfaces;
using RazorPageCampaignsWebsite.Services.Breadcrumb;
using RazorPageCampaignsWebsite.Core.Models;
using Blackpool.Zengenti.CMS.Models.Weddings;
using RazorPageCampaignsWebsite.Core.Interfaces;

namespace RazorPageCampaignsWebsite.Pages;

public class IndexModel : BasePageModel<GettingMarriedHome>
{

    public IndexModel(ILogger<BasePageModel<GettingMarriedHome>> logger,
                      IDataService<GettingMarriedHome> dataService,
                      IContentRepository contentRepository, BreadcrumbService breadcrumb) : base(logger, dataService, contentRepository, breadcrumb) { }


    public override async Task OnGetAsync() // Default handler
    {
        ViewData["Title"] = "Homepage";
      

        await base.OnGetAsync();
        Items = Items.Take(1).ToList();
    
      LogAction("Getting Married Home data loaded");
    }


}


