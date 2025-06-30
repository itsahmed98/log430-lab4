using System.Collections.Generic;
using System.Threading.Tasks;
using MagasinCentral.Controllers;
using MagasinCentral.Models;
using MagasinCentral.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using MagasinCentral.ViewModels;


namespace MagasinCentral.Tests.UnitTests.Controllers
{
    public class PerformanceControllerTest
    {
        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenServiceIsNull()
        {
            // Arrange
            IPerformancesService? service = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new PerformancesController(service!));
        }

        [Fact]
        public async Task Index_ShouldReturnViewWithPerformanceData()
        {
            // Arrange
            var serviceMock = new Mock<IPerformancesService>();
            var controller = new PerformancesController(serviceMock.Object);

            var performanceData = new PerformancesViewModel();
            serviceMock.Setup(s => s.GetPerformances()).ReturnsAsync(performanceData);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(performanceData, viewResult.Model);
        }
    }
}