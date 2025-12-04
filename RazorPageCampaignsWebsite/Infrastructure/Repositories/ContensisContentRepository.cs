using Content.Modelling.Models.Interfaces;
using RazorPageCampaignsWebsite.Core.Interfaces;
using Zengenti.Contensis.Delivery;

namespace RazorPageCampaignsWebsite.Infrastructure.Repositories
{
    // Infrastructure/Repositories/ContensisRepository.cs
    using Zengenti.Contensis.Delivery;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Blackpool.Zengenti.CMS.Models.Interfaces;

    public class ContensisContentRepository : IContentRepository
    {
        private readonly IContensisClient _client;

        public ContensisContentRepository(IContensisClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public List<T> GetChildEntries<T>(string parentUri) where T : class, IGettingMarried
        {
            var results = new List<T>();

            // 1. Get the parent node by its URI
            var parentNode = _client.Nodes.GetByPath(parentUri);

            if (parentNode == null)
                return results;

            // 2. Loop through its children and get their entries typed as T
            var children = parentNode.Children();
            if (children == null || !children.Any())
                return results;

            foreach (var childNode in children)
            {
                var entry = childNode.Entry<T>();
                if (entry != null)
                    results.Add(entry);
            }

            return results;
        }
    }

}
