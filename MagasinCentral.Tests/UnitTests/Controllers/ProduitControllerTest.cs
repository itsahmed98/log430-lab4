using MagasinCentral.Controllers;
using MagasinCentral.Data;
using MagasinCentral.Services;


namespace MagasinCentral.Tests.UnitTests.Controllers
{
    public class ProduitServiceTest
    {

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenContextIsNull()
        {
            // Arrange
            MagasinDbContext? context = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ProduitService(context!));
        }

    }
}