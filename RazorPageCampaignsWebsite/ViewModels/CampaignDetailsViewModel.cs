using Content.Modelling.Models.Templates.Base;

namespace RazorPageCampaignsWebsite.ViewModels
{
    public class CampaignDetailsViewModel
    {
        public BaseBG? ConcreteModel { get; set; }
        public string? ContentTypeId { get; set; }
        public string? ModelType { get; set; }
        public List<dynamic>? OriginalItems { get; set; }
    }
}
