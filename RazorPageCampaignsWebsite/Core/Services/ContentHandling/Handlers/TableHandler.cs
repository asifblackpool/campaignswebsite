using Blackpool.Zengenti.CMS.Models.Canvas.Helpers;
using Blackpool.Zengenti.CMS.Models.Canvas.Tables;
using Blackpool.Zengenti.CMS.Models.GenericTypes;
using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;
using RazorPageCampaignsWebsite.Core.Services.ContentHandling.Interfaces;
using RazorPageCampaignsWebsite.Helpers.Interfaces;
using RazorPageCampaignsWebsite.Helpers.Wrappers;

namespace RazorPageCampaignsWebsite.Core.Services.ContentHandling.Handlers
{
    public class TableHandler : IContentHandler
    {
        private readonly ISerializationHelper _serializer;
        private readonly ITableHelper _tableHelper;

        public TableHandler(ISerializationHelper serializer, ITableHelper tableHelper)
        {
            _serializer = serializer;
            _tableHelper = tableHelper;
        }

        string IContentHandler.ContentType => throw new NotImplementedException();

        public bool CanHandle(string className) => className == typeof(Table).Name;

        public async Task<IHtmlContent> HandleAsync(SerialisedItem item)
        {
            var htmlContent = new HtmlContentBuilder();

            try
            {
                // Configure JSON serializer settings
                var settings = new JsonSerializerSettings
                {
                    MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    Converters = { new TableComponentConverter() }
                };

                // Deserialize the table content
                var table = await Task.Run(() =>
                    JsonConvert.DeserializeObject<Table>(item.Content, settings));

                if (table != null)
                {
                    // Build and append the table HTML
                    var tableHtml = _tableHelper.BuildHtmlTable(table);
                    htmlContent.AppendHtml(tableHtml);
                }
            }
            catch (Exception ex)
            {
                htmlContent.AppendHtml($"<!-- Error processing Table handler: {ex.Message} -->");
            }

            return htmlContent;
        }
    }
}
