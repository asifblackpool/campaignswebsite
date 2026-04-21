
using Microsoft.AspNetCore.Http;
using Nancy;
using Zengenti.Contensis.Delivery;

namespace RazorPageCampaignsWebsite.Services
{
    public interface IContensisClientResolver
    {
        ContensisClient GetClient();
        bool IsPreview();
        string GetHost();
    }

    public class ContensisClientResolver : IContensisClientResolver
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ContensisClient? _cachedClient;
        private bool? _cachedIsPreview;

        public ContensisClientResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ContensisClient GetClient()
        {
            // Cache per request (since resolver is scoped)
            if (_cachedClient != null)
                return _cachedClient;

            bool isPreview = IsPreview();
            _cachedClient = ContensisClientFactory.CreateClient(isPreview);
            return _cachedClient;
        }

        public string GetHost()
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null)
            {
                return "Host not found";
            }
            return request.Host.Host.ToString().ToLower().Trim();
        }

        public bool IsPreview()
        {
            if (_cachedIsPreview.HasValue)
                return _cachedIsPreview.Value;

            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null)
            {
                _cachedIsPreview = false;
                return false;
            }

            var host = request.Host.Host.ToString().ToLower().Trim();
            _cachedIsPreview = host.Contains("preview") ||
                               host == "localhost" ||
                               host == "127.0.0.1";
            return _cachedIsPreview.Value;
        }
    }
}
