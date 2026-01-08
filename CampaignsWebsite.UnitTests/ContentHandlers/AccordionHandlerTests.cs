using Blackpool.Zengenti.CMS.Models.Accordions;
using Blackpool.Zengenti.CMS.Models.Components;
using Microsoft.AspNetCore.Html;
using Moq;
using RazorPageCampaignsWebsite.Core.Services.ContentHandling.Handlers;
using RazorPageCampaignsWebsite.Helpers.Interfaces;
using RazorPageCampaignsWebsite.Helpers.Wrappers;
using CampaignsWebsite.UnitTests.ContentHandlers.Base;
using Xunit;

namespace CampaignsWebsite.UnitTests.ContentHandlers
{
    public class AccordionHandlerTests : HandlerTestBase
    {
        private readonly Mock<ISerializationHelper> _serializerMock;
        private readonly Mock<IAccordionRenderer> _rendererMock;
        private readonly AccordionHandler _handler;

        public AccordionHandlerTests()
        {
            _serializerMock = CreateSerializerMock();
            _rendererMock = new Mock<IAccordionRenderer>();
            _handler = new AccordionHandler(_serializerMock.Object, _rendererMock.Object);
        }

        [Fact]
        public async Task HandleAsync_WithValidAccordion_RendersCorrectly()
        {
            // Arrange
            var item = CreateTestItem(
                content: "{\"AccordionName\":\"faq\",\"Items\":[]}",
                type: typeof(Accordion));

            var accordion = new Accordion
            {
                AccordionName = "faq",
                AccordionContent = new List<AccordionContent>()
            };

            _serializerMock.Setup(x => x.DeserializeAsync<Accordion>(item))
                         .ReturnsAsync(accordion);

            _rendererMock.Setup(x => x.Render("faq", It.IsAny<List<AccordionContent>>()))
                       .Returns(new HtmlString("<div class='accordion'></div>"));

            // Act
            var result = await _handler.HandleAsync(item);
            var html  = GetHtmlString(result);

            // Assert
            Assert.Equal("<div class='accordion'></div>", html);
        }

        [Fact]
        public async Task HandleAsync_WithNullAccordion_ReturnsEmpty()
        {
            // Arrange
            var item = CreateTestItem("{}", typeof(Accordion));
            _serializerMock.Setup(x => x.DeserializeAsync<Accordion>(item))
                         .ReturnsAsync((Accordion)null);

            // Act
            var result = await _handler.HandleAsync(item);
            var html = GetHtmlString(result);

            // Assert
            Assert.Equal("", html);
        }
    }
}
