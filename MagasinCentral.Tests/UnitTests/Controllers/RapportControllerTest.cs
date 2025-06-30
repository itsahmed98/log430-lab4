using System.Collections.Generic;
using System.Threading.Tasks;
using MagasinCentral.Controllers;
using MagasinCentral.Models;
using MagasinCentral.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace MagasinCentral.Tests.UnitTests.Controllers
{
    /// <summary>
    ///     Tests unitaires pour le RapportController.
    /// </summary>
    public class RapportControllerTest
    {
        [Fact]
        public void Constructeur_NullService_ThrowsArgumentNullException()
        {
            // Arrange
            IRapportService rapportService = null!;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new RapportController(rapportService));
        }

        [Fact]
        public async Task Index_ReturnsViewWithRapportConsolide()
        {
            // Arrange
            var rapportServiceMock = new Mock<IRapportService>();
            var rapportController = new RapportController(rapportServiceMock.Object);

            var rapportConsolide = new List<RapportDto>
            {
                new RapportDto
                {
                    NomMagasin = "TestMagasin",
                    ChiffreAffairesTotal = 123.45m,
                    TopProduits = new List<InfosVenteProduit>
                    {
                        new InfosVenteProduit { NomProduit = "X", QuantiteVendue = 5, TotalVentes = 25m }
                    },
                    StocksRestants = new List<InfosStockProduit>
                    {
                        new InfosStockProduit { NomProduit = "X", QuantiteRestante = 45 }
                    }
                }
            };

            rapportServiceMock
                .Setup(s => s.ObtenirRapportConsolideAsync())
                .ReturnsAsync(rapportConsolide);

            // Act
            var result = rapportController.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(rapportConsolide, viewResult.Model);
        }
    }
}
