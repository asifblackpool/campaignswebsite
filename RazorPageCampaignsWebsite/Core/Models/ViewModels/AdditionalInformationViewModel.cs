using Content.Modelling.Models.AssetGallery;
using Content.Modelling.Models.Components.Data;
using Content.Modelling.Models.Templates.Base;

namespace RazorPageCampaignsWebsite.Core.Models.ViewModels
{
    // ViewModel for the component
    public class AdditionalInformationViewModel
    {

        public List<DataNavigationLink> DataNavigationLinks { get; set; } = new();
        public List<Asset> Assets { get; set; } = new();
        public List<BaseBG> LinkedEntries { get; set; } = new();
        public string Url { get; set; } = string.Empty;

        public bool HasContent =>
            DataNavigationLinks.Any()
            || Assets.Any()
            || LinkedEntries.Any()
            || !string.IsNullOrEmpty(Url);
    }
}
