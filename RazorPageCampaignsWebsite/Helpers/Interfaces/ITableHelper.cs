using Blackpool.Zengenti.CMS.Models.Canvas.Tables;
using Microsoft.AspNetCore.Html;

namespace RazorPageCampaignsWebsite.Helpers.Interfaces
{
    public interface ITableHelper
    {
        IHtmlContent BuildHtmlTable(Table table);
    }
}
