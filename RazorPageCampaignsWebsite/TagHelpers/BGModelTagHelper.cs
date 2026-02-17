using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Content.Modelling.Models.Templates;
using Content.Modelling.Models.Templates.Base;
using RazorPageCampaignsWebsite.ViewModels;
using System.Text;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Zengenti.Contensis.Delivery;
using Content.Modelling.Constants;

namespace RazorPageCampaignsWebsite.TagHelpers
{
    [HtmlTargetElement("bg-model")]
    public class BGModelTagHelper : TagHelper
    {
        [HtmlAttributeName("model")]
        public CampaignDetailsViewModel? Model { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContext { get; set; }


        private IViewComponentHelper? _viewComponentHelper;
        private string _canvasHtml = string.Empty;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (Model?.ConcreteModel == null)
            {
                output.SuppressOutput();
                return;
            }

            try
            {
                // Get IViewComponentHelper service
                _viewComponentHelper ??= ViewContext?.HttpContext.RequestServices.GetRequiredService<IViewComponentHelper>();
                (_viewComponentHelper as IViewContextAware)?.Contextualize(ViewContext);

                var contentBuilder = new StringBuilder();

                // RENDER CANVAS - Using the ViewComponent that works!
                if (Model.ConcreteModel is IHasSerialisedCanvas hasCanvas)
                {
                    var canvasData = hasCanvas.GetSerialisedCanvas();
                    
                    if (canvasData != null)
                    {
                        var canvasContent = await _viewComponentHelper.InvokeAsync("Canvas",canvasData);
                        using (var writer = new System.IO.StringWriter())
                        {
                            canvasContent.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                            _canvasHtml = writer.ToString();
                        }
                    }
                }

                // Clear any existing content
                output.Content.Clear();
                // Set the tag name and attributes
                //output.TagName = "div";
                //output.Attributes.SetAttribute("class", $"bg-model {Model.ContentTypeId}");

                if (Model.ConcreteModel is BGStandard standard)
                {
                    output.PostElement.AppendHtml($@"{_canvasHtml}");
                }

                // Add type-specific content if needed
                if (Model.ConcreteModel is BGStandardWithImages images)
                {
                    output.PostElement.AppendHtml($@"{_canvasHtml}");
                }

                if (Model.ConcreteModel is BGStandardWithDocuments documents)
                {
                    output.PostElement.AppendHtml($@"{_canvasHtml}");
                }

                if (Model.ConcreteModel is BGStandardWithForms forms)
                {
                    output.PostElement.AppendHtml($@"{_canvasHtml}");
                    if (!string.IsNullOrEmpty(forms.FormID)) {

                        string temp = GetLegacyFormEmbed(forms.FormID);
                        output.PostElement.AppendHtml(temp);
                    }
                      
                }
            }
            catch (Exception ex)
            {
                output.TagName = "div";
                output.Attributes.SetAttribute("class", "alert alert-danger");
                output.PostElement.AppendHtml($"<p>Error rendering content: {ex.Message}</p>");
            }


        }

 
        private string RenderFallback(object model)
        {
            return $@"<div class='alert alert-warning'>
                <p>Unknown model type: {model.GetType().Name}</p>
                <p>ContentTypeId: {(model as BaseBG)?.Sys.ContentTypeId ?? "Unknown"}</p>
            </div>";
        }

        // Helper method in your PageModel or base class
        public string GetLegacyFormEmbed(string formId, int height = 900)
        {
            return $@"
            <div class='contensis-form-wrapper contensis-form-{formId}'>
                <iframe 
                    src='https://www.blackpool.gov.uk/Testing/forms/FormInPageRenderer.aspx?id={formId}'
                    width='100%'
                    height='{height}px'
                    style='border:0;'>
                </iframe>
            </div>
            ";


        }
    }
}