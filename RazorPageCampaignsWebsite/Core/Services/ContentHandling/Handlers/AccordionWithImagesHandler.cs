using Blackpool.Zengenti.CMS.Constants;
using Blackpool.Zengenti.CMS.Models.Components;
using Blackpool.Zengenti.CMS.Models.GenericTypes;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RazorPageCampaignsWebsite.Core.Services.ContentHandling.Interfaces;
using RazorPageCampaignsWebsite.Helpers.Interfaces;
using RazorPageCampaignsWebsite.Helpers.Renderers;

namespace RazorPageCampaignsWebsite.Core.Services.ContentHandling.Handlers
{
    public class AccordionWithImagesHandler : IContentHandler
    {
        private readonly IHtmlHelper _htmlHelper;
        private readonly ITableHelper _tableHelper;

        public AccordionWithImagesHandler(IHtmlHelper htmlHelper, ITableHelper tableHelper)
        {
            _htmlHelper = htmlHelper;
            _tableHelper = tableHelper;
        }

        public bool CanHandle(string className)
        {
            return className == typeof(AccordionContentWithImages).Name;
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

                var content = item.Content.Trim();
                var jsonObject = JObject.Parse(content);
                var accordionArray = jsonObject[ComponentKeys.ACCORDION_CONTENT_IMAGES];

                var allItems = accordionArray?.ToObject<List<AccordionContentWithImages>>();

                //var accordionContent = JsonConvert.DeserializeObject<AccordionContentWithImages>(allItems);
                if (allItems == null)
                {
                    return htmlContent;
                }

        
                var accordionHtml = _htmlHelper.RenderAccordion(string.Empty, allItems);
                htmlContent.AppendHtml(accordionHtml);

                return htmlContent;
            }
            catch (Exception ex)
            {
                htmlContent.AppendHtml($"<!-- Error processing Accordion With Images Handler: {ex.Message} -->");
                return htmlContent;
            }
        }
    }
}
