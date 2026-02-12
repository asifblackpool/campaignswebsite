using Content.Modelling.Models.Components.Data;
using Content.Modelling.Models.Templates.Base;
using Content.Modelling.Models.Templates;
using Microsoft.AspNetCore.Mvc;
using Content.Modelling.Models.AssetGallery;
using RazorPageCampaignsWebsite.Core.Models.ViewModels;
using RazorPageCampaignsWebsite.Core.Interfaces;
using Zengenti.Contensis.Delivery;

namespace RazorPageCampaignsWebsite.Components
{
    public class AdditionalInformationViewComponent : ViewComponent
    {
        private readonly ContensisClient _contensisClient;

        public AdditionalInformationViewComponent(ContensisClient contensisClient)
        {
            _contensisClient = contensisClient;
        }

        public IViewComponentResult Invoke(BaseBG? model)
        {

            // Try to get model from parameter first
            if (model == null)
            {
                // Try to get from ViewData
                model = ViewData["Model"] as BaseBG;
                // Try to get from ViewBag
                if (model == null && ViewBag.Model is BaseBG viewBagModel)
                {
                    model = viewBagModel;
                }
            }

            // If still null, try to get from ViewContext
            if (model == null)
            {
                model = ViewContext.ViewData.Model as BaseBG;
            }

            if (model == null || !(model is BGStandard bgStandard))
                return Content(string.Empty);



            // Cast to BGStandard to access the properties
            var temp = model as BGStandard;

            if (temp == null)
                return Content(string.Empty); // Return empty if not the right type

            // Create view model with only the data needed for the sidebar
            var viewModel = new AdditionalInformationViewModel
            {
              
                Assets = temp.Assets ?? new List<Asset>(),
                DataNavigationLinks = temp.GetDataNavigationLinks ?? new List<DataNavigationLink>(),
                LinkedEntries = temp.GetReferencedEntries(_contensisClient,1, null)
              
            };

            return View(viewModel);
        }
    }
}
