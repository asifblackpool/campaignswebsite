using Content.Modelling.Models.Forms;
using Microsoft.AspNetCore.Html;
using Moq;
using RazorPageCampaignsWebsite.Core.Services.ContentHandling.Handlers;
using RazorPageCampaignsWebsite.Helpers.Interfaces;
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
    public class WebFormsHandlerTests : HandlerTestBase
    {
        private readonly Mock<ISerializationHelper> _serializerMock;
        private readonly Mock<IFormHelper> _formHelperMock;
        private readonly WebFormsHandler _handler;

        public WebFormsHandlerTests()
        {
            _serializerMock = CreateSerializerMock();
            _formHelperMock = new Mock<IFormHelper>();
            _handler = new WebFormsHandler(_serializerMock.Object, _formHelperMock.Object);
        }

        [Fact]
        public async Task HandleAsync_WithValidForm_ReturnsFormHtml()
        {
            // Arrange
            var item = CreateTestItem(
                content: "{\"Value\":{\"ContentType\":{\"Id\":\"form123\"}}}",
                type: typeof(WebForms));

            var expectedForm = new WebForms
            {
                Value = new ValueObject
                {
                    ContentType = new ContentTypeObject { Id = "form123" }
                }
            };

            _serializerMock.Setup(x => x.DeserializeAsync<WebForms>(item))
                         .ReturnsAsync(expectedForm);

            _formHelperMock.Setup(x => x.TagBuilder("lgwebsite", "form123"))
                         .Returns(new HtmlString("<form id='form123'></form>"));

            // Act
            var result = await _handler.HandleAsync(item);
            var html = GetHtmlString(result);

            // Assert
            Assert.Equal("<form id='form123'></form>", html);
        }

        [Fact]
        public async Task HandleAsync_WithNullContentType_ReturnsEmpty()
        {
            // Arrange
            var item = CreateTestItem("{}", typeof(WebForms));
            _serializerMock.Setup(x => x.DeserializeAsync<WebForms>(item))
                         .ReturnsAsync(new WebForms { Value = null });

            // Act
            var result = await _handler.HandleAsync(item);
            var html = GetHtmlString(result);

            // Assert
            Assert.Equal("", html);
        }
    }
}
