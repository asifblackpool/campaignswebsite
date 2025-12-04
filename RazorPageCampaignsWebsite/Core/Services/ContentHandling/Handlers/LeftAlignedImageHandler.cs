using Blackpool.Zengenti.CMS.Models.Canvas.Helpers;
using Blackpool.Zengenti.CMS.Models.Canvas.Images.Base;
using Blackpool.Zengenti.CMS.Models.GenericTypes;
using Content.Modelling.Models.Canvas.Images;
using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;
using RazorPageCampaignsWebsite.Core.Services.ContentHandling.Interfaces;
using RazorPageCampaignsWebsite.Helpers.Interfaces;


namespace RazorPageCampaignsWebsite.Core.Services.ContentHandling.Handlers
{
    public class LeftAlignedImageHandler : IContentHandler
    {
        private readonly IImageHelper _imageHelper;

        public LeftAlignedImageHandler(IImageHelper imageHelper)
        {
            _imageHelper = imageHelper;
        }

        public bool CanHandle(string className)
        {
            return className == typeof(LeftAlignedImage).Name;
        }

        public async Task<IHtmlContent> HandleAsync(SerialisedItem item)
        {
            if (item?.Content == null)
                return HtmlString.Empty;

            try
            {
                await Task.Delay(100); // Example delay
                var settings = new JsonSerializerSettings
                {
                    Converters = new List<JsonConverter> { new CmsImageConverter() },
                    NullValueHandling = NullValueHandling.Ignore
                };

                LeftAlignedImage? result = JsonConvert.DeserializeObject<LeftAlignedImage>(item.Content, settings);

                if (result?.Image?.Asset?.Sys?.Uri != null)
                {
                    string? tempUrl = _imageHelper.GetImageUrl(uri: result.Image.Asset.Sys.Uri);
                    if (!string.IsNullOrEmpty(tempUrl))
                    {
                        result.Image.Asset.Sys.Uri = tempUrl;
                    }
                }

                return result != null ? new HtmlString(result.ToHtml()) : HtmlString.Empty;
            }
            catch (Exception ex)
            {
                // Log error here
                return new HtmlString($"<!-- Error processing content: {ex.Message} -->");
            }
        }
    }
}
