
using Content.Modelling.Models.Interfaces;
using Zengenti.Contensis.Delivery;

namespace RazorPageCampaignsWebsite.Core.Interfaces
{
    public interface IContentRepository
    {
        //List<T> GetChildEntries<T>(string parentUri);
        List<T> GetChildEntries<T>(string parentUri) where T : class, IPageTemplates;
    }
}
