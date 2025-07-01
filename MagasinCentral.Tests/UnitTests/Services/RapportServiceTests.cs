using MagasinCentral.Data;
using MagasinCentral.Models;
using MagasinCentral.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace MagasinCentral.Tests.UnitTests.Services
{
    /// <summary>
    /// Tests unitaires pour Rapport service.
    /// </summary>
    public class RapportServiceTests
    {
        private readonly IMemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions());
        /// <summary>
        /// Construit un DbContext InMemory fraîchement seedé.
        /// </summary>
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

        /// <summary>
        /// Vérifie que le service retourne une liste de rapports consolidés.
        /// </summary>
        [Fact]
        public async Task GetRapportConsolideAsync_ShouldReturnRapportConsolides()
        {
            // Arrange
            var context = await CreateInMemoryContextAsync();
            var service = new RapportService(context, _memoryCache);

            // Act
            var listeRapports = await service.ObtenirRapportConsolideAsync();

            // Assert
            Assert.Equal(5, listeRapports.Count);
            Assert.Contains(listeRapports, r => r.NomMagasin == "Stock Central");
        }

        /// <summary>
        /// Vérifie que le chiffre d'affaires total est calculé correctement pour un magasin.
        /// </summary>
        [Fact]
        public async Task GetRapportConsolideAsync_ShouldReturnCorrectAmount()
        {
            // Arrange
            var context = await CreateInMemoryContextAsync();
            var service = new RapportService(context, _memoryCache);

            // Act
            var listeRapports = await service.ObtenirRapportConsolideAsync();
            var rapportMagasin1 = listeRapports.First(r => r.NomMagasin == "Magasin Centre-Ville"); // Le premier magasin ajouté dans le seeder

            // Assert
            Assert.Equal(51.75m, rapportMagasin1.ChiffreAffairesTotal);
        }

        /// <summary>
        /// Vérifie que les stocks restants sont complets pour un magasin sans ventes.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetRapportConsolideAsync_StocksRestantsCompletParMagasin()
        {
            // Arrange
            var context = await CreateInMemoryContextAsync();
            var service = new RapportService(context, _memoryCache);

            // Act
            var listeRapports = await service.ObtenirRapportConsolideAsync();
            var rapportMag3 = listeRapports.First(r => r.NomMagasin == "Magasin Quartier Nord");

            // Assert
            Assert.Equal(4, rapportMag3.StocksRestants.Count);
            Assert.All(rapportMag3.StocksRestants, s => Assert.Equal(50, s.QuantiteRestante));
        }
    }
}
