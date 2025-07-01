using MagasinCentral.Data;
using MagasinCentral.Models;
using MagasinCentral.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace MagasinCentral.Tests.UnitTests.Services
{ }

/// <summary>
/// Tests unitaires pour le service ProduitService.
/// </summary>
public class ProduitServiceTest
{
    IMemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions());
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
    public void Constructor_ShouldThrowArgumentNullException_WhenContextIsNull()
    {
        // Arrange
        MagasinDbContext? context = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ProduitService(context!, _memoryCache));
    }

    [Fact]
    public async Task GetAllProduitsAsync_ShouldReturnAllProducts()
    {
        // Arrange
        var context = await CreateInMemoryContextAsync();
        var service = new ProduitService(context, _memoryCache);

        // Act
        var produits = await service.GetAllProduitsAsync();

        // Assert
        Assert.NotNull(produits);
    }

    [Fact]
    public async Task GetProduitByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        // Arrange
        var context = await CreateInMemoryContextAsync();
        var service = new ProduitService(context, _memoryCache);

        // Act
        var result = await service.GetProduitByIdAsync(999); // ID that does not exist

        // Assert
        Assert.Null(result);
    }
}
