using Blackpool.Zengenti.CMS.Models.Canvas.Helpers;
using Blackpool.Zengenti.CMS.Models.GenericTypes;
using Content.Modelling.Models.Forms;
using Microsoft.AspNetCore.Html;
using RazorPageCampaignsWebsite.Core.Services.ContentHandling.Interfaces;
using RazorPageCampaignsWebsite.Helpers.Interfaces;
using RazorPageCampaignsWebsite.Helpers.Wrappers;

namespace RazorPageCampaignsWebsite.Core.Services.ContentHandling.Handlers
{
    public class WebFormsHandler : IContentHandler
    {
        private readonly ISerializationHelper _serializer;
        private readonly IFormHelper _formHelper;

        public WebFormsHandler(ISerializationHelper serializer, IFormHelper formHelper)
        {
            _serializer = serializer;
            _formHelper = formHelper;
        }

        public bool CanHandle(string className) => className == typeof(WebForms).Name;

        public async Task<IHtmlContent> HandleAsync(SerialisedItem item)
        {
            var htmlContent = new HtmlContentBuilder();

            try
            {
                // Deserialize the web form content
                var form = await _serializer.DeserializeAsync<WebForms>(item);

                if (form?.Value?.ContentType?.Id != null)
                {
                    // Generate the form HTML
                    var formHtml = _formHelper.TagBuilder("lgwebsite", form.Value.ContentType.Id);
                    htmlContent.AppendHtml(formHtml);
                }
            }
            catch (Exception ex)
            {
                htmlContent.AppendHtml($"<!-- Error processing WebForms item: {ex.Message} -->");
            }

            return htmlContent;
        }
    }
}
