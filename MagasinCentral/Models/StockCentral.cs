using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagasinCentral.Models
{
    /// <summary>
    /// Représente le stock central (entrepôt) pour tous les magasins.
    /// Chaque produit y a une quantité globale.
    /// </summary>
    public class StockCentral
    {
        [Key]
        [ForeignKey(nameof(Produit))]
        public int ProduitId { get; set; }

        /// <summary>
        /// Quantité totale du produit dans le stock central.
        /// </summary>
        [Required]
        public int Quantite { get; set; }

        /// <summary>
        /// Propriété de navigation vers le produit.
        /// </summary>
        public Produit Produit { get; set; } = null!;
    }
}
