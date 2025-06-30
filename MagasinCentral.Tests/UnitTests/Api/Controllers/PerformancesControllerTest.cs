using MagasinCentral.Api.Controllers;
using MagasinCentral.Services;
using MagasinCentral.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace MagasinCentral.Tests.UnitTests.Api.Controllers
{
    public class PerformancesControllerTest
    {
        private readonly Mock<IPerformancesService> _svc;
        private readonly PerformancesController _ctrl;

        public PerformancesControllerTest()
        {
            _svc = new Mock<IPerformancesService>();
            var logger = new LoggerFactory().CreateLogger<PerformancesController>();
            _ctrl = new PerformancesController(logger, _svc.Object);
        }

        [Fact]
        public async Task GetPerformances_WithData_ShouldReturnOkObjectResult()
        {
            // Arrange
            var fakeVm = new PerformancesViewModel();
            _svc.Setup(s => s.GetPerformances()).ReturnsAsync(fakeVm);

            // Act
            var result = await _ctrl.GetPerformances();

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(fakeVm, ok.Value);
        }

        [Fact]
        public async Task GetPerformances_WhenNullReturned_ShouldReturnOkWithNull()
        {
            _svc.Setup(s => s.GetPerformances()).ReturnsAsync((PerformancesViewModel?)null);

            var result = await _ctrl.GetPerformances();

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Null(ok.Value);
        }

        [Fact]
        public async Task GetPerformances_WhenException_ShouldReturn500()
        {
            _svc.Setup(s => s.GetPerformances()).ThrowsAsync(new Exception("boom"));

            var result = await _ctrl.GetPerformances();

            var err = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, err.StatusCode);
        }
    }
}
