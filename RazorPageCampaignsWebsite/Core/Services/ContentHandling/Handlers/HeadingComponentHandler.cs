using Microsoft.AspNetCore.Html;
using Blackpool.Zengenti.CMS.Models.Canvas.Headers;
using Blackpool.Zengenti.CMS.Models.GenericTypes;

using Newtonsoft.Json;
using Blackpool.Zengenti.CMS.Models.Canvas.Helpers;
using RazorPageCampaignsWebsite.Core.Services.ContentHandling.Interfaces;

namespace RazorPageCampaignsWebsite.Core.Services.ContentHandling.Handlers
{
    // Services/ContentHandling/Handlers/HeadingComponentHandler.cs
    public class HeadingComponentHandler : IContentHandler
    {
        private readonly JsonSerializerSettings _settings;

        public HeadingComponentHandler()
        {
            _settings = new JsonSerializerSettings
            {
                Converters = { new HeadingComponentConverter() }
            };
        }

        public bool CanHandle(string className)
            => className == typeof(HeadingComponent).Name;



        public async Task<IHtmlContent> HandleAsync(SerialisedItem item)
        {
            await Task.Yield(); // Ensure async context
            try
            {
                if (!string.IsNullOrEmpty(item.Content))
                {
                    var objItem = JsonConvert.DeserializeObject<HeadingComponent>(item.Content, _settings);
                    return new HtmlString(objItem?.ToHtml() ?? string.Empty);
                }

                return new HtmlString(string.Empty);

            }
            catch
            {
                return new HtmlString(string.Empty);
            }

        }
    }
}
