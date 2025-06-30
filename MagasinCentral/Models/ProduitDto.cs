using System.ComponentModel.DataAnnotations;

namespace MagasinCentral.Models
{
    public class ProduitDto
    {
        /// <summary>
        /// Nom du produit.
        /// </summary>
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
        public decimal Prix { get; set; }

        /// <summary>
        /// Description du produit.
        /// </summary>
        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
