using Blackpool.Zengenti.CMS.Models.Canvas.Images;
using Blackpool.Zengenti.CMS.Models.GenericTypes;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using RazorPageCampaignsWebsite.Core.Services.ContentHandling.Interfaces;
using RazorPageCampaignsWebsite.Helpers.Interfaces;

namespace RazorPageCampaignsWebsite.Core.Services.ContentHandling.Handlers
{
    // Services/ContentHandling/Handlers/ImageDataHandler.cs
    public class ImageDataHandler : IContentHandler
    {
        private readonly IImageHelper _imageHelper;

        public ImageDataHandler(IImageHelper imageHelper)
        {
            _imageHelper = imageHelper;
        }

        string IContentHandler.ContentType => throw new NotImplementedException();

        public bool CanHandle(string className)
            => className == typeof(ImageData).Name;

        public async Task<IHtmlContent> HandleAsync(SerialisedItem item)
        {

            try
            {
                var imageData = JsonConvert.DeserializeObject<ImageData>(item.Content);
                if (imageData?.Value == null) return HtmlString.Empty;

                var imageUrl = await _imageHelper.GetImageUrlAsync(imageData);
                if (string.IsNullOrEmpty(imageUrl)) return HtmlString.Empty;

                var imgTag = new TagBuilder("img");
                imgTag.Attributes.Add("src", imageUrl);
                imgTag.Attributes.Add("alt", imageData.Value.AltText ?? "");
                imgTag.AddCssClass("img-responsive");

                var container = new TagBuilder("div");
                container.AddCssClass("img-container");
                container.InnerHtml.AppendHtml(imgTag);

                return container;
            }
            catch (Exception ex)
            {
                return new HtmlString(ex.Message);
            }

        }
    }
}

