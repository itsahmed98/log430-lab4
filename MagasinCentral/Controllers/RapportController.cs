using System.Diagnostics;
using MagasinCentral.Models;
using Microsoft.AspNetCore.Mvc;
using MagasinCentral.Data;
using MagasinCentral.Services;

namespace MagasinCentral.Controllers
{
    public class RapportController : Controller
    {
        private readonly IRapportService _rapportService;

        /// <summary>
        /// Constructeur de <see cref="RapportController"/>.
        /// </summary>
        /// <param name="rapportService"></param>
        public RapportController(IRapportService rapportService)

        {
            _rapportService = rapportService ?? throw new ArgumentNullException(nameof(rapportService));
        }

        /// <summary>
        /// Affiche la page d'accueil du rapport consolidé.
        /// </summary>
        public IActionResult Index()
        {
            IActionResult result = null!;
            try
            {
                var rapportConsolide = _rapportService.ObtenirRapportConsolideAsync().Result;
                result = View(rapportConsolide);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                ViewBag.ErrorMessage = $"Une erreur est survenue lors de la génération du rapport: {ex.Message}";
                result = View("Error");
            }

            return result;
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
