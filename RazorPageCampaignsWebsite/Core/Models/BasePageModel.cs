using Content.Modelling.Models.Interfaces;
using Content.Modelling.Models.Templates;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageCampaignsWebsite.Core.Interfaces;
using RazorPageCampaignsWebsite.Helpers;
using RazorPageCampaignsWebsite.Services.Breadcrumb;
using RazorPageCampaignsWebsite.Services.Interfaces;
using Content.Modelling.Constants;
using Content.Modelling.Models.Templates.Base;
using Content.Modelling.Models.GenericTypes;
using System.Text.Json.Serialization;
using System.Text.Json;
using RazorPageCampaignsWebsite.ViewModels;

namespace RazorPageCampaignsWebsite.Core.Models
{
    public class BasePageModel<T> : PageModel where T : class
    {
        public CampaignDetailsViewModel ViewModel { get; set; } = new();

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
   
            StoreTitle(Items);
            StoreImageStrip(Items);
        }

        public virtual async Task OnGetByPathAsync(string path)
        {
            _logger.LogInformation($"Loading {PageType} data");
            Items = await _dataService.GetAllAsync(path);
            Reset();
            ViewData["Title"] = $"{PageType}s - {DateTime.Now.Year}";
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

        // Usage in your method:
        protected void PopulateConcreteModel(List<T> items)
        {
            Helpers.Serialisation.SerializationHelper sh = new Helpers.Serialisation.SerializationHelper();

            if (items?.Count > 0)
            {
                var item = items.First();

                // Always try to get the concrete type
                if (item != null)
                {
                
                    string? content = (item != null) ? item.ToString(): string.Empty;
                    if (content != null)
                    {
                        SerialisedItem temp = new SerialisedItem("baseBGkey", typeof(BaseBG), content, "");
                        BaseBG? baseIem = sh.Deserialize<BaseBG>(temp);
                        if (baseIem != null) 
                        {
                            string contentTypeId = baseIem.Sys.ContentTypeId;
                            
                        var concreteModel = BGTypeResolver.DeserializeToConcreteType(contentTypeId, content);

                            switch (concreteModel)
                            {
                                case BGStandard standard:
                                    ViewData["Model"] = concreteModel as BGStandard;
                                    ViewModel.ConcreteModel = concreteModel as BGStandard;
                                    break;
                                case BGStandardWithImages images:
                                    ViewData["Model"] = concreteModel as BGStandardWithImages;
                                    ViewModel.ConcreteModel = concreteModel as BGStandardWithImages;
                                    break;
                                case BGStandardWithForms forms:
                                    ViewData["Model"] = concreteModel as BGStandardWithForms;
                                    ViewModel.ConcreteModel = concreteModel as BGStandardWithForms;
                                    break;
                                case null:
                                    concreteModel = baseIem;
                                    break;
                            }

                            ViewData["ModelType"] = concreteModel?.GetType().Name;
                            ViewData["ContentTypeId"] = contentTypeId;


                            ViewModel.ContentTypeId = contentTypeId;
                            ViewModel.ModelType = concreteModel?.GetType().Name;
                            ViewModel.OriginalItems = Items as List<dynamic>;
                        }
                    }    
                }
                else
                {
                    ViewData["Model"] = null;
                }
            }

        }

        /// <summary>
        /// Override this in derived pages if they need to do additional processing
        /// </summary>
        protected virtual void OnModelPopulated()
        {
            // Hook for derived classes
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


    #region BT Type Resolver 

    public static class BGTypeResolver
    {
        private static readonly Dictionary<string, Type> _typeMap = new()
        {
            { ContensisClientKeys.BG_STANDARD, typeof(BGStandard) },
            { ContensisClientKeys.BG_STANDARD_WITH_IMAGES, typeof(BGStandardWithImages) },
            { ContensisClientKeys.BG_STANDARD_WITH_FORMS, typeof(BGStandardWithForms) },
            { ContensisClientKeys.BG_STANDARD_WITH_DOCUMENTS, typeof(BGStandardWithDocuments) },
        };

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static Type GetConcreteType(string contentTypeId)
        {
            return _typeMap.TryGetValue(contentTypeId, out var type) ? type : typeof(BaseBG);
        }

        public static object? DeserializeToConcreteType(string contentTypeId, string json)
        {
            if (string.IsNullOrEmpty(json)) return null;

            var concreteType = GetConcreteType(contentTypeId);

            try
            {
                return JsonSerializer.Deserialize(json, concreteType, _jsonOptions);
            }
            catch (JsonException ex)
            {
                // Log exception if needed
                Console.WriteLine($"Failed to deserialize JSON to {concreteType.Name}: {ex.Message}");
                return null;
            }
        }

        public static T? DeserializeToConcreteType<T>(string contentTypeId, string json) where T : BaseBG
        {
            var expectedType = GetConcreteType(contentTypeId);

            if (typeof(T) != expectedType)
            {
                throw new ArgumentException($"Type mismatch: Expected {expectedType.Name} but requested {typeof(T).Name}");
            }

            return DeserializeToConcreteType(contentTypeId, json) as T;
        }

        /// <summary>
        /// Legacy method for backward compatibility
        /// </summary>
        public static object? CastToConcreteType(BaseBG item)
        {
            if (item == null) return null;

            // Instead of Convert.ChangeType, we serialize and deserialize
            var json = JsonSerializer.Serialize(item, _jsonOptions);
            return DeserializeToConcreteType(item.Sys.ContentTypeId, json);
        }
    }

    #endregion


}
