using RazorPageCampaignsWebsite.Models;

namespace RazorPageCampaignsWebsite.Services.Interfaces
{
    public interface ICmsViewModelFactory
    {
        Task<(string ViewName, object ViewModel)> CreateAsync(CmsNode node);
    }
}
