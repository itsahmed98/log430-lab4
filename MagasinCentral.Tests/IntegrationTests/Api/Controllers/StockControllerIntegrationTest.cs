using System.Net;
using MagasinCentral.IntegrationTests;
using MagasinCentral.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace MagasinCentral.Tests.IntegrationTests.Api.Controllers
{
    public class StockControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public StockControllerIntegrationTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Helper pour créer un client authentifié via FakeJwtAuthHandler et override IStockService
        /// </summary>
        private HttpClient CreateAuthenticatedClient(Mock<IStockService> stockServiceMock)
        {
            return _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // 1) Authentification factice
                    services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = "Test";
                        options.DefaultChallengeScheme = "Test";
                    })
                    .AddScheme<AuthenticationSchemeOptions, FakeJwtAuthHandler>(
                        "Test", _ => { });

                    // 2) Override du service
                    services.RemoveAll<IStockService>();
                    services.AddSingleton(stockServiceMock.Object);
                });
            }).CreateClient();
        }

        [Fact]
        public async Task GetStockMagasin_Returns200Ok_WithQuantity()
        {
            // Arrange
            var magasinId = 42;
            var expectedQuantity = 123;
            var mockService = new Mock<IStockService>();
            mockService
                .Setup(s => s.GetStockByMagasinId(magasinId))
                .ReturnsAsync(expectedQuantity);

            var client = CreateAuthenticatedClient(mockService);

            // Act
            var response = await client.GetAsync($"/api/v1/stocks?magasinId={magasinId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal(expectedQuantity.ToString(), content);
        }

        [Fact]
        public async Task GetStockMagasin_WithoutAuth_Returns401Unauthorized()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/v1/stocks?magasinId=1");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetStockMagasin_NotFound_Returns404()
        {
            var magasinId = 99;
            var mockService = new Mock<IStockService>();
            mockService
                .Setup(s => s.GetStockByMagasinId(magasinId))
                .ReturnsAsync((int?)null);

            var client = CreateAuthenticatedClient(mockService);
            var response = await client.GetAsync($"/api/v1/stocks?magasinId={magasinId}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetStockMagasin_ServiceThrowsException_Returns500()
        {
            var magasinId = 7;
            var mockService = new Mock<IStockService>();
            mockService
                .Setup(s => s.GetStockByMagasinId(magasinId))
                .ThrowsAsync(new Exception("fail"));

            var client = CreateAuthenticatedClient(mockService);
            var response = await client.GetAsync($"/api/v1/stocks?magasinId={magasinId}");

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}
