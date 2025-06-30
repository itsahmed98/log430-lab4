using MagasinCentral.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MagasinCentral.Services
{
    /// <summary>
    /// Interface avec les opérations pour gérer/mettre à jour les produits.
    /// </summary>
    public interface IProduitService
    {
        /// <summary>
        /// Retourne la liste de tous les produits.
        /// </summary>
        Task<List<Produit>> GetAllProduitsAsync();

        /// <summary>
        /// Récupère un produit par son ID.
        /// </summary>
        Task<Produit?> GetProduitByIdAsync(int produitId);

        /// <summary>
        /// Met à jour un produit existant.
        /// </summary>
        Task ModifierProduitAsync(int produitId, ProduitDto produit);

        /// <summary>Recherche de produits par identifiant, nom ou catégorie.</summary>
        Task<List<Produit>> RechercherProduitsAsync(string terme);

    }
}
