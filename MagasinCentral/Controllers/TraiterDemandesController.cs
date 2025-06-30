using MagasinCentral.Models;
using MagasinCentral.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MagasinCentral.Controllers
{
    /// <summary>
    ///     Contrôleur pour UC6 : approbation/refus des demandes de réapprovisionnement.
    /// </summary>
    public class TraiterDemandesController : Controller
    {
        private readonly IReapprovisionnementService _reapprovisionnementService;

        public TraiterDemandesController(IReapprovisionnementService reapprovisionnementService)
        {
            _reapprovisionnementService = reapprovisionnementService
                ?? throw new ArgumentNullException(nameof(reapprovisionnementService));
        }

        /// <summary>
        /// Affiche la liste des demandes en attente.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var demandes = await _reapprovisionnementService.GetDemandesEnAttenteAsync();
            return View(demandes);
        }

        /// <summary>
        /// Traiter une demande en attente.
        /// </summary>
        /// <param name="demandeId">ID de la demande à traiter</param>
        /// <param name="approuver">true = approuver, false = refuser</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Traiter(int demandeId, bool approuver)
        {
            Console.WriteLine($"Traitement de la demande {demandeId} : {(approuver ? "Approuver" : "Refuser")}");
            try
            {
                await _reapprovisionnementService.TraiterDemandeAsync(demandeId, approuver);
                TempData["Succès"] = approuver
                    ? "La demande a été approuvée avec succès."
                    : "La demande a été refusée.";
            }
            catch (Exception ex)
            {
                TempData["Erreur"] = $"Impossible de traiter la demande : {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
