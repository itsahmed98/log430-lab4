using MagasinCentral.Api.Controllers;
using MagasinCentral.Models;
using MagasinCentral.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace MagasinCentral.Tests.UnitTests.Api.Controllers
{
    public class ProduitControllerTest
    {
        private readonly Mock<IProduitService> _svc;
        private readonly ProduitController _ctrl;

        public ProduitControllerTest()
        {
            _svc = new Mock<IProduitService>();
            var logger = new LoggerFactory().CreateLogger<ProduitController>();
            _ctrl = new ProduitController(logger, _svc.Object);
        }

        [Fact]
        public async Task GetProduits_ShouldReturnOkList()
        {
            var list = new List<Produit> { new Produit { ProduitId = 1, Nom = "X" } };
            _svc.Setup(s => s.GetAllProduitsAsync()).ReturnsAsync(list);

            var result = await _ctrl.GetProduits();

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(list, ok.Value);
        }

        [Fact]
        public async Task GetProduit_ExistingId_ShouldReturnOk()
        {
            var dto = new Produit { ProduitId = 42, Nom = "Stylo" };
            _svc.Setup(s => s.GetProduitByIdAsync(42)).ReturnsAsync(dto);

            var result = await _ctrl.GetProduit(42);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dto, ok.Value);
        }

        [Fact]
        public async Task GetProduit_UnknownId_ShouldReturnNotFound()
        {
            _svc.Setup(s => s.GetProduitByIdAsync(99)).ReturnsAsync((Produit?)null);

            var result = await _ctrl.GetProduit(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task ModifierProduit_ValidDto_ShouldReturnOkWithDto()
        {
            var dto = new ProduitDto { Nom = "Nouveau", Prix = 5m };
            _svc.Setup(s => s.GetProduitByIdAsync(1))
                .ReturnsAsync(new Produit { ProduitId = 1 });
            _svc.Setup(s => s.ModifierProduitAsync(1, dto))
                .Returns(Task.CompletedTask);

            var result = await _ctrl.ModifierProduit(1, dto);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dto, ok.Value);
        }
    }
}
