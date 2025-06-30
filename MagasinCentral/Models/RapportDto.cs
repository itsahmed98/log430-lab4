using System.Collections.Generic;

namespace MagasinCentral.Models
{
    /// <summary>
    /// DTO (Data Transfer Object) pour la vue du rapport.
    /// </summary>
    public class RapportDto
    {
        /// <summary>
        /// Nom du magasin (ou "Stock Central" pour l’entrepôt).
        /// </summary>
        public string NomMagasin { get; set; } = string.Empty;

        /// <summary>
        /// Chiffre d’affaires total du magasin.
        /// </summary>
        public decimal ChiffreAffairesTotal { get; set; }

        /// <summary>
        /// Liste des produits les plus vendus (top 3) dans ce magasin.
        /// </summary>
        public List<InfosVenteProduit> TopProduits { get; set; } = new List<InfosVenteProduit>();

        /// <summary>
        /// Quantités restantes pour chaque produit (dans ce magasin ou stock central).
        /// </summary>
        public List<InfosStockProduit> StocksRestants { get; set; } = new List<InfosStockProduit>();
    }

    /// <summary>
    /// Informations sur les ventes d’un produit (top produits).
    /// </summary>
    public class InfosVenteProduit
    {
        public string NomProduit { get; set; } = string.Empty;
        public int QuantiteVendue { get; set; }
        public decimal TotalVentes { get; set; }
    }

    /// <summary>
    /// Informations sur le stock restant d’un produit.
    /// </summary>
    public class InfosStockProduit
    {
        public string NomProduit { get; set; } = string.Empty;
        public int QuantiteRestante { get; set; }
    }
}
