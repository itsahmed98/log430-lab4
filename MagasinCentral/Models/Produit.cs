using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MagasinCentral.Models
{
    /// <summary>
    /// Représente un produit vendu dans les magasins.
    /// </summary>
    public class Produit
    {
        /// <summary>
        /// Clé primaire du produit.
        /// </summary>
        [Key]
        public int ProduitId { get; set; }

        /// <summary>
        /// Nom du produit.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Nom { get; set; } = string.Empty;

        /// <summary>
        /// Catégorie du produit.
        /// </summary>
        [MaxLength(50)]
        public string? Categorie { get; set; }

        /// <summary>
        /// Prix unitaire actuel.
        /// </summary>
        [Required]
        public decimal Prix { get; set; }

        /// <summary>
        /// Description du produit.
        /// </summary>
        [MaxLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// Collection des stocks locaux par magasin.
        /// </summary>
        public ICollection<MagasinStockProduit> StocksMagasin { get; set; } = new List<MagasinStockProduit>();

        /// <summary>
        /// Collection des ventes associées à ce produit.
        /// </summary>
        public ICollection<Vente> Ventes { get; set; } = new List<Vente>();

        public StockCentral? StockCentral { get; set; }

        /// <summary>
        /// Collection des demandes de réapprovisionnement associées à ce produit.
        /// </summary>
        public ICollection<DemandeReapprovisionnement> DemandesReapprovisionnement { get; set; } = new List<DemandeReapprovisionnement>();
    }
}
