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

        public async Task InvokeAsync(HttpContext context, BreadcrumbService breadcrumbService, ILogger<BreadcrumbMiddleware> logger)
        {
            logger.LogInformation("BreadcrumbMiddleware starting");

            try
            {
                breadcrumbService.Reset();
                logger.LogInformation("Breadcrumbs reset successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to reset breadcrumbs");
                // Continue anyway
            }

            await _next(context);
            logger.LogInformation("BreadcrumbMiddleware completed");
        }
    }
}


