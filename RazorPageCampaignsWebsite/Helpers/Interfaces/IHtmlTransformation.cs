using HtmlAgilityPack;

namespace RazorPageCampaignsWebsite.Helpers.Interfaces
{
    public interface IHtmlTransformation
    {
        Task ApplyAsync(HtmlDocument document);
    }
}
