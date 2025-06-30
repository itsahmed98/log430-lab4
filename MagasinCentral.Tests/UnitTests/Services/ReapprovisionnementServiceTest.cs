using MagasinCentral.Data;
using MagasinCentral.Models;
using MagasinCentral.Services;
using Microsoft.EntityFrameworkCore;

namespace MagasinCentral.Tests.UnitTests.Services
{
    /// <summary>
    /// Tests unitaires pour reapprovisionnement service.
    /// </summary>
    public class ReapprovisionnementServiceTest
    {
        /// <summary>
        /// Test du constructeur du service de réapprovisionnement avec un contexte null.
        /// </summary>
        [Fact]
        public void Constructeur_NullContexte_ThrowsArgumentNullException()
        {
            // Arrange
            MagasinDbContext contexte = null!;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ReapprovisionnementService(contexte));
        }

        /// <summary>
        /// Test de la méthode GetStocksAsync pour vérifier qu'elle retourne les stocks correctement.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetStocksAsync_ReturnsStocksCorrectement()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MagasinDbContext>()
                .UseInMemoryDatabase(databaseName: "GetStocksAsyncTest")
                .Options;

            using var contexte = new MagasinDbContext(options);
            contexte.Magasins.Add(new Magasin { MagasinId = 1, Nom = "Magasin 1" });
            contexte.Produits.Add(new Produit { ProduitId = 1, Nom = "Produit A" });
            contexte.MagasinStocksProduits.Add(new MagasinStockProduit { MagasinId = 1, ProduitId = 1, Quantite = 10 });
            contexte.StocksCentraux.Add(new StockCentral { ProduitId = 1, Quantite = 50 });
            await contexte.SaveChangesAsync();

            var service = new ReapprovisionnementService(contexte);

            // Act
            var stocks = await service.GetStocksAsync(1);

            // Assert
            Assert.Single(stocks);
            Assert.Equal("Produit A", stocks[0].NomProduit);
            Assert.Equal(10, stocks[0].QuantiteLocale);
            Assert.Equal(50, stocks[0].QuantiteCentral);
        }

        /// <summary>
        /// Test de la méthode CreerDemandeReapprovisionnementAsync pour vérifier qu'elle crée une demande correctement.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreerDemandeReapprovisionnementAsync_ShouldCreatesDemandeCorrectement()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MagasinDbContext>()
                .UseInMemoryDatabase(databaseName: "CreerDemandeReapprovisionnementAsyncTest")
                .Options;

            using var contexte = new MagasinDbContext(options);
            contexte.Magasins.Add(new Magasin { MagasinId = 1, Nom = "Magasin 1" });
            contexte.Produits.Add(new Produit { ProduitId = 1, Nom = "Produit A" });
            await contexte.SaveChangesAsync();

            var service = new ReapprovisionnementService(contexte);

            // Act
            await service.CreerDemandeReapprovisionnementAsync(1, 1, 20);

            // Assert
            var demande = await contexte.DemandesReapprovisionnement
                .FirstOrDefaultAsync(d => d.MagasinId == 1 && d.ProduitId == 1 && d.QuantiteDemandee == 20);
            Assert.NotNull(demande);
        }

        /// <summary>
        /// Test de la méthode GetDemandesReapprovisionnementAsync pour vérifier qu'elle retourne toutes les demandes.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetDemandesReapprovisionnementAsync_ShouldReturnAllDemandes()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MagasinDbContext>()
                .UseInMemoryDatabase(databaseName: "GetDemandesReapprovisionnementAsyncTest")
                .Options;

            using var contexte = new MagasinDbContext(options);
            contexte.Magasins.Add(new Magasin { MagasinId = 1, Nom = "Magasin 1" });
            contexte.Produits.Add(new Produit { ProduitId = 1, Nom = "Produit A" });
            contexte.DemandesReapprovisionnement.Add(new DemandeReapprovisionnement
            {
                MagasinId = 1,
                ProduitId = 1,
                QuantiteDemandee = 20,
                DateDemande = DateTime.UtcNow,
                Statut = "EnAttente"
            });
            await contexte.SaveChangesAsync();

            var service = new ReapprovisionnementService(contexte);

            // Act
            var demandes = await service.GetDemandesReapprovisionnementAsync();

            // Assert
            Assert.Single(demandes);
            Assert.Equal("EnAttente", demandes[0].Statut);
        }
    }
}
