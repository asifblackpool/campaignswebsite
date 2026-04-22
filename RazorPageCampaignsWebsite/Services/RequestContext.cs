namespace RazorPageCampaignsWebsite.Services
{

    public interface IRequestContext
    {
        bool IsPreview { get; }
        string Host { get; }
        IHeaderDictionary Headers { get; }
    }


    public class RequestContext : IRequestContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private bool? _cachedIsPreview;
        private string? _cachedHost;

        public RequestContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Host
        {
            get
            {
                if (_cachedHost != null) return _cachedHost;

                var request = _httpContextAccessor.HttpContext?.Request;

                _cachedHost = request?.Host.Host.ToLower().Trim() ?? string.Empty;

                return _cachedHost;
            }
        }

        public IHeaderDictionary? Headers
        {
            get
            {
             
                return _httpContextAccessor.HttpContext?.Request.Headers;
            }
        }

        public bool IsPreview
        {
            get
            {
                if (_cachedIsPreview.HasValue) return _cachedIsPreview.Value;

                var host = Host;

                _cachedIsPreview =
                    host.Contains("preview") ||
                    host == "localhost" ||
                    host == "127.0.0.1";

                return _cachedIsPreview.Value;
            }
        }
    }
}
