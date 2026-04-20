using Microsoft.Extensions.Caching.Memory;
using RazorPageCampaignsWebsite.Constants;
using RazorPageCampaignsWebsite.Services.Interfaces;
using Zengenti.Contensis.Delivery;

namespace RazorPageCampaignsWebsite.Services
{
    public class ContensisDataService<T> : IDataService<T> where T : class, new()
    {
        private readonly IContensisClientResolver _clientResolver;
        private readonly IMemoryCache _cache;

        public ContensisDataService(IContensisClientResolver clientResolver, IMemoryCache cache)
        {
            _clientResolver = clientResolver ?? throw new ArgumentNullException(nameof(clientResolver));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public Task<List<T>> GetAllAsync(string? path)
        {
            string effectivePath = string.IsNullOrEmpty(path) ? WebsiteConstants.SITE_VIEW_PATH : path;
            var client = _clientResolver.GetClient();
            bool isPreview = _clientResolver.IsPreview();

            string cacheKey = $"{typeof(T).Name}_{effectivePath}_{(isPreview ? "preview" : "live")}";

            // Try to get from cache - handle possible null value
            if (_cache.TryGetValue(cacheKey, out List<T>? cachedData) && cachedData != null)
            {
                return Task.FromResult(cachedData);
            }

            // Not in cache – load data
            var data            = LoadData(effectivePath, client);
            var cacheOptions    = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            _cache.Set(cacheKey, data, cacheOptions);
            return Task.FromResult(data);
        }

        private List<T> LoadData(string path, ContensisClient client)
        {
            var data = new List<T>();

            try
            {
                var node = client.Nodes.GetByPath(path, null, 1);
                var entryId = (node != null && node.EntryId != null) ? node.EntryId : node?.Id;
                if (entryId != null)
                {
                    var entry = client.Entries.Get<T>((Guid)entryId, null, 1);
                    if (entry != null)
                        data.Add(entry);
                }
            }
            catch (Exception ex)
            {
                // Log error (you can inject ILogger here if needed)
                Console.WriteLine($"Error loading data for {path}: {ex.Message}");
            }

            return data;
        }
        public Task<T?> GetByIdAsync(int id, string? path)
        {
            var allData = GetAllAsync(path).Result;
            var item = allData.FirstOrDefault(x =>
            {
                var idValue = x.GetType().GetProperty("Id")?.GetValue(x) as int?;
                return idValue == id;
            });

            return Task.FromResult(item);
        }
    }
}