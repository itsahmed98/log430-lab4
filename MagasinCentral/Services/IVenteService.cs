using MagasinCentral.ViewModels;
using System.Threading.Tasks;
using MagasinCentral.Models;

namespace MagasinCentral.Services
{
    /// <summary>
    /// Interface avec les opérations pour gérer les ventes.
    /// </summary>
    public interface IVenteService
    {
        /// <summary>
        /// Enregistre une vente pour un magasin donné.
        /// </summary>
        /// <param name="magasinId">La magasin dont la vente est faite</param>
        /// <param name="lignes">Ligne de ventes</param>
        Task<int> CreerVenteAsync(int magasinId, List<(int produitId, int quantite)> lignes);

        /// <summary>
        /// Annuler une vente existante.
        /// </summary>
        /// <param name="venteId">La vente</param>
        Task AnnulerVenteAsync(int venteId);

        /// <summary>
        /// Récupère la liste de toutes les ventes.
        /// </summary>
        Task<List<Vente>> GetVentesAsync();
    }
}
