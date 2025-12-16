using Blackpool.Zengenti.CMS.Models.Interfaces;
using Blackpool.Zengenti.CMS.Models.Templates;
using Content.Modelling.Models.Interfaces;
using RazorPageCampaignsWebsite.Core.Interfaces;

namespace RazorPageCampaignsWebsite.Core.Services
{
    

    // Core/Services/ContentService.cs
    public class ContentService : IContentService
    {
        private readonly IContentRepository _repository;

        public ContentService(IContentRepository repository)
        {
            _repository = repository;
        }

        public List<IPageTemplates> GetChildPages(string parentUri)
        {
            return _repository.GetChildEntries<BGStandard>(parentUri)
                       .Cast<IPageTemplates>()
                       .ToList();
        }
    }
}
