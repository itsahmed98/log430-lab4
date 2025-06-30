using MagasinCentral.Controllers;
using MagasinCentral.Data;
using MagasinCentral.Models;
using MagasinCentral.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Microsoft.EntityFrameworkCore;

namespace MagasinCentral.Tests.UnitTests.Controllers
{


    /// <summary>
    /// Tests unitaires pour le contrôleur VenteController.
    /// </summary>
    public class VenteControllerTest
    {
        private readonly Mock<IVenteService> _venteServiceMock;
        private readonly Mock<IProduitService> _produitServiceMock;
        private readonly MagasinDbContext _contexte;

        public VenteControllerTest()
        {
            _venteServiceMock = new Mock<IVenteService>();
            _produitServiceMock = new Mock<IProduitService>();

            var options = new DbContextOptionsBuilder<MagasinDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_For_Constructor")
            .Options;
            _contexte = new MagasinDbContext(options);
        }

        [Fact]
        public void Constructeur_NullServices_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new VenteController(null!, _produitServiceMock.Object, _contexte));
            Assert.Throws<ArgumentNullException>(() => new VenteController(_venteServiceMock.Object, null!, _contexte));
            Assert.Throws<ArgumentNullException>(() => new VenteController(_venteServiceMock.Object, _produitServiceMock.Object, null!));
        }

    }
}