using MagasinCentral.Controllers;
using MagasinCentral.Data;
using MagasinCentral.Services;
using Microsoft.Extensions.Caching.Memory;


namespace MagasinCentral.Tests.UnitTests.Controllers
{
    public class ProduitServiceTest
    {
        IMemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions());

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenContextIsNull()
        {
            // Arrange
            MagasinDbContext? context = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ProduitService(context!, _memoryCache));
        }

    }
}