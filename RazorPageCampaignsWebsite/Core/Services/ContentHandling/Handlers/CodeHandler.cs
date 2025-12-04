using Blackpool.Zengenti.CMS.Models.Canvas.Code;
using Blackpool.Zengenti.CMS.Models.GenericTypes;
using Microsoft.AspNetCore.Html;
using RazorPageCampaignsWebsite.Core.Services.ContentHandling.Interfaces;
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
                    // Add the raw code content
                    htmlContent.AppendHtml(codeItem.Value.Code);

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
