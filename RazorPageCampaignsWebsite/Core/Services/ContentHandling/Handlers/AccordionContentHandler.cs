using Blackpool.Zengenti.CMS.Models.Components;
using Blackpool.Zengenti.CMS.Models.GenericTypes;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using RazorPageCampaignsWebsite.Core.Services.ContentHandling.Interfaces;
using RazorPageCampaignsWebsite.Helpers;
using RazorPageCampaignsWebsite.Helpers.Interfaces;

namespace RazorPageCampaignsWebsite.Core.Services.ContentHandling.Handlers
{
    public class AccordionContentHandler : IContentHandler
    {
        private readonly IHtmlHelper _htmlHelper;
        private readonly ITableHelper _tableHelper;

        public AccordionContentHandler(IHtmlHelper htmlHelper, ITableHelper tableHelper)
        {
            _htmlHelper = htmlHelper;
            _tableHelper = tableHelper;
        }

        public bool CanHandle(string className)
        {
            return className == typeof(AccordionContent).Name;
        }

        public async Task<IHtmlContent> HandleAsync(SerialisedItem item)
        {
            var htmlContent = new HtmlContentBuilder();

            if (!CanHandle(item?.ClassName) || string.IsNullOrEmpty(item.Content))
            {
                return htmlContent;
            }

            try
            {
                await Task.Delay(100); // Example delay
                var accordionContent = JsonConvert.DeserializeObject<AccordionContent>(item.Content);
                if (accordionContent == null)
                {
                    return htmlContent;
                }

                // Render accordion
                var accordionList = new List<AccordionContent> { accordionContent };
                var accordionHtml = _htmlHelper.RenderAccordion(string.Empty, accordionList);
                htmlContent.AppendHtml(accordionHtml);

                return htmlContent;
            }
            catch (Exception ex)
            {
                htmlContent.AppendHtml($"<!-- Error processing Accordion Content Handler: {ex.Message} -->");
                return htmlContent;
            }
        }
    }
}
