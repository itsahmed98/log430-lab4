using MagasinCentral.Api.Controllers;
using MagasinCentral.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace MagasinCentral.Tests.UnitTests.Api.Controllers
{
    public class StockControllerTest
    {
        private readonly Mock<IStockService> _svc;
        private readonly StockController _ctrl;

        public StockControllerTest()
        {
            _svc = new Mock<IStockService>();
            var logger = new LoggerFactory().CreateLogger<StockController>();
            _ctrl = new StockController(logger, _svc.Object);
        }

        [Fact]
        public async Task GetStockMagasin_Existing_ShouldReturnQuantity()
        {
            _svc.Setup(s => s.GetStockByMagasinId(5)).ReturnsAsync(123);

            var result = await _ctrl.GetStockMagasin(5);

            var ok = Assert.IsType<ActionResult<int>>(result);
            Assert.Equal(123, ok.Value);
        }

        [Fact]
        public async Task GetStockMagasin_Unknown_ShouldReturnNotFound()
        {
            _svc.Setup(s => s.GetStockByMagasinId(99)).ReturnsAsync((int?)null);

            var result = await _ctrl.GetStockMagasin(99);

            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}
