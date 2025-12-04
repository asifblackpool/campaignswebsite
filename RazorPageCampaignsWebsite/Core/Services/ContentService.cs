using Blackpool.Zengenti.CMS.Models.Interfaces;
using Blackpool.Zengenti.CMS.Models.Weddings;
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

        public List<IGettingMarried> GetChildPages(string parentUri)
        {
            return _repository.GetChildEntries<GettingMarried>(parentUri)
                       .Cast<IGettingMarried>()
                       .ToList();
        }
    }
}
