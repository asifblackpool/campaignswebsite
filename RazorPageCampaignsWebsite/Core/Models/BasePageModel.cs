using Blackpool.Zengenti.CMS.Models.Interfaces;
using Blackpool.Zengenti.CMS.Models.Templates;
using Content.Modelling.Models.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageCampaignsWebsite.Core.Interfaces;
using RazorPageCampaignsWebsite.Helpers;
using RazorPageCampaignsWebsite.Services.Breadcrumb;
using RazorPageCampaignsWebsite.Services.Interfaces;

namespace RazorPageCampaignsWebsite.Core.Models
{
    public class BasePageModel<T> : PageModel where T : class
    {
        protected readonly ILogger<BasePageModel<T>> _logger;
        protected readonly IDataService<T> _dataService;
        protected readonly IContentRepository _contentRepository;
        protected readonly BreadcrumbService _breadcrumb;

        public string UrlSiteViewPath
        {
            get
            {
                string? path = HttpContext.Request.Path;
                path = path == null ? string.Empty : path.RemoveFileExtension(FILE_Extension.ASPX);
                return path == null ? string.Empty : path;
            }
        }

        public List<BreadcrumbItem> Breadcrumbs
        {
            get { return _breadcrumb.GetBreadcrumbs(HttpContext); }
        }

        // Shared properties
        public string PageType => typeof(T).Name;
        public List<T> Items { get; protected set; } = new();

        // Constructor with DI
        public BasePageModel(ILogger<BasePageModel<T>> logger,IDataService<T> dataService,
                             IContentRepository contentRepository, BreadcrumbService breadcrumb)
        {
            _logger = logger;
            _dataService = dataService;
            _breadcrumb = breadcrumb;
            _contentRepository = contentRepository;
        }

        // Shared initialization
        public virtual async Task OnGetAsync()
        {
            _logger.LogInformation($"Loading {PageType} data");
            Items = await _dataService.GetAllAsync();
            Reset();

            ViewData["Title"] = $"{PageType}s - {DateTime.Now.Year}";
            StoreModel(Items);
            StoreTitle(Items);
            StoreImageStrip(Items);
        }

        public virtual async Task OnGetByPathAsync(string path)
        {
            _logger.LogInformation($"Loading {PageType} data");
            Items = await _dataService.GetAllAsync(path);
            Reset();
            ViewData["Title"] = $"{PageType}s - {DateTime.Now.Year}";
            StoreModel(Items);
            StoreTitle(Items);
            StoreImageStrip(Items);

        }

        // Example of using content repository inside your page model
        protected List<TChild> GetChildEntries<TChild>(string parentUri) where TChild : class, IPageTemplates
        {
            return _contentRepository.GetChildEntries<TChild>(parentUri);
        }

        // Shared method
        protected void LogAction(string action)
        {
            _logger.LogInformation($"{PageType} action: {action}");
        }

        private void StoreModel(List<T> items)
        {
            ViewData["Model"] = items != null && items.Count > 0 ? items.Take(1).FirstOrDefault() : null;
            var temp = ViewData["Model"];
        }

        private void StoreTitle(List<T> items)
        {
            ViewData["Title"] = $"{PageType}s - {DateTime.Now.Year}";
            var temp = items != null && items.Count > 0 ? (dynamic)items.First() : null;
            if (temp != null)

            {
                try
                {
                    ViewData["Title"] = temp.Title != null ? temp.Title : temp.PageTitle; // Works if T has Title, but throws exception if it doesn't
                }
                catch
                {
                    ViewData["Title"] = null;
                }

            }

        }

        private void StoreImageStrip(List<T> items)


        {
           
        }

        private void Reset()
        {
            ViewData["Title"] = null;
            ViewData["Model"] = null;
            ViewData["ImageStrip"] = null;
        }
    }
}
