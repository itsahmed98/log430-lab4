using MagasinCentral.IntegrationTests;
using MagasinCentral.Services;
using MagasinCentral.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace MagasinCentral.Tests.IntegrationTests.Api.Controllers
{
    public class PerformancesControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public PerformancesControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public void Constructor_WithNullLogger_ThrowsException()
        {
            var mockService = new Mock<IPerformancesService>();
            Assert.Throws<ArgumentNullException>(() =>
                new MagasinCentral.Api.Controllers.PerformancesController(null!, mockService.Object));
        }

        [Fact]
        public void Constructor_WithNullService_ThrowsException()
        {
            var mockLogger = new Mock<ILogger<MagasinCentral.Api.Controllers.PerformancesController>>();
            Assert.Throws<ArgumentNullException>(() =>
                new MagasinCentral.Api.Controllers.PerformancesController(mockLogger.Object, null!));
        }

        [Fact]
        public async Task GetPerformances_Returns200Ok()
        {
            // Arrange
            var performancesMock = new PerformancesViewModel
            {
                RevenusParMagasin = new List<RevenuMagasin>
                {
                    new RevenuMagasin { MagasinId = 1, NomMagasin = "ÉTS", ChiffreAffaires = 1234.56m }
                },
                ProduitsRupture = new List<StockProduitLocal>
                {
                    new StockProduitLocal { MagasinId = 1, NomMagasin = "ÉTS", ProduitId = 10, NomProduit = "Clavier", QuantiteLocale = 0 }
                },
                ProduitsSurstock = new List<StockProduitLocal>
                {
                    new StockProduitLocal { MagasinId = 1, NomMagasin = "ÉTS", ProduitId = 20, NomProduit = "Souris", QuantiteLocale = 999 }
                },
                TendancesHebdomadairesParMagasin = new Dictionary<int, List<VentesQuotidiennes>>
                {
                    [1] = new List<VentesQuotidiennes>
                    {
                        new VentesQuotidiennes { Date = DateTime.UtcNow.Date, MontantVentes = 300m }
                    }
                }
            };

            var mockService = new Mock<IPerformancesService>();
            mockService.Setup(s => s.GetPerformances())
                       .ReturnsAsync(performancesMock);

            // On remplace le handler JWT par notre FakeJwtAuthHandler en schéma "Test"
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // 1) Configurer le schéma "Test" comme authentification par défaut
                    services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = "Test";
                        options.DefaultChallengeScheme = "Test";
                    })
                    .AddScheme<AuthenticationSchemeOptions, FakeJwtAuthHandler>(
                        "Test", _ => { });

                    // 2) Remplacer IPerformancesService par le mock
                    services.RemoveAll<IPerformancesService>();
                    services.AddSingleton(mockService.Object);
                });
            }).CreateClient();

            // Act
            var response = await client.GetAsync("/api/v1/performances");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            Assert.Contains("revenusParMagasin", json);
            Assert.Contains("produitsRupture", json);
            Assert.Contains("produitsSurstock", json);
            Assert.Contains("tendancesHebdomadairesParMagasin", json);
        }

        [Fact]
        public async Task GetPerformances_WithoutToken_Returns401Unauthorized()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/v1/performances");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetPerformances_ServiceThrowsException_Returns500()
        {
            var mockService = new Mock<IPerformancesService>();
            mockService.Setup(s => s.GetPerformances())
                       .ThrowsAsync(new Exception("Simulated service failure"));

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = "Test";
                        options.DefaultChallengeScheme = "Test";
                    })
                    .AddScheme<AuthenticationSchemeOptions, FakeJwtAuthHandler>(
                        "Test", _ => { });

                    services.RemoveAll<IPerformancesService>();
                    services.AddSingleton(mockService.Object);
                });
            }).CreateClient();

            var response = await client.GetAsync("/api/v1/performances");
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}
