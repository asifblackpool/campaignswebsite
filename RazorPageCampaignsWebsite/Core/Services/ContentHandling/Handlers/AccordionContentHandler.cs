// Core/Services/ContentHandling/Handlers/AccordionWithCtaHandler.cs (Simplified)
using Blackpool.Zengenti.CMS.Constants;
using Blackpool.Zengenti.CMS.Models.Components;
using Blackpool.Zengenti.CMS.Models.GenericTypes;
using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RazorPageCampaignsWebsite.Core.Services.ContentHandling.Interfaces;
using RazorPageCampaignsWebsite.Helpers.Interfaces;
using RazorPageCampaignsWebsite.Helpers.Wrappers;

namespace RazorPageCampaignsWebsite.Core.Services.ContentHandling.Handlers
{
    public class AccordionContentHandler : IContentHandler
    {
        private readonly IGovUkAccordionRenderer _accordionRenderer;

        public AccordionContentHandler(IGovUkAccordionRenderer accordionRenderer)
        {
            _accordionRenderer = accordionRenderer;
        }

        public bool CanHandle(string className)
        {
            return className == typeof(AccordionContent).Name;
        }

        public async Task<IHtmlContent> HandleAsync(SerialisedItem item)
        {
            if (string.IsNullOrEmpty(item.Content))
                return HtmlString.Empty;

            try
            {
                List<IGovUkAccordionRenderer> accordionItems;
                // Do this:


                // Get all items:

                var content = item.Content.Trim();
                var jsonObject = JObject.Parse(content);
                var accordionArray = jsonObject[ComponentKeys.ACCORDION_CONTENT];

                // Get all items:
                var allItems = accordionArray?.ToObject<AccordionContent>();


                if (allItems?.Body == string.Empty)
                    return HtmlString.Empty;

                string accordionTitle = allItems?.Title ?? "Accordion";

                bool rememberExpanded = allItems.IsExpanded;

                var options = new GovUkAccordionOptions
                {
                    AccordionId = $"accordion-{Guid.NewGuid():N}",
                    RememberExpandedState = rememberExpanded
                };

                if (allItems == null)
                    return HtmlString.Empty;

                List<AccordionContent> accordionContentList = new List<AccordionContent>();
                accordionContentList.Add(allItems);
                var temp = _accordionRenderer.RenderGovUkAccordion(accordionTitle, accordionContentList, options);

                return temp;
            }
            catch
            {
                return new HtmlString("<!-- Error rendering accordion content -->");
            }
        }
    }
}