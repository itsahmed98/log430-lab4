using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagasinCentral.Models
{
    /// <summary>
    ///     Représente une vente réalisée dans un magasin.
    /// </summary>
    public class Vente
    {
        [Key]
        public int VenteId { get; set; }

        /// <summary>
        /// Date et heure de la vente.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Clé étrangère vers le magasin.
        /// </summary>
        [Required]
        public int MagasinId { get; set; }

        /// <summary>
        /// Propriété de navigation vers le magasin.
        /// </summary>
        public Magasin Magasin { get; set; } = null!;

        /// <summary>
        ///   Toutes les lignes associées à cette vente.
        /// </summary>
        public ICollection<LigneVente> Lignes { get; set; } = new List<LigneVente>();

    }
}
