using MagasinCentral.Controllers;
using Moq;
using MagasinCentral.Services;
using MagasinCentral.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace MagasinCentral.Tests.UnitTests.Controllers
{

    /// <summary>
    /// Tests unitaires pour le contrôleur TraiterDemandesController.
    /// </summary>
    public class TraiterDemandesControllerTest
    {
        private readonly Mock<IReapprovisionnementService> _serviceMock;
        private readonly TraiterDemandesController _controller;

        public TraiterDemandesControllerTest()
        {
            _serviceMock = new Mock<IReapprovisionnementService>();
            _controller = new TraiterDemandesController(_serviceMock.Object);

            var httpContext = new DefaultHttpContext();
            var tempDataProviderMock = new Mock<ITempDataProvider>();
            _controller.TempData = new TempDataDictionary(httpContext, tempDataProviderMock.Object);
        }

        [Fact]
        public void Constructeur_NullService_ThrowsArgumentNullException()
        {
            // Arrange
            IReapprovisionnementService reapprovisionnementService = null!;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new TraiterDemandesController(reapprovisionnementService));
        }

        [Fact]
        public async Task Index_ReturnsViewWithDemandesEnAttente()
        {
            // Arrange
            var demandes = new List<DemandeReapprovisionnement>
            {
                new DemandeReapprovisionnement { DemandeId = 1, ProduitId = 101, QuantiteDemandee = 50, Statut = "En attente" }
            };

            _serviceMock.Setup(s => s.GetDemandesEnAttenteAsync()).ReturnsAsync(demandes);

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(demandes, viewResult.Model);
        }

        [Fact]
        public async Task Traiter_ValidDemandeId_Approuve()
        {
            // Arrange
            int demandeId = 1;
            bool approuver = true;

            _serviceMock.Setup(s => s.TraiterDemandeAsync(demandeId, approuver))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Traiter(demandeId, approuver);

            // Assert
            _serviceMock.Verify(s => s.TraiterDemandeAsync(demandeId, approuver), Times.Once);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("La demande a été approuvée avec succès.", _controller.TempData["Succès"]);
        }
    }
}