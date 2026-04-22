
using Microsoft.AspNetCore.Http;
using Nancy;
using Zengenti.Contensis.Delivery;

namespace RazorPageCampaignsWebsite.Services
{
    public interface IContensisClientResolver
    {
        ContensisClient GetClient();
        string showHost();
        bool isPreview();
        

    }


    public class ContensisClientResolver : IContensisClientResolver
    {
        private ContensisClient? _cachedClient;
        private readonly IRequestContext _requestContext;

        public ContensisClientResolver(IRequestContext rc)
        {
            _requestContext = rc;
        }

        public ContensisClient GetClient()
        {
            // Cache per request (since resolver is scoped)
            if (_cachedClient != null)
                return _cachedClient;

            bool isPreview = _requestContext.IsPreview;
            _cachedClient = ContensisClientFactory.CreateClient(isPreview);
            return _cachedClient;
        }

        public string showHost() { return _requestContext.Host.ToString().ToLower();  } 
        public bool isPreview() { return _requestContext.IsPreview; }



    }
}
