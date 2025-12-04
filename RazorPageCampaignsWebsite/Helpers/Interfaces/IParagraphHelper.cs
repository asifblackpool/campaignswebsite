using Blackpool.Zengenti.CMS.Models.Canvas.Paragraphs;
using Microsoft.AspNetCore.Html;

namespace RazorPageCampaignsWebsite.Helpers.Interfaces
{
    // IParagraphHelper.cs
    public interface IParagraphHelper
    {
   
        Task<IHtmlContent> FragmentParagraphAsync(FragmentParagraph fp);
    }
}
