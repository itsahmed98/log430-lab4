using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using MagasinCentral.IntegrationTests;
using MagasinCentral.Models;
using MagasinCentral.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace MagasinCentral.Tests.IntegrationTests.Api.Controllers
{
    public class ProduitControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public ProduitControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        // Helper pour builder le client avec FakeJwtAuthHandler et override de IProduitService
        private HttpClient CreateAuthenticatedClient(Mock<IProduitService> produitServiceMock)
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

                    // 2) Remplacement du service
                    services.RemoveAll<IProduitService>();
                    services.AddSingleton(produitServiceMock.Object);
                });
            }).CreateClient();
        }

        [Fact]
        public async Task GetProduits_Returns200Ok_And_ArrayJson()
        {
            // Arrange
            var listesMock = new List<Produit>
            {
                new Produit { ProduitId = 1, Nom = "P1", Prix = 10m },
                new Produit { ProduitId = 2, Nom = "P2", Prix = 20m }
            };
            var mockService = new Mock<IProduitService>();
            mockService.Setup(s => s.GetAllProduitsAsync())
                       .ReturnsAsync(listesMock);

            var client = CreateAuthenticatedClient(mockService);

            // Act
            var response = await client.GetAsync("/api/v1/produits");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            Assert.Equal(JsonValueKind.Array, doc.RootElement.ValueKind);
            Assert.Equal(2, doc.RootElement.GetArrayLength());
        }

        [Fact]
        public async Task GetProduits_WithoutAuth_Returns401Unauthorized()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/v1/produits");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetProduits_ServiceThrowsException_Returns500()
        {
            var mockService = new Mock<IProduitService>();
            mockService.Setup(s => s.GetAllProduitsAsync())
                       .ThrowsAsync(new Exception("fail"));

            var client = CreateAuthenticatedClient(mockService);
            var response = await client.GetAsync("/api/v1/produits");
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async Task GetProduit_NotFound_Returns404()
        {
            var mockService = new Mock<IProduitService>();
            mockService.Setup(s => s.GetProduitByIdAsync(123))
                       .ReturnsAsync((Produit?)null);

            var client = CreateAuthenticatedClient(mockService);
            var response = await client.GetAsync("/api/v1/produits/123");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetProduit_WithoutAuth_Returns401Unauthorized()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/v1/produits/1");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetProduit_ServiceThrowsException_Returns500()
        {
            var mockService = new Mock<IProduitService>();
            mockService.Setup(s => s.GetProduitByIdAsync(It.IsAny<int>()))
                       .ThrowsAsync(new Exception("fail"));

            var client = CreateAuthenticatedClient(mockService);
            var response = await client.GetAsync("/api/v1/produits/1");
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async Task ModifierProduit_Returns200Ok()
        {
            // Arrange
            var dto = new ProduitDto { Nom = "Modifié", Prix = 99m };
            var existing = new Produit { ProduitId = 7, Nom = "Old", Prix = 10m };
            var mockService = new Mock<IProduitService>();
            mockService.Setup(s => s.GetProduitByIdAsync(7))
                       .ReturnsAsync(existing);
            mockService.Setup(s => s.ModifierProduitAsync(7, dto))
                       .Returns(Task.CompletedTask);

            var client = CreateAuthenticatedClient(mockService);

            // Act
            var response = await client.PutAsJsonAsync("/api/v1/produits/7", dto);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var returned = await response.Content.ReadFromJsonAsync<ProduitDto>();
            Assert.Equal(dto.Nom, returned!.Nom);
            Assert.Equal(dto.Prix, returned.Prix);
        }

        [Fact]
        public async Task ModifierProduit_NotFound_Returns404()
        {
            var dto = new ProduitDto { Nom = "X", Prix = 1m };
            var mockService = new Mock<IProduitService>();
            mockService.Setup(s => s.GetProduitByIdAsync(99))
                       .ReturnsAsync((Produit?)null);

            var client = CreateAuthenticatedClient(mockService);
            var response = await client.PutAsJsonAsync("/api/v1/produits/99", dto);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task ModifierProduit_WithoutAuth_Returns401Unauthorized()
        {
            var client = _factory.CreateClient();
            var dto = new ProduitDto { Nom = "X", Prix = 1m };
            var response = await client.PutAsJsonAsync("/api/v1/produits/1", dto);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
