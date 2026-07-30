using RazorPageCampaignsWebsite.Models;
using System.Xml;

namespace RazorPageCampaignsWebsite.Services.Interfaces
{
    public interface IZengentiClient
    {
        Task<List<string>> GetTopLevelSectionNamesAsync();
        Task<CmsNode?> GetNodeByPathAsync(string path);
        Task<List<CmsNode>> GetChildNodesAsync(string parentPath);
    }
}
