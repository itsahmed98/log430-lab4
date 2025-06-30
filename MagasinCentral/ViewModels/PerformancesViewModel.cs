
namespace MagasinCentral.ViewModels
{
    /// <summary>
    ///     Regroupe tous les indicateurs clés du tableau de bord (UC3).
    /// </summary>
    public class PerformancesViewModel
    {
        /// <summary>
        /// Revenus par magasin.
        /// </summary>
        public List<RevenuMagasin> RevenusParMagasin { get; set; } = new List<RevenuMagasin>();

        /// <summary>
        /// Produits en rupture de stock.
        /// </summary>
        public List<StockProduitLocal> ProduitsRupture { get; set; } = new List<StockProduitLocal>();

        /// <summary>
        /// Produits en surstock.
        /// </summary>
        public List<StockProduitLocal> ProduitsSurstock { get; set; } = new List<StockProduitLocal>();

        /// <summary>
        /// Tendances hebdomadaires : ventes quotidiennes par magasin.
        /// </summary>
        public Dictionary<int, List<VentesQuotidiennes>> TendancesHebdomadairesParMagasin { get; set; }
            = new Dictionary<int, List<VentesQuotidiennes>>();
    }

    /// <summary>
    ///  Represente le revenu d'un magasin.
    /// </summary>
    public class RevenuMagasin
    {
        public int MagasinId { get; set; }
        public string NomMagasin { get; set; } = string.Empty;
        public decimal ChiffreAffaires { get; set; }
    }

    /// <summary>
    /// Represente le stock d'un produit dans un magasin local.
    /// </summary>
    public class StockProduitLocal
    {
        public int MagasinId { get; set; }
        public string NomMagasin { get; set; } = string.Empty;
        public int ProduitId { get; set; }
        public string NomProduit { get; set; } = string.Empty;
        public int QuantiteLocale { get; set; }
    }

    /// <summary>
    /// Representes les ventes quotidiennes pour un magasin.
    /// </summary>
    public class VentesQuotidiennes
    {
        public DateTime Date { get; set; }
        public decimal MontantVentes { get; set; }
    }
}
