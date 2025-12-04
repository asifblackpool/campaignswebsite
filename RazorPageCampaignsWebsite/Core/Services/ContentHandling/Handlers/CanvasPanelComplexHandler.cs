using Blackpool.Zengenti.CMS.Models.Canvas.Panels;
using Blackpool.Zengenti.CMS.Models.GenericTypes;
using Microsoft.AspNetCore.Html;
using RazorPageCampaignsWebsite.Core.Services.ContentHandling.Interfaces;
using RazorPageCampaignsWebsite.Helpers.Interfaces;
using RazorPageCampaignsWebsite.Helpers.Wrappers;

namespace RazorPageCampaignsWebsite.Core.Services.ContentHandling.Handlers
{
    public class CanvasPanelComplexHandler : IContentHandler
    {
        private readonly ISerializationHelper _serializer;
        private readonly IPanelHelper _panelHelper;

        public CanvasPanelComplexHandler(ISerializationHelper serializer, IPanelHelper panelHelper)
        {
            _serializer = serializer;
            _panelHelper = panelHelper;
        }

        public bool CanHandle(string className) => className == typeof(CanvasPanelComplex).Name;

        public async Task<IHtmlContent> HandleAsync(SerialisedItem item)
        {
            var htmlContent = new HtmlContentBuilder();

            try
            {
                // Deserialize the panel content
                var panel = await _serializer.DeserializeAsync<CanvasPanelComplex>(item);

                if (panel != null)
                {
                    // Build and append the panel HTML
                    var panelHtml = _panelHelper.BuildPanel(panel);
                    htmlContent.AppendHtml(panelHtml);
                }
            }
            catch (Exception ex)
            {
                htmlContent.AppendHtml($"<!-- Error processing CanvasPanelComplex handler: {ex.Message} -->");
            }

            return htmlContent;
        }
    }
}
