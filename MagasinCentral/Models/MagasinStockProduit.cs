using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagasinCentral.Models
{
    /// <summary>
    /// Représente la quantité en stock d'un produit dans un magasin donné.
    /// </summary>
    public class MagasinStockProduit
    {
        /// <summary>
        /// Clé primaire composite (StoreId + ProductId).
        /// </summary>
        public int MagasinId { get; set; }

        /// <summary>
        /// Clé primaire composite (StoreId + ProductId).
        /// </summary>
        public int ProduitId { get; set; }

        /// <summary>
        /// Quantité disponible en stock pour ce produit dans le magasin.
        /// </summary>
        [Required]
        public int Quantite { get; set; }

        /// <summary>
        /// Propriété de navigation vers le magasin.
        /// </summary>
        public Magasin Magasin { get; set; } = null!;

        /// <summary>
        /// Propriété de navigation vers le produit.
        /// </summary>
        public Produit Produit { get; set; } = null!;
    }
}
