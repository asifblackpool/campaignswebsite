using Content.Modelling.Models.Components.Data;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorPageCampaignsWebsite.Helpers.Interfaces;

namespace RazorPageCampaignsWebsite.Helpers.Renderers
{
    public class BgCtaLinkRenderer : IBgCtaLinkRenderer
    {
        private readonly IHtmlHelper _htmlHelper;

        // Inject IHtmlHelper in the constructor
        public BgCtaLinkRenderer(IHtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }

        public async Task<IHtmlContent> RenderBgCtaButtonAsync(BGCTALink cta, string buttonClass)
        {
            if (cta == null)
                return HtmlString.Empty;

            // Call the extension method using the injected IHtmlHelper
            await Task.Delay(100); // Example delay
            return _htmlHelper.RenderBgCtaButton(cta, buttonClass);
        }
    }
}
