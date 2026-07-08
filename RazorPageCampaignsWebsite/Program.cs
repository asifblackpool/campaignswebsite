using Content.Modelling.CMS.Helpers.Errors;
using Content.Modelling.Extensions;
using Content.Modelling.Helpers.Connector;
using Content.Modelling.Helpers.Errors;
using Content.Modelling.Models.Search;
using Content.Modelling.Services;
using DotNetEnv;
using Microsoft.AspNetCore.HttpOverrides;
using RazorPageCampaignsWebsite.Constants;
using RazorPageCampaignsWebsite.Core.Interfaces;
using RazorPageCampaignsWebsite.Core.Services.ContentHandling;
using RazorPageCampaignsWebsite.Core.Services.ContentProcessing.Interfaces;
using RazorPageCampaignsWebsite.Core.Services.Processors;
using RazorPageCampaignsWebsite.Helpers;
using RazorPageCampaignsWebsite.Helpers.Html;
using RazorPageCampaignsWebsite.Helpers.Interfaces;
using RazorPageCampaignsWebsite.Helpers.Renderers;
using RazorPageCampaignsWebsite.Helpers.Renderers.Components;
using RazorPageCampaignsWebsite.Helpers.Serialisation;
using RazorPageCampaignsWebsite.Helpers.Wrappers;
using RazorPageCampaignsWebsite.Infrastructure.Repositories;
using RazorPageCampaignsWebsite.Middleware;
using RazorPageCampaignsWebsite.Services;
using RazorPageCampaignsWebsite.Services.Breadcrumb;
using RazorPageCampaignsWebsite.Services.Interfaces;
using Zengenti.Contensis.Delivery;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables FIRST
DotNetEnv.Env.TraversePath().Load();

// Configure Forwarded Headers to trust the proxy
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                               ForwardedHeaders.XForwardedProto |
                               ForwardedHeaders.XForwardedHost;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
    options.AllowedHosts.Clear();
});

builder.Services.AddScoped<IContensisClientResolver, ContensisClientResolver>();

// Register the concrete ContensisClient so that any class expecting it gets the correct per‑request client
builder.Services.AddScoped<ContensisClient>(sp =>
{
    var resolver = sp.GetRequiredService<IContensisClientResolver>();
    return resolver.GetClient();
});

// Register generic data service (this depends on IContensisClient)
builder.Services.AddTransient(typeof(IDataService<>), typeof(ContensisDataService<>));
builder.Services.AddTransient<IContentRepository, ContensisContentRepository>();

// Register helpers
builder.Services.AddScoped<ISerializationHelper, SerializationHelper>();
builder.Services.AddScoped<ICanvasPanelHelper, CanvasPanelHelperWrapper>();
builder.Services.AddScoped<IPanelHelper, PanelHelperWrapper>();
builder.Services.AddScoped<IParagraphHelper, ParagraphHelperWrapper>();
builder.Services.AddScoped<INavigationLinkHelper, NavigationLinkHelperWrapper>();
builder.Services.AddScoped<IFormHelper, FormHelperWrapper>();
builder.Services.AddScoped<IContentFragmentHelper, ContentFragmentHelper>();
builder.Services.AddScoped<IImageHelper, ImageHelperWrapper>();
builder.Services.AddScoped<ITableHelper, TableHelperWrapper>();
builder.Services.AddScoped<IAccordionRenderer, AccordionRenderer>();
builder.Services.AddScoped<IBgCtaLinkRenderer, BgCtaLinkRenderer>();
builder.Services.AddScoped<IGovUkAccordionWithCtaButtonRenderer, GovUkAccordionWithCtaButtonRenderer>();
builder.Services.AddScoped<IGovUkAccordionWithImagesRenderer, GovUkAccordionWithImagesRenderer>();
builder.Services.AddScoped<IGovUkAccordionRenderer, GovUkAccordionRenderer>();
builder.Services.AddScoped<ViewComponentRenderer>();

// Processors
builder.Services.AddScoped<ITextProcessor, HtmlTextProcessor>();

// Configure logging
builder.Services.AddLogging(configure =>
    configure.AddConsole().SetMinimumLevel(LogLevel.Information));

// Add services to the container
string relativeUrlPath = WebsiteConstants.SITE_VIEW_PATH.TrimEnd('/');
builder.Services
    .AddRazorPages()
    .AddRazorPagesOptions(options =>
    {
        options.Conventions.AddPageRoute("/Home/Index", WebsiteConstants.SITE_VIEW_PATH);
        options.Conventions.AddPageRoute("/Home/Details", WebsiteConstants.SITE_VIEW_PATH + "{*slug}");
        options.Conventions.Add(new GlobalHeaderPageApplicationModelConvention());
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IRequestContext, RequestContext>();
builder.Services.AddScoped<BreadcrumbService>();

// Automatic register all content handlers 
builder.Services.AddContentHandlers();

// Add all content modelling services (one line!)
builder.Services.AddContentModelling(builder.Configuration, options =>
{
    options.DefaultCacheMinutes = 10;           // override default cache
    options.DebugTokenKey = "DebugToken";       // change appsettings key if needed
    options.EnableDebugModeByDefault = false;
});

var app = builder.Build();

// ---------- ADD THIS LINE ----------
app.UseContentModelling();   // Initialises ErrorDisplayHelper.DebugChecker
// ----------------------------------

app.UseForwardedHeaders();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();

// Block everything except static assets and /campaigns
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value ?? "";
    bool isStaticAsset = path.StartsWith("/css/", StringComparison.OrdinalIgnoreCase)
                         || path.StartsWith("/js/", StringComparison.OrdinalIgnoreCase)
                         || path.StartsWith("/images/", StringComparison.OrdinalIgnoreCase)
                         || path.StartsWith("/lib/", StringComparison.OrdinalIgnoreCase);

    if (!isStaticAsset && !path.StartsWith("/campaigns", StringComparison.OrdinalIgnoreCase))
    {
        context.Response.StatusCode = 404;
        return;
    }
    await next();
});

app.UseRouting();
app.UseMiddleware<BreadcrumbMiddleware>();
app.UseStatusCodePagesWithReExecute("/Error");
app.MapRazorPages();

app.Run();

#region static class to create the Contensis Client (unchanged)

public static class ContensisClientFactory
{
    private static readonly object _lock = new object();
    private static bool _envLoaded = false;

    private static void EnsureEnvironmentLoaded()
    {
        if (!_envLoaded)
        {
            lock (_lock)
            {
                if (!_envLoaded)
                {
                    Env.TraversePath().Load();
                    _envLoaded = true;
                }
            }
        }
    }
    public static ContensisClient CreatePreviewClient()
    {
        EnsureEnvironmentLoaded();
        return ContensisClient.Create(
            projectId: Env.GetString("PROJECT_API_ID"),
            rootUrl: string.Format("https://api-{0}.cloud.contensis.com", Env.GetString("ALIAS")),
            clientId: Env.GetString("CONTENSIS_CLIENT_ID"),
            sharedSecret: Env.GetString("CONTENSIS_CLIENT_SECRET"),
            versionStatus: VersionStatus.Latest
        );
    }

    public static ContensisClient CreateLiveClient()
    {
        EnsureEnvironmentLoaded();
        return ContensisClient.Create(
            projectId: Env.GetString("PROJECT_API_ID"),
            rootUrl: string.Format("https://api-{0}.cloud.contensis.com", Env.GetString("ALIAS")),
            clientId: Env.GetString("CONTENSIS_CLIENT_ID"),
            sharedSecret: Env.GetString("CONTENSIS_CLIENT_SECRET"),
            versionStatus: VersionStatus.Published
        );
    }

    public static ContensisClient CreateClient(bool isPreview, string QueryStringversionStatus)
    {
        if (QueryStringversionStatus == "latest")
            return CreatePreviewClient();
        else if (QueryStringversionStatus == "published")
            return CreateLiveClient();
        else
            return isPreview ? CreatePreviewClient() : CreateLiveClient();
    }
}

#endregion