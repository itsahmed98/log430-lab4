using MagasinCentral.Services;
using MagasinCentral.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MagasinCentral.Controllers
{
    /// <summary>
    /// Contrôleur pour les performances du tableau de bord.
    /// </summary>
    public class PerformancesController : Controller
    {
        private readonly IPerformancesService _performancesService;

        public PerformancesController(IPerformancesService performancesService)
        {
            _performancesService = performancesService ?? throw new ArgumentNullException(nameof(performancesService));
        }

        /// <summary>
        /// Affiche le tableau de bord des performances.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var model = await _performancesService.GetPerformances();
            return View(model);
        }
    }
}