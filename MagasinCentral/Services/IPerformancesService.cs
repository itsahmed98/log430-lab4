using MagasinCentral.ViewModels;
using System.Threading.Tasks;

namespace MagasinCentral.Services
{
    /// <summary>
    /// Interface pour visualiser les performances.
    /// </summary>
    public interface IPerformancesService
    {
        /// <summary>
        /// Récupère les données du tableau de bord pour les performances.
        /// </summary>
        Task<PerformancesViewModel> GetPerformances();
    }
}
