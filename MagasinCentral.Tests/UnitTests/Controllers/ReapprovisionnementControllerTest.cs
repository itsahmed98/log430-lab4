using MagasinCentral.Services;
using MagasinCentral.Controllers;
using MagasinCentral.ViewModels;
using Moq;
using Microsoft.AspNetCore.Mvc;

namespace MagasinCentral.Tests.UnitTests.Controllers
{
    /// <summary>
    /// Tests unitaires pour le contrôleur de réapprovisionnement.
    /// </summary>
    public class ReapprovisionnementControllerTest
    {
        /// <summary>
        /// Test du constructeur du contrôleur de réapprovisionnement avec un service null.
        /// </summary>
        [Fact]
        public void Constructeur_NullService_ShouldThrowArgumentNullException()
        {
            // Arrange
            IReapprovisionnementService reapprovisionnementService = null!;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ReapprovisionnementController(reapprovisionnementService));
        }

        /// <summary>
        /// Test de la méthode Index pour vérifier qu'elle retourne la vue avec les stocks.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Index_ReturnsViewWithStocks()
        {
            // Arrange
            var reapprovisionnementServiceMock = new Mock<IReapprovisionnementService>();
            var controller = new ReapprovisionnementController(reapprovisionnementServiceMock.Object);

            int magasinId = 1;
            var stocks = new List<StockVue>
            {
                new StockVue { ProduitId = 1, NomProduit = "Produit A", QuantiteLocale = 10, QuantiteCentral = 50 },
                new StockVue { ProduitId = 2, NomProduit = "Produit B", QuantiteLocale = 5, QuantiteCentral = 30 }
            };

            reapprovisionnementServiceMock
                .Setup(s => s.GetStocksAsync(magasinId))
                .ReturnsAsync(stocks);

            // Act
            var result = await controller.Index(magasinId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(stocks, viewResult.Model);
            Assert.Equal(magasinId, viewResult.ViewData["MagasinId"]);
        }
    }
}