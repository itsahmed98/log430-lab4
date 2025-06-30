namespace MagasinCentral.ViewModels
{
    /// <summary>
    /// Représente la vue des stocks d'un produit dans un magasin et le stock central.
    /// </summary>
    public class StockVue
    {
        /// <summary>
        /// Identifiant du produit.
        /// </summary>
        public int ProduitId { get; set; }

        /// <summary>
        /// Nom du produit.
        /// </summary>
        public string NomProduit { get; set; } = string.Empty;

        /// <summary>
        /// Quantité disponible dans le magasin.
        /// </summary>
        public int QuantiteLocale { get; set; }

        /// <summary>
        /// Quantité disponible dans le stock central.
        /// </summary>
        public int QuantiteCentral { get; set; }
    }
}
