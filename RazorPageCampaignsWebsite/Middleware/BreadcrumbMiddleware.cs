using RazorPageCampaignsWebsite.Services;
using RazorPageCampaignsWebsite.Services.Breadcrumb;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Zengenti.Contensis.Delivery;


namespace RazorPageCampaignsWebsite.Middleware
{
    // Middleware/BreadcrumbMiddleware.cs
    public class BreadcrumbMiddleware
    {
        private readonly RequestDelegate _next;

        public BreadcrumbMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, BreadcrumbService breadcrumbService)
        {
            // Reset and enable auto-generation by default
            breadcrumbService.Reset();

            await _next(context);
        }
    }
}


