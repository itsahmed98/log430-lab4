using MagasinCentral.Services;
using MagasinCentral.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MagasinCentral.Models;

namespace MagasinCentral.Controllers
{


    /// <summary>
    ///     Contrôleur pour les performances du tableau de bord (UC3).
    /// </summary>
    public class ProduitController : Controller
    {
        private readonly IProduitService _produitService;

        public ProduitController(IProduitService produitService)
        {
            _produitService = produitService ?? throw new ArgumentNullException(nameof(produitService));
        }

        /// <summary>
        /// Afficher la liste des produits disponibles.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var model = await _produitService.GetAllProduitsAsync();
            return View(model);
        }

        /// <summary>
        /// Modifier un produit existant.
        /// </summary>
        /// <param name="produitId"></param>
        public async Task<IActionResult> Modifier(int produitId)
        {
            var produit = await _produitService.GetProduitByIdAsync(produitId);
            if (produit == null)
            {
                return NotFound($"Produit avec ID={produitId} non trouvé.");
            }

            return View(produit);
        }

        /// <summary>
        /// Modifier un produit existant avec les données du formulaire.
        /// </summary>
        /// <param name="produit"></param>
        [HttpPost]
        public async Task<IActionResult> Modifier(int produitId, ProduitDto produit)
        {
            IActionResult result = null!;

            if (!ModelState.IsValid)
            {
                result = View(produit);
            }

            try
            {
                await _produitService.ModifierProduitAsync(produitId, produit);
                TempData["Succès"] = $"Le produit « {produit.Nom} » a bien été mis à jour.";
                result = RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Erreur lors de la mise à jour : {ex.Message}");
                result = View(produit);
            }

            return result;
        }

        /// <summary>
        /// Recherche de produits par identifiant, nom ou catégorie.
        /// </summary>
        /// <param name="produit"></param>
        /// <returns></returns>
        public async Task<IActionResult> Recherche(string produit)
        {
            if (string.IsNullOrWhiteSpace(produit))
                return View(new List<Produit>());

            var résultats = await _produitService.RechercherProduitsAsync(produit);
            ViewData["Terme"] = produit;
            return View(résultats);
        }

    }
}