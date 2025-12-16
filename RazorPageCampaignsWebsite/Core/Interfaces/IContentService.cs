
using Content.Modelling.Models.Interfaces;

namespace RazorPageCampaignsWebsite.Core.Interfaces
{
    public interface IContentService
    {
        List<IPageTemplates> GetChildPages(string parentUri);
    }
}
