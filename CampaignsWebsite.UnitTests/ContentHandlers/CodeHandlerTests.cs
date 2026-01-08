using Blackpool.Zengenti.CMS.Models.Canvas.Code;
using Moq;
using RazorPageCampaignsWebsite.Core.Services.ContentHandling.Handlers;
using RazorPageCampaignsWebsite.Helpers.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CampaignsWebsite.UnitTests.ContentHandlers.Base;
using Xunit;

namespace CampaignsWebsite.UnitTests.ContentHandlers
{
    public class CodeHandlerTests : HandlerTestBase
    {
        private readonly Mock<ISerializationHelper> _serializerMock;
        private readonly CodeHandler _handler;

        public CodeHandlerTests()
        {
            _serializerMock = CreateSerializerMock();
            _handler = new CodeHandler(_serializerMock.Object);
        }

        [Fact]
        public async Task HandleAsync_WithValidCode_ReturnsRawHtml()
        {
            // Arrange
            var item = CreateTestItem(
                content: "{\"Value\":{\"Code\":\"<iframe src='test'></iframe>\"}}",
                type: typeof(Code));

            _serializerMock.Setup(x => x.DeserializeAsync<Code>(item))
                         .ReturnsAsync(new Code
                         {
                             Value = new CodeValue
                             {
                                 Code = "<iframe src='test'></iframe>"
                             }
                         });

            // Act
            var result = await _handler.HandleAsync(item);
            var html = GetHtmlString(result);

            // Assert
            Assert.Equal("<iframe src='test'></iframe><br /><br />", html);
        }

        [Fact]
        public async Task HandleAsync_WithNullCode_ReturnsEmpty()
        {
            // Arrange
            var item = CreateTestItem("{}", typeof(Code));
            _serializerMock.Setup(x => x.DeserializeAsync<Code>(item))
                         .ReturnsAsync(new Code { Value = null });

            // Act
            var result = await _handler.HandleAsync(item);
            var html = GetHtmlString(result);

            // Assert
            Assert.Contains("Error item is null", html);

        }

        [Fact]
        public async Task HandleAsync_WithInvalidJson_ReturnsErrorComment()
        {
            // Arrange
            var item = CreateTestItem("invalid json", typeof(Code));

            // Act
            var result = await _handler.HandleAsync(item);
            var html = GetHtmlString(result);

            // Assert
            Assert.Contains("Error", html);
        }
    }
}
