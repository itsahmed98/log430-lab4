using MagasinCentral.Models;
using MagasinCentral.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MagasinCentral.Api.Controllers
{
    [ApiController]
    [Route("api/v1/rapport")]
    [Authorize]
    public class RapportController : ControllerBase
    {
        private readonly ILogger<RapportController> _logger;
        private readonly IRapportService _rapportService;

        public RapportController(ILogger<RapportController> logger, IRapportService rapportService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _rapportService = rapportService ?? throw new ArgumentNullException(nameof(rapportService));
        }

        /// <summary>
        /// Récupère le rapport consolidé des ventes.
        /// </summary>
        /// <returns>Un rapport consolidé des ventes des magasins</returns>
        [HttpGet]
        [ProducesResponseType(typeof(RapportDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RapportDto>> Get()
        {
            _logger.LogInformation("Début de la génération du rapport consolidé des ventes.");

            try
            {
                var rapport = await _rapportService.ObtenirRapportConsolideAsync();

                if (rapport == null)
                {
                    _logger.LogWarning("Aucun rapport généré : le service a retourné null.");
                    return NotFound("Aucun rapport disponible.");
                }

                _logger.LogInformation("Rapport consolidé généré avec succès.");
                return Ok(rapport);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la génération du rapport consolidé.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Une erreur est survenue lors de la génération du rapport.");
            }
        }
    }
}
