using Blackpool.Zengenti.CMS.Models.Weddings.Base;
using Microsoft.AspNetCore.Mvc;
using RazorPageCampaignsWebsite.Core.Models;

namespace RazorPageCampaignsWebsite.Components
{
   
    public class TitleViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            string noTitle = "No title";
            var temp = ViewData["Model"] as GettingMarriedBase;
            var model = new LayoutModel
            {
                Title = (temp != null && temp.PageTitle != null) ? temp.PageTitle : noTitle,
                IsHomePage = ViewContext.RouteData.Values["page"]?.ToString() == "/Home/Index"
            };

            if (model.Title == noTitle)
            {
                model.Title = (temp != null && temp?.Title != null) ? temp.Title : noTitle;
        
            }
            return View(model);
        }
    }
}
