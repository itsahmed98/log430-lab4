using MagasinCentral.Data;
using MagasinCentral.Models;
using MagasinCentral.Services;
using Microsoft.EntityFrameworkCore;

namespace MagasinCentral.Tests.UnitTests.Services
{
    /// <summary>
    /// Tests unitaires pour le service PerformancesService.
    /// </summary>
    public class PerformancesServiceTest
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
        public void Constructor_ShouldThrowArgumentNullException_WhenContextIsNull()
        {
            // Arrange
            MagasinDbContext? context = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new PerformancesService(context!));
        }
    }
}
