using System.Collections.Generic;
using System.Threading.Tasks;
using MagasinCentral.Models;

namespace MagasinCentral.Services
{
    /// <summary>
    /// Interface du service pour Rapport.
    /// </summary>
    public interface IRapportService
    {
        /// <summary>
        /// Obtenir le rapport consolidé de tous les magasins et du stock central.
        /// </summary>
        /// <returns>Liste de <see cref="RapportDto"/>.</returns>
        Task<List<RapportDto>> ObtenirRapportConsolideAsync();
    }
}
