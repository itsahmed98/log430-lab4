using System;
using System.Threading.Tasks;
using MagasinCentral.Services;
using MagasinCentral.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MagasinCentral.Controllers
{
    /// <summary>
    ///     Controller pour consulter le stock et demander un réapprovisionnement.
    /// </summary>
    public class ReapprovisionnementController : Controller
    {
        private readonly IReapprovisionnementService _reapprisonnementService;

        public ReapprovisionnementController(IReapprovisionnementService reapprisonnementService)
        {
            _reapprisonnementService = reapprisonnementService ?? throw new ArgumentNullException(nameof(reapprisonnementService));
        }

        /// <summary>
        ///     Affiche la liste des produits avec stock local et central pour un magasin donné.
        /// </summary>
        public async Task<IActionResult> Index(int magasinId)
        {
            var listeStocks = await _reapprisonnementService.GetStocksAsync(magasinId);

            ViewData["MagasinId"] = magasinId;
            return View(listeStocks);
        }

        /// <summary>
        /// Crée une demande de réapprovisionnement pour un produit donné.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DemanderReapprovisionnement(int magasinId, int produitId, int quantite)
        {
            try
            {
                await _reapprisonnementService.CreerDemandeReapprovisionnementAsync(magasinId, produitId, quantite);

                TempData["Succès"] = $"Demande de réapprovisionnement créée pour le produit: {produitId} pour une quantité de : {quantite} unités.";
            }
            catch (Exception ex)
            {
                TempData["Erreur"] = $"Impossible de créer la demande : {ex.Message}";
            }

            // Redirection vers Index pour afficher à nouveau les stocks
            return RedirectToAction(nameof(Index), new { magasinId });
        }

        public async Task<IActionResult> DemandesReapprovisionnement()
        {
            var demandes = await _reapprisonnementService.GetDemandesReapprovisionnementAsync();
            return View(demandes);
        }
    }
}