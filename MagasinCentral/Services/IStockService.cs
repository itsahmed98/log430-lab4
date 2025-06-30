using MagasinCentral.Models;
using Microsoft.AspNetCore.Mvc;

namespace MagasinCentral.Services
{
    /// <summary>
    /// Interface avec les opérations pour gérer les ventes.
    /// </summary>
    public interface IStockService
    {
        /// <summary>
        /// Récupérer la quantité du stock dans un magasin spécifique.
        /// </summary>
        /// <param name="magasinId"></param>
        /// <returns></returns>
        Task<int?> GetStockByMagasinId(int magasinId);
    }
}