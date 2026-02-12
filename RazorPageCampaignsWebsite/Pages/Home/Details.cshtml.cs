using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageCampaignsWebsite.Helpers;
using RazorPageCampaignsWebsite.Core.Models;
using RazorPageCampaignsWebsite.Services.Interfaces;
using RazorPageCampaignsWebsite.Services.Breadcrumb;
using RazorPageCampaignsWebsite.Core.Interfaces;
using Content.Modelling.Models.Templates;
using Content.Modelling.Models.Templates.Base;

namespace RazorPageCampaignsWebsite.Pages.Home
{
    public class DetailsModel : BasePageModel<dynamic>
    {

        public DetailsModel(ILogger<BasePageModel<dynamic>> logger,
                            IDataService<dynamic> dataService,
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
            PopulateConcreteModel(Items);


            LogAction("Getting Campaign details specific data loaded");
        }

        protected override void OnModelPopulated()
        {

            // This runs after the concrete model is populated
            if (ViewModel.ConcreteModel is BGStandardWithForms formsModel)
            {
                // Do something specific to forms pages
            }
        }
    }
}
