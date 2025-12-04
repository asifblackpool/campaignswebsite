using Blackpool.Zengenti.CMS.Models.Canvas.Helpers;
using Blackpool.Zengenti.CMS.Models.Canvas.Panels;
using Microsoft.AspNetCore.Html;
using RazorPageCampaignsWebsite.Helpers.Interfaces;
using System.Web;

namespace RazorPageCampaignsWebsite.Helpers
{
    public class PanelHelperWrapper : IPanelHelper
    {
        public IHtmlContent BuildPanel(CanvasPanelComplex panel)
        {
            string htmlString = PanelHelper.BuildPanel(panel); 
            return new HtmlString(htmlString);
        }
    }
}
