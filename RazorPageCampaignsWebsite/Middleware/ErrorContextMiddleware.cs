namespace RazorPageCampaignsWebsite.Middleware
{
    public class ErrorContextMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Only capture for the original request, not the /Error re-execute
            if (!context.Request.Path.StartsWithSegments("/Error"))
            {
                context.Items["OriginalPath"] =
                    context.Request.Path + context.Request.QueryString;
            }

            await _next(context);
        }
    }

    public static class ErrorContextMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorContext(this IApplicationBuilder app)
            => app.UseMiddleware<ErrorContextMiddleware>();
    }
}
