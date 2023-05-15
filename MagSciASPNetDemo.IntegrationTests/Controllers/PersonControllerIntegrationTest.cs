using FluentAssertions;
using HtmlAgilityPack;

namespace ContactsManagement.IntegrationTests.Controllers
{
    /// <summary>
    /// Here, you are the client sending requests to the program
    /// Foreach( IActionMethod method in Controller) { Create ValidTest(method); Create InvalidTest(method); }
    /// </summary>
    public class PersonControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient httpClient;
        public PersonControllerIntegrationTest(CustomWebApplicationFactory factory)
        {
            httpClient = factory.CreateClient();
        }

        #region Index
        [Fact]
        public async Task Index_ToReturnView()
        {
            //Arrange
            //Act
            HttpResponseMessage response = await httpClient.GetAsync("Persons/Index");
            //Assert
            response.Should().BeSuccessful();

            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(response.Content.ToString());
            var document = html.DocumentNode;
            // You can validate if persons table exist on document
        }
        #endregion
    }
}
