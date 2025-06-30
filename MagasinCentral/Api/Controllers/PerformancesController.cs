using MagasinCentral.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MagasinCentral.Api.Controllers
{
    /// <summary>
    /// Un contrôleur pour les performances du tableau de bord.
    /// </summary>
    [ApiController]
    [Route("api/v1/performances")]
    [Authorize]
    public class PerformancesController : ControllerBase
    {
        private readonly ILogger<PerformancesController> _logger;
        private readonly IPerformancesService _performancesService;

        public PerformancesController(ILogger<PerformancesController> logger, IPerformancesService performanceService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _performancesService = performanceService ?? throw new ArgumentNullException(nameof(performanceService));
        }

        /// <summary>
        /// Récupère les performances des magasins.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> GetPerformances()
        {
            _logger.LogInformation("Début de la récupération des performances du magasin.");
            try
            {
                var performances = await _performancesService.GetPerformances();
                if (performances == null)
                {
                    _logger.LogWarning("Aucune performance trouvée : le service a retourné null.");
                }
                _logger.LogInformation("Performances récupérées avec succès.");
                return Ok(performances);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des performances du magasin.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Une erreur est survenue lors de la récupération des performances.");
            }
        }
    }
}
