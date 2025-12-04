using Blackpool.Zengenti.CMS.Models.Canvas.Panels;
using Microsoft.AspNetCore.Html;

namespace RazorPageCampaignsWebsite.Helpers.Interfaces
{
    public interface IPanelHelper
    {
        IHtmlContent BuildPanel(CanvasPanelComplex panel);
    }
}
