using Blackpool.Zengenti.CMS.Models.Canvas.Panels;
using Microsoft.AspNetCore.Html;

namespace RazorPageCampaignsWebsite.Helpers.Interfaces
{
    public interface ICanvasPanelHelper
    {
        IHtmlContent BuildPanel(CanvasPanel panel);
    }
}
