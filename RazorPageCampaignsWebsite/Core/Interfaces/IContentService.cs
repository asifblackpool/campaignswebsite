using Blackpool.Zengenti.CMS.Models.Interfaces;

namespace RazorPageCampaignsWebsite.Core.Interfaces
{
    public interface IContentService
    {
        List<IGettingMarried> GetChildPages(string parentUri);
    }
}
