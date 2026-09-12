
using Content.Modelling.Models.Interfaces;

namespace RazorPageCampaignsWebsite.Core.Interfaces
{
    public interface IContentService
    {
        Task<List<IPageTemplates>> GetChildPagesAsync(string parentUri);
    }
}
