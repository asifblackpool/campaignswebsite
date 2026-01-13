using Microsoft.AspNetCore.Rewrite;
using RazorPageCampaignsWebsite.Constants;
using RazorPageCampaignsWebsite.Core.Interfaces;
using RazorPageCampaignsWebsite.Core.Services.ContentHandling;
using RazorPageCampaignsWebsite.Core.Services.ContentProcessing.Interfaces;
using RazorPageCampaignsWebsite.Core.Services.Processors;
using RazorPageCampaignsWebsite.Helpers;
using RazorPageCampaignsWebsite.Helpers.Html;
using RazorPageCampaignsWebsite.Helpers.Interfaces;
using RazorPageCampaignsWebsite.Helpers.Renderers;
using RazorPageCampaignsWebsite.Helpers.Serialisation;
using RazorPageCampaignsWebsite.Helpers.Wrappers;
using RazorPageCampaignsWebsite.Infrastructure.Repositories;
using RazorPageCampaignsWebsite.Middleware;
using RazorPageCampaignsWebsite.Services;
using RazorPageCampaignsWebsite.Services.Breadcrumb;
using RazorPageCampaignsWebsite.Services.Interfaces;
using Zengenti.Contensis.Delivery;



var builder = WebApplication.CreateBuilder(args);

// Register generic data service
builder.Services.AddTransient(typeof(IDataService<>), typeof(ContensisDataService<>));

// Register IContensisClient as a singleton
builder.Services.AddSingleton<IContensisClient>(sp =>
{
    var realClient = ContensisClient.Create();
    return new ContensisClientWrapper(realClient);
});

//register repositories
builder.Services.AddTransient<IContentRepository, ContensisContentRepository>();

//register helpers
builder.Services.AddScoped<ISerializationHelper, SerializationHelper>();
builder.Services.AddScoped<ICanvasPanelHelper, CanvasPanelHelperWrapper>();
builder.Services.AddScoped<IPanelHelper, PanelHelperWrapper>();
builder.Services.AddScoped<IParagraphHelper, ParagraphHelperWrapper>();
builder.Services.AddScoped<INavigationLinkHelper, NavigationLinkHelperWrapper> ();
builder.Services.AddScoped<IFormHelper, FormHelperWrapper>();
builder.Services.AddScoped<IContentFragmentHelper, ContentFragmentHelper>();
builder.Services.AddScoped<IImageHelper, ImageHelperWrapper>();
builder.Services.AddScoped<ITableHelper, TableHelperWrapper>();
builder.Services.AddScoped<IAccordionRenderer, AccordionRenderer>();
builder.Services.AddScoped<IBgCtaLinkRenderer, BgCtaLinkRenderer>();
builder.Services.AddScoped<IGovUkAccordionWithCtaButtonRenderer, GovUkAccordionWithCtaButtonRenderer>();
builder.Services.AddScoped<IGovUkAccordionWithImagesRenderer, GovUkAccordionWithImagesRenderer>();
builder.Services.AddScoped<IGovUkAccordionRenderer, GovUkAccordionRenderer>();


//Processors
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

        // Override root to always render blog post at '/'
        options.Conventions.AddPageRoute("/Home/Index", WebsiteConstants.SITE_VIEW_PATH);
        options.Conventions.AddPageRoute("/Home/Details", WebsiteConstants.SITE_VIEW_PATH + "{*slug}");


        options.Conventions.Add(new GlobalHeaderPageApplicationModelConvention());
    });

builder.Services.AddScoped<BreadcrumbService>();
builder.Services.AddHttpContextAccessor();

//automatic register all content handlers 
builder.Services.AddContentHandlers();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection(); NO SUPPORT FOR .NET 9,0 
app.UseStaticFiles();

//add rewrite options 
// Configure URL rewriting
var rewriteOptions = new RewriteOptions()
    // Rewrite root path to your specific page
    .AddRewrite(@"^$", WebsiteConstants.SITE_VIEW_PATH.TrimEnd('/'), skipRemainingRules: true);
// Rewrite /venues to the full path
//.AddRewrite(@"^venues$", WebsiteConstants.SITE_VIEW_PATH.TrimEnd('/'), skipRemainingRules: true);

app.UseRewriter(rewriteOptions);




app.UseRouting();
// app.UseHttpLogging(); NO SUPPORT FOR .NET 9,0 
app.UseMiddleware<BreadcrumbMiddleware>();
app.UseStatusCodePagesWithReExecute("/Error");
app.MapRazorPages();

DotNetEnv.Env.TraversePath().Load();

ContensisClient.Configure(
    new ContensisClientConfiguration(
        rootUrl: string.Format("https://api-{0}.cloud.contensis.com", DotNetEnv.Env.GetString("ALIAS")),
        projectApiId: DotNetEnv.Env.GetString("PROJECT_API_ID"),
        clientId: DotNetEnv.Env.GetString("CONTENSIS_CLIENT_ID"),
        sharedSecret: DotNetEnv.Env.GetString("CONTENSIS_CLIENT_SECRET")
    )
);

app.Run();
