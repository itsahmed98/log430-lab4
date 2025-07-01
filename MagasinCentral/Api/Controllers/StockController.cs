using Microsoft.AspNetCore.Mvc;
using MagasinCentral.Services;
using Microsoft.AspNetCore.Authorization;

namespace MagasinCentral.Api.Controllers
{
    /// <summary>
    /// Contrôleur pour le stock des magasins.
    /// </summary>
    [ApiController]
    [Route("api/v1/stocks")]
    //[Authorize]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;
        private readonly ILogger<StockController> _logger;

        /// <summary>
        /// Constructeur du contrôleur de rapport.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="stockService"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public StockController(ILogger<StockController> logger, IStockService stockService)
        {
            {
                _stockService = stockService ?? throw new ArgumentNullException(nameof(stockService));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }
        }

        /// <summary>
        /// Retourner la quantité de stock pour un magasin spécifique (avec le ID).
        /// </summary>
        /// <param name="magasinId">L'identifiant du magasin dans lequel on veut récupérer la quantité du stock</param>
        /// <returns></returns>
        [HttpGet(Name = "GetStockMagasin")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> GetStockMagasin(int magasinId)
        {
            _logger.LogInformation("Récupération du stock pour le magasin avec ID {MagasinId}", magasinId);
            try
            {
                int? quantite = await _stockService.GetStockByMagasinId(magasinId);

                if (quantite == null)
                    return NotFound();

                return quantite.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération du stock du magasin: {MagasinId}", magasinId);
                return StatusCode(500, "Une erreur s'est produite lors de la récupération du stock.");
            }
        }
    }
}