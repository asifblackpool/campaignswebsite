
using Microsoft.AspNetCore.Http;
using Nancy;
using System.Net.Http;
using Zengenti;
using Zengenti.Contensis.Delivery;

namespace RazorPageCampaignsWebsite.Services
{
    public interface IContensisClientResolver
    {
        ContensisClient GetClient();
        string showHost { get; }
        bool isPreview { get; }
        string showVersionStatus { get; }
        

    }

    public class ContensisClientResolver : IContensisClientResolver
    {
        private ContensisClient? _cachedClient;
        private readonly IRequestContext _requestContext;
        private string? _computedVersionStatus;
        private bool? _computedIsPreview;

        public ContensisClientResolver(IRequestContext rc)
        {
            _requestContext = rc;
        }

        public ContensisClient GetClient()
        {
            if (_cachedClient != null)
                return _cachedClient;

            string versionStatus = DetermineVersionStatus();
            _cachedClient = ContensisClientFactory.CreateClient(
                isPreview: versionStatus == "latest",
                QueryStringversionStatus: versionStatus
            );
            return _cachedClient;
        }

        private string DetermineVersionStatus()
        {
            // 1. Query string override
            string? queryVersion = _requestContext.GetQueryStringVersionStatus();
            if (!string.IsNullOrEmpty(queryVersion) && (queryVersion == "latest" || queryVersion == "published"))
            {
                _computedVersionStatus   = queryVersion;
                _computedIsPreview       = queryVersion == "latest";
                return _computedVersionStatus;
            }

            // 2. Header from reverse proxy
            if (_requestContext.Headers.TryGetValue("x-entry-versionstatus", out var headerValues))
            {
                string? headerVersion = headerValues.FirstOrDefault();
                if (!string.IsNullOrEmpty(headerVersion) && (headerVersion == "latest" || headerVersion == "published"))
                {
                    _computedVersionStatus = headerVersion;
                    _computedIsPreview = headerVersion == "latest";
                    return _computedVersionStatus;
                }
            }

            // 3. Host-based fallback
            string host = _requestContext.Host.ToString().ToLower();
            bool isPreviewHost = host.Contains("preview-blackpool") || host.Contains("cloud.contensis.com") || host.Contains("localhost");

            _computedVersionStatus = isPreviewHost ? "latest" : "published";
            _computedIsPreview = isPreviewHost;
            return _computedVersionStatus;
        }

        // Interface implementation – exact naming (lowercase first letter)
        public string showVersionStatus => _computedVersionStatus ?? DetermineVersionStatus();
        public string showHost => _requestContext.Host.ToString().ToLower();
        public bool isPreview => _computedIsPreview ?? (_computedVersionStatus == "latest");
    }
}
