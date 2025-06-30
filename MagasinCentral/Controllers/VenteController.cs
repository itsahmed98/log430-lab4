using Microsoft.AspNetCore.Mvc;
using MagasinCentral.Services;
using MagasinCentral.Data;
using Microsoft.EntityFrameworkCore;
using MagasinCentral.Models;

namespace MagasinCentral.Controllers;

/// <summary>
/// Controller pour gérer les ventes.
/// </summary>
public class VenteController : Controller
{
    private readonly IVenteService _venteService;
    private readonly IProduitService _produitService;
    private readonly MagasinDbContext _contexte;

    /// <summary>
    /// Constructeur pour initialiser les services nécessaires à la gestion des ventes.
    /// </summary>
    /// <param name="venteService"></param>
    /// <param name="produitService"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public VenteController(IVenteService venteService, IProduitService produitService, MagasinDbContext contexte)
    {
        _venteService = venteService ?? throw new ArgumentNullException(nameof(venteService));
        _produitService = produitService ?? throw new ArgumentNullException(nameof(produitService));
        _contexte = contexte ?? throw new ArgumentNullException(nameof(contexte));
    }


    /// <summary>
    /// Affiche le formulaire pour enregistrer une vente dans un magasin spécifique.
    /// </summary>
    /// <param name="magasinId"></param>
    public async Task<IActionResult> Enregistrer(int? magasinId)
    {
        ViewData["Magasins"] = await _contexte.Magasins.ToListAsync();
        ViewData["MagasinId"] = magasinId ?? 0;
        var produits = await _produitService.GetAllProduitsAsync();
        return View(produits);
    }


    /// <summary>
    /// Enregistre une vente pour un magasin donné avec les produits et quantités spécifiés.
    /// </summary>
    /// <param name="magasinId"></param>
    /// <param name="produitId"></param>
    /// <param name="quantite"></param>
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Enregistrer(int magasinId, List<int> produitId, List<int> quantite)
    {
        if (magasinId <= 0)
            return BadRequest("Magasin invalide, veuillez en sélectionner un.");

        var lignes = produitId
            .Select((id, i) => (id, quantite[i]))
            .Where(x => x.Item2 > 0)
            .ToList();
        if (!lignes.Any())
        {
            TempData["Erreur"] = "Veuillez sélectionner au moins 1 produit.";
            return RedirectToAction(nameof(Enregistrer), new { magasinId });
        }
        var venteId = await _venteService.CreerVenteAsync(magasinId, lignes);
        TempData["Succès"] = $"Vente #{venteId} créée.";
        return RedirectToAction(nameof(Liste));
    }

    /// <summary>
    /// Annule une vente existante et restitue le stock au magasin.
    /// </summary>
    /// <param name="venteId"></param>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Retour(int venteId)
    {
        try
        {
            await _venteService.AnnulerVenteAsync(venteId);
            TempData["Succès"] = "Vente annulée et stock restitué.";
        }
        catch (Exception ex)
        {
            TempData["Erreur"] = ex.Message;
        }
        return RedirectToAction("Liste", "Vente");
    }

    /// <summary>
    /// Affiche la liste de toutes les ventes enregistrées, incluant les informations sur le magasin et les produits.
    /// </summary>
    public async Task<IActionResult> Liste()
    {
        List<Vente> ventes = await _venteService.GetVentesAsync();
        return View(ventes);
    }


}
