using Microsoft.AspNetCore.Mvc;


namespace RazorPageCampaignsWebsite.Components.WebForms
{
    public class WebFormsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Content.Modelling.Models.Forms.WebForms frm)
        {
            // You can pass additional data via ViewData if needed
            ViewData["RenderTime"] = DateTime.Now;

            if (frm != null)
            {
              
                    return View(frm);
                
            }
            return View(null);
        }
    }


}
