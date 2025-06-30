using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MagasinCentral.Models
{
    /// <summary>
    /// Représente un magasin physique.
    /// </summary>
    public class Magasin
    {
        /// <summary>
        /// Clé primaire du magasin.
        /// </summary>
        [Key]
        public int MagasinId { get; set; }

        /// <summary>
        /// Nom du magasin.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Nom { get; set; } = string.Empty;

        /// <summary>
        /// Adresse du magasin.
        /// </summary>
        [MaxLength(200)]
        public string? Adresse { get; set; }

        /// <summary>
        /// Collection des stocks locaux par produit.
        /// </summary>
        public ICollection<MagasinStockProduit> StocksProduits { get; set; } = new List<MagasinStockProduit>();

        /// <summary>
        /// Collection des ventes effectuées dans ce magasin.
        /// </summary>
        public ICollection<Vente> Ventes { get; set; } = new List<Vente>();

        public ICollection<DemandeReapprovisionnement> DemandesReapprovisionnement { get; set; } = new List<DemandeReapprovisionnement>();
    }
}
