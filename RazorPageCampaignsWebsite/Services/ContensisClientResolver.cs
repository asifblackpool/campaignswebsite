
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

        public ContensisClientResolver(IRequestContext rc)
        {
            _requestContext = rc;
        }

        public ContensisClient GetClient()
        {
            // Cache per request (since resolver is scoped)
            //if (_cachedClient != null)
            //    return _cachedClient;

            bool isPreview = showVersionStatus == "latest" ? true : false;
            string versionStatus = showVersionStatus;

            string? temp = _requestContext.GetQueryStringVersionStatus();

            if (temp != null)
            {
                _cachedClient = ContensisClientFactory.CreateClient(isPreview, temp);
            }
            else 
            {
              _cachedClient = ContensisClientFactory.CreateClient(isPreview, showVersionStatus);

            }
          
            return _cachedClient;
        }
        /// <summary>
        ///  if version status is latest
        /// </summary>
        public string showVersionStatus
        {   get
            {
                string entryVersionStatus = _requestContext.Headers.TryGetValue("x-entry-versionstatus", out var values) 
                    ? values.FirstOrDefault() ?? "found" : "not found";
                return entryVersionStatus;
            }
        }
           
        public string showHost { get { return _requestContext.Host.ToString().ToLower(); } }
        public bool isPreview { get { return _requestContext.IsPreview; } }
        



    }
}
