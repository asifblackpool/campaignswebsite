using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace CampaignsWebsite.UnitTests.Helpers
{
    public static class TagBuilderExtensions
    {
        public static string Render(this TagBuilder tagBuilder)
        {
            using var writer = new StringWriter();
            tagBuilder.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }

        public static void ShouldRenderAs(this TagBuilder tagBuilder, string expectedHtml)
        {
            var actual = tagBuilder.Render();
            actual.Should().Be(expectedHtml);
        }
    }
}
