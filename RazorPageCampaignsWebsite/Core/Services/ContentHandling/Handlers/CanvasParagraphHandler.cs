using Blackpool.Zengenti.CMS.Helpers;
using Blackpool.Zengenti.CMS.Models.Canvas.Images;
using Blackpool.Zengenti.CMS.Models.Canvas.Paragraphs;
using Blackpool.Zengenti.CMS.Models.GenericTypes;
using Microsoft.AspNetCore.Html;
using RazorPageCampaignsWebsite.Core.Services.ContentHandling.Interfaces;
using RazorPageCampaignsWebsite.Core.Services.ContentProcessing.Interfaces;
using RazorPageCampaignsWebsite.Helpers.Serialisation;
using RazorPageCampaignsWebsite.Helpers.Wrappers;

namespace RazorPageCampaignsWebsite.Core.Services.ContentHandling.Handlers
{
    public class CanvasParagraphHandler : IContentHandler
    {
        private readonly ISerializationHelper _serializer;
        private readonly ITextProcessor _textProcessor;

        public CanvasParagraphHandler(ISerializationHelper serializer, ITextProcessor textProcessor)
        {
            _serializer = serializer;
            _textProcessor = textProcessor;
        }

        public bool CanHandle(string className) => className == typeof(CanvasParagraph).Name;

        public async Task<IHtmlContent> HandleAsync(SerialisedItem item)
        {
            var objItem = await _serializer.DeserializeAsync<CanvasParagraph>(item);

            if (objItem != null)
            {
                var processedText = await _textProcessor.ProcessAsync(objItem?.Value);
                if (objItem?.Properties != null && objItem.Properties.ParagraphType == "lead")
                {
                    return new HtmlString($"<p class=\"shade-black lead-paragraph\">{processedText}</p>");
                }
                else
                {
                    return new HtmlString($"<p class=\"shade-black\">{processedText}</p>");
                }

            }


            return new HtmlString($"");
        }
    }
}
