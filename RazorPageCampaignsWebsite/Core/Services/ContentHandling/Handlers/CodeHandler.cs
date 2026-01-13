using Blackpool.Zengenti.CMS.Models.Canvas.Code;
using Blackpool.Zengenti.CMS.Models.GenericTypes;
using Microsoft.AspNetCore.Html;
using RazorPageCampaignsWebsite.Core.Services.ContentHandling.Interfaces;
using RazorPageCampaignsWebsite.Helpers;
using RazorPageCampaignsWebsite.Helpers.Wrappers;

namespace RazorPageCampaignsWebsite.Core.Services.ContentHandling.Handlers
{
    public class CodeHandler : IContentHandler
    {
        private readonly ISerializationHelper _serializer;

        public CodeHandler(ISerializationHelper serializer)
        {
            _serializer = serializer;
        }

        public bool CanHandle(string className) => className == typeof(Code).Name;

        public async Task<IHtmlContent> HandleAsync(SerialisedItem item)
        {
            var htmlContent = new HtmlContentBuilder();

            try
            {
                // Deserialize the code content
                var codeItem = await _serializer.DeserializeAsync<Code>(item);

                if (codeItem?.Value?.Code != null)
                {
                    var code = codeItem.Value.Code.Trim();
                    var urlType = UrlTypeHelper.GetUrlType(code);

                    switch (urlType)
                    {
                        case UrlTypeHelper.UrlType.GoogleMaps:
                            var embedCode = UrlTypeHelper.GetGoogleMapsEmbedCode(code);
                            htmlContent.AppendHtml(embedCode);
                            break;

                        case UrlTypeHelper.UrlType.YouTube:
                        case UrlTypeHelper.UrlType.Vimeo:
                            var videoEmbed = UrlTypeHelper.GetEmbedCode(code);
                            htmlContent.AppendHtml($"<div class=\"video-container\">{videoEmbed}</div>");
                            break;

                        case UrlTypeHelper.UrlType.Image:
                            htmlContent.AppendHtml($"<img src=\"{code}\" alt=\"\" class=\"embedded-image\" />");
                            break;

                        case UrlTypeHelper.UrlType.PDF:
                            htmlContent.AppendHtml($"<a href=\"{code}\" target=\"_blank\" class=\"pdf-link\">View PDF</a>");
                            break;

                        default:
                            // For unknown or non-embedable URLs, just display as link
                            htmlContent.AppendHtml($"<a href=\"{code}\" target=\"_blank\">{code}</a>");
                            break;
                    }


                    // Add line breaks after the code block
                    htmlContent.AppendHtml("<br /><br />");
                }

                if (codeItem?.Value?.Code == null)
                {
                    htmlContent.AppendHtml($"<!-- Error item is null -->");
                }
            }
            catch (Exception ex)
            {
                htmlContent.AppendHtml($"<!-- Error processing Code Handler: {ex.Message} -->");
            }

            return htmlContent;
        }
    }

}
