using Blackpool.Zengenti.CMS.Models.Components;
using Microsoft.AspNetCore.Html;

namespace RazorPageCampaignsWebsite.Helpers.Interfaces
{
    public interface IAccordionRenderer
    {
        IHtmlContent Render(string accordionName, List<AccordionContent> items);
    }
}
