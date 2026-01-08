using Moq;
using Xunit;
using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Blackpool.Zengenti.CMS.Models.GenericTypes;
using Blackpool.Zengenti.CMS.Models.Canvas.Headers;
using CampaignsWebsite.UnitTests.ContentHandlers.Base;
using RazorPageCampaignsWebsite.Core.Services.ContentHandling.Handlers;

namespace CampaignsWebsite.UnitTests.ContentHandlers
{
    public class HeadingComponentHandlerTests : HandlerTestBase
        {
            private readonly HeadingComponentHandler _handler;

            public HeadingComponentHandlerTests()
            {
                _handler = new HeadingComponentHandler();
            }

            [Fact]
            public void CanHandle_ReturnsTrueForHeadingComponent()
            {
                // Act & Assert
                Assert.True(_handler.CanHandle(typeof(HeadingComponent).Name));
            }

            [Fact]
            public void CanHandle_ReturnsFalseForOtherTypes()
            {
                // Act & Assert
                Assert.False(_handler.CanHandle("OtherType"));
            }

            [Fact]
            public async Task HandleAsync_ReturnsEmptyForInvalidJson()
            {
                // Arrange
                var item = CreateTestItem("{ invalid json }", typeof(HeadingComponent));

                // Act
                var result = await _handler.HandleAsync(item);
                var html = GetHtmlString(result);

                // Assert
                Assert.Empty(html);
            }


            [Fact]
            public async Task HandleAsync_ReturnsEmptyForEmptyContent()
            {
                // Arrange
                var item = CreateTestItem("", typeof(HeadingComponent));

                // Act
                var result = await _handler.HandleAsync(item);
                var html = GetHtmlString(result);

                // Assert
                Assert.Empty(html);
            }

      

            // Test-only class
            public class OtherContentType { }
        }
}
