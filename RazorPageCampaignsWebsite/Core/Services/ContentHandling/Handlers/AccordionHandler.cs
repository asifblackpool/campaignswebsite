using Blackpool.Zengenti.CMS.Models.Accordions;
using Blackpool.Zengenti.CMS.Models.GenericTypes;
using Microsoft.AspNetCore.Html;
using RazorPageCampaignsWebsite.Core.Services.ContentHandling.Interfaces;
using RazorPageCampaignsWebsite.Helpers.Interfaces;
using RazorPageCampaignsWebsite.Helpers.Wrappers;

namespace RazorPageCampaignsWebsite.Core.Services.ContentHandling.Handlers
{
    public class AccordionHandler : IContentHandler
    {
        private readonly ISerializationHelper _serializer;
        private readonly IAccordionRenderer _accordionRenderer;

        public AccordionHandler(ISerializationHelper serializer, IAccordionRenderer accordionRenderer)
        {
            _serializer = serializer;
            _accordionRenderer = accordionRenderer;
        }

        public bool CanHandle(string className) => className == typeof(Accordion).Name;

        public async Task<IHtmlContent> HandleAsync(SerialisedItem item)
        {
            var htmlContent = new HtmlContentBuilder();

            try
            {
                // Deserialize the accordion content
                var accordion = await _serializer.DeserializeAsync<Accordion>(item);

                if (accordion != null)
                {
                    // Get accordion content items
                    var accordionList = accordion.GetSerialisedContent();
                    var accordionName = accordion.AccordionName ?? string.Empty;

                    // Render the accordion
                    var renderedAccordion = _accordionRenderer.Render(accordionName, accordionList);
                    htmlContent.AppendHtml(renderedAccordion);
                }
            }
            catch (Exception ex)
            {
                htmlContent.AppendHtml($"<!-- Error processing Accordion Handler: {ex.Message} -->");
            }

            return htmlContent;
        }
    }
}
