using System.Collections.Generic;
using System.Threading.Tasks;
using MagasinCentral.Models;
using MagasinCentral.ViewModels;

namespace MagasinCentral.Services
{
    /// <summary>
    /// Interface qui définit les opérations des demandes de réapprovisionnement.
    /// </summary>
    public interface IReapprovisionnementService
    {
        /// <summary>
        /// Retourne la liste des produits avec stock local et stock central pour un magasin donné.
        /// </summary>
        /// <param name="magasinId">Identifiant du magasin.</param>
        Task<List<StockVue>> GetStocksAsync(int magasinId);

        /// <summary>
        /// Crée une nouvelle demande de réapprovisionnement pour un produit dans un magasin.
        /// </summary>
        /// <param name="magasinId">Identifiant du magasin demandeur.</param>
        /// <param name="produitId">Identifiant du produit concerné.</param>
        /// <param name="quantiteDemande">Quantité demandée pour réapprovisionnement.</param>
        Task CreerDemandeReapprovisionnementAsync(int magasinId, int produitId, int quantiteDemande);

        /// <summary>
        /// Récupère la liste des demandes de réapprovisionnement.
        /// </summary>
        /// <returns></returns>
        Task<List<DemandeReapprovisionnement>> GetDemandesReapprovisionnementAsync();

        /// <summary>
        /// Récupère uniquement les demandes en statut "EnAttente".
        /// </summary>
        Task<List<DemandeReapprovisionnement>> GetDemandesEnAttenteAsync();

        /// <summary>
        /// Traiter une demande en attente
        /// </summary>
        Task TraiterDemandeAsync(int demandeId, bool approuver);
    }
}
