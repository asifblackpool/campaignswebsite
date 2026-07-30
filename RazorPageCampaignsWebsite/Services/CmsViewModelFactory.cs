using RazorPageCampaignsWebsite.Models;
using RazorPageCampaignsWebsite.Services;
using RazorPageCampaignsWebsite.Services.Interfaces;
using RazorPageCampaignsWebsite.ViewModels;

namespace RazorPageCampaignsWebsite.Services
{
    public class CmsViewModelFactory : ICmsViewModelFactory
    {
        private readonly ContentViewModelService _viewModelService;

        public CmsViewModelFactory(ContentViewModelService viewModelService)
        {
            _viewModelService = viewModelService;
        }

        public async Task<(string ViewName, object ViewModel)> CreateAsync(CmsNode node)
        {
            DetailsViewModel detailsViewModel = await _viewModelService.GetViewModelForPathAsync(node.Path);
            var wrapper = new ViewModelWrapper { ViewModel = detailsViewModel };
            return ("DynamicPage", wrapper);
        }
    }
}