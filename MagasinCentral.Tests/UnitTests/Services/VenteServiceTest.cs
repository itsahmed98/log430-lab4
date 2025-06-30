using MagasinCentral.Data;
using MagasinCentral.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace MagasinCentral.Tests.UnitTests.Services
{
    public class VenteServiceTest
    {
        private async Task<MagasinDbContext> CreateInMemoryContextAsync()
        {
            var options = new DbContextOptionsBuilder<MagasinDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
                .Options;

            var context = new MagasinDbContext(options);
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
            return context;
        }

        [Fact]
        public void Constructeur_NullDbContext_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new VenteService(null!));
        }

        [Fact]
        public async Task CreerVenteAsync_ValidData_CreatesVente()
        {

            // Arrange
            var magasinId = 1;
            var lignes = new List<(int produitId, int quantite)>
            {
                (3, 2),
                (2, 3)
            };

            var serviceMock = new Mock<IVenteService>();
            serviceMock
                .Setup(s => s.CreerVenteAsync(magasinId, lignes))
                .ReturnsAsync(1);

            var context = await CreateInMemoryContextAsync();
            var venteService = new VenteService(context);

            // Act
            var venteId = await venteService.CreerVenteAsync(magasinId, lignes);

            // Assert
            Assert.True(venteId > 0);
        }
    }
}