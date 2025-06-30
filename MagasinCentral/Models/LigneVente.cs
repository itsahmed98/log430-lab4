using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagasinCentral.Models
{
    /// <summary>
    /// Représente une ligne de vente dans une transaction de vente.
    /// </summary>
    public class LigneVente
    {
        [Key]
        public int LigneVenteId { get; set; }

        [Required]
        public int VenteId { get; set; }

        [ForeignKey(nameof(VenteId))]
        public Vente Vente { get; set; } = null!;

        [Required]
        public int ProduitId { get; set; }

        [ForeignKey(nameof(ProduitId))]
        public Produit Produit { get; set; } = null!;

        [Required]
        public int Quantite { get; set; }

        [Required]
        public decimal PrixUnitaire { get; set; }
    }
}