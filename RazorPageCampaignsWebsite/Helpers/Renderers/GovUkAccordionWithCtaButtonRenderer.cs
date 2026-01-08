// Helpers/Renderers/GovUkAccordionRendererWithCtaButton.cs
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Blackpool.Zengenti.CMS.Models.Components;
using System.Text.Encodings.Web;
using RazorPageCampaignsWebsite.Helpers.Interfaces;

namespace RazorPageCampaignsWebsite.Helpers.Renderers
{
    public class GovUkAccordionWithCtaButtonRenderer : IGovUkAccordionWithCtaButtonRenderer
    {
        private readonly IHtmlHelper _htmlHelper;
        private readonly HtmlEncoder _htmlEncoder;
        private readonly ILogger<GovUkAccordionWithCtaButtonRenderer> _logger;

        public GovUkAccordionWithCtaButtonRenderer(
            IHtmlHelper htmlHelper,
            HtmlEncoder htmlEncoder,
            ILogger<GovUkAccordionWithCtaButtonRenderer> logger)
        {
            _htmlHelper = htmlHelper;
            _htmlEncoder = htmlEncoder;
            _logger = logger;
        }

        public IHtmlContent RenderGovUkAccordion(
            string accordionTitle,
            List<AccordionWithCTAContent> items,
            GovUkAccordionOptions? options = null)
        {
            if (items == null || !items.Any())
            {
                _logger.LogWarning("No accordion items provided for rendering");
                return HtmlString.Empty;
            }

            options ??= new GovUkAccordionOptions();

            // Generate accordion ID if not provided
            if (string.IsNullOrEmpty(options.AccordionId))
            {
                options.AccordionId = $"accordion-{SanitizeId(accordionTitle)}-{Guid.NewGuid():N}";
            }

            var accordionContainer = CreateAccordionContainer(options, accordionTitle);

            int counter = 1;
            foreach (var item in items)
            {
                var sectionHtml = RenderAccordionSection(item, accordionTitle, counter);
                accordionContainer.InnerHtml.AppendHtml(sectionHtml);
                counter++;
            }

            return accordionContainer;
        }

        private TagBuilder CreateAccordionContainer(GovUkAccordionOptions options, string accordionTitle)
        {
            var accordionContainer = new TagBuilder("div");
            accordionContainer.AddCssClass(options.ContainerClass);

            if (!string.IsNullOrEmpty(options.AdditionalContainerClasses))
            {
                accordionContainer.AddCssClass(options.AdditionalContainerClasses);
            }

            accordionContainer.Attributes["data-module"] = options.ModuleName;
            accordionContainer.Attributes["id"] = options.AccordionId;

            if (options.RememberExpandedState)
            {
                accordionContainer.Attributes["data-remember-expanded"] = "true";
            }

            // Add custom container attributes
            foreach (var attr in options.ContainerAttributes)
            {
                if (!accordionContainer.Attributes.ContainsKey(attr.Key))
                {
                    accordionContainer.Attributes[attr.Key] = attr.Value;
                }
            }

            // Add data attribute for title (optional, can be useful for JS)
            accordionContainer.Attributes["data-title"] = accordionTitle;

            return accordionContainer;
        }

        private IHtmlContent RenderAccordionSection(
            AccordionWithCTAContent item,
            string accordionTitle,
            int counter)
        {
            var sectionDiv = new TagBuilder("div");
            sectionDiv.AddCssClass("govuk-accordion__section");

            // First section expanded by default (GOV.UK pattern)
            if (counter == 1)
            {
                sectionDiv.AddCssClass("govuk-accordion__section--expanded");
            }

            // Section Header
            var headerDiv = new TagBuilder("div");
            headerDiv.AddCssClass("govuk-accordion__section-header");
            sectionDiv.InnerHtml.AppendHtml(headerDiv);

            // Heading
            var heading = new TagBuilder("h2");
            heading.AddCssClass("govuk-accordion__section-heading");
            headerDiv.InnerHtml.AppendHtml(heading);

            // Button/Span
            var buttonSpan = new TagBuilder("span");
            buttonSpan.AddCssClass("govuk-accordion__section-button");
            buttonSpan.Attributes["id"] = $"accordion-{SanitizeId(accordionTitle)}-heading-{counter}";
            buttonSpan.InnerHtml.Append(item.Title ?? $"Section {counter}");
            heading.InnerHtml.AppendHtml(buttonSpan);

            // Content Div
            var contentDiv = new TagBuilder("div");
            contentDiv.AddCssClass("govuk-accordion__section-content");
            contentDiv.Attributes["id"] = $"accordion-{SanitizeId(accordionTitle)}-content-{counter}";
            contentDiv.Attributes["aria-labelledby"] = $"accordion-{SanitizeId(accordionTitle)}-heading-{counter}";

            // Hide all sections except first (GOV.UK default)
            if (counter != 1)
            {
                contentDiv.Attributes["hidden"] = "hidden";
                contentDiv.Attributes["aria-hidden"] = "true";
            }
            else
            {
                contentDiv.Attributes["aria-hidden"] = "false";
            }

            sectionDiv.InnerHtml.AppendHtml(contentDiv);

            // Body Content
            var bodyDiv = new TagBuilder("div");
            bodyDiv.AddCssClass("govuk-body");
            contentDiv.InnerHtml.AppendHtml(bodyDiv);

            // Main Content - safely handle HTML
            if (!string.IsNullOrEmpty(item.BodyContent))
            {
                // Use HtmlString to preserve HTML markup
                bodyDiv.InnerHtml.AppendHtml(new HtmlString(item.BodyContent));
            }

            // CTA Button if present
            if (item.CtaAccordionButton != null)
            {
                bodyDiv.InnerHtml.AppendHtml(new HtmlString("<br />"));
                var buttonHtml = RenderCtaButton(item.CtaAccordionButton, accordionTitle, counter);
                bodyDiv.InnerHtml.AppendHtml(buttonHtml);
            }

            return sectionDiv;
        }

        private IHtmlContent RenderCtaButton(AccordionButton button, string accordionTitle, int counter)
        {
            var anchor = new TagBuilder("a");
            anchor.AddCssClass("cta-link-button");
            anchor.AddCssClass("accessibility-tab");

            // Encode the URL for safety
            var encodedUrl = _htmlEncoder.Encode(button.Url);
            anchor.Attributes["href"] = encodedUrl;

            // Add onclick handler as per your HTML example
            anchor.Attributes["onclick"] = $"window.location.href='{encodedUrl}'";

            // Accessibility attributes
            anchor.Attributes["role"] = "button";
            anchor.Attributes["tabindex"] = "0";
            anchor.Attributes["aria-label"] = $"{_htmlEncoder.Encode(button.Text)} (click to navigate)";

            // Add data attributes for tracking/identification
            anchor.Attributes["data-accordion-title"] = _htmlEncoder.Encode(accordionTitle);
            anchor.Attributes["data-section-index"] = counter.ToString();
            anchor.Attributes["data-button-type"] = "accordion-cta";

            anchor.InnerHtml.Append(_htmlEncoder.Encode(button.Text));
            return anchor;
        }

        private string SanitizeId(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "default";

            // Remove spaces and special characters for ID safety
            // Keep it simple and consistent with your existing patterns
            return System.Text.RegularExpressions.Regex.Replace(
                input.ToLowerInvariant(),
                @"[^a-z0-9\-]",
                "-"
            ).Trim('-');
        }
    }
}