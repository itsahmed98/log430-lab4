using MagasinCentral.Api.Controllers;
using MagasinCentral.Models;
using MagasinCentral.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace MagasinCentral.Tests.UnitTests.Api.Controllers
{
    public class RapportControllerTest
    {
        private readonly Mock<IRapportService> _svc;
        private readonly RapportController _ctrl;

        public RapportControllerTest()
        {
            _svc = new Mock<IRapportService>();
            var logger = new LoggerFactory().CreateLogger<RapportController>();
            _ctrl = new RapportController(logger, _svc.Object);
        }

        [Fact]
        public async Task Get_WhenData_ShouldReturnOk()
        {
            var data = new List<RapportDto> { new RapportDto { NomMagasin = "A" } };
            _svc.Setup(s => s.ObtenirRapportConsolideAsync()).ReturnsAsync(data);

            var result = await _ctrl.Get();

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(data, ok.Value);
        }

        [Fact]
        public async Task Get_WhenNull_ShouldReturnNotFound()
        {
            _svc.Setup(s => s.ObtenirRapportConsolideAsync()).ReturnsAsync((List<RapportDto>?)null);

            var result = await _ctrl.Get();

            var nf = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Aucun rapport disponible.", nf.Value);
        }

        [Fact]
        public async Task Get_WhenException_ShouldReturn500()
        {
            _svc.Setup(s => s.ObtenirRapportConsolideAsync()).ThrowsAsync(new Exception("fail"));

            var result = await _ctrl.Get();

            var err = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, err.StatusCode);
        }
    }
}
