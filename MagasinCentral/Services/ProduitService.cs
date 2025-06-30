using MagasinCentral.Data;
using MagasinCentral.Models;
using Microsoft.EntityFrameworkCore;

namespace MagasinCentral.Services
{
    /// <summary>
    /// Service pour gérer les opérations liées aux produits.
    /// </summary>
    public class ProduitService : IProduitService
    {
        private readonly MagasinDbContext _contexte;

        public ProduitService(MagasinDbContext contexte)
        {
            _contexte = contexte ?? throw new ArgumentNullException(nameof(contexte));
        }

        /// <inheritdoc />
        public async Task<List<Produit>> GetAllProduitsAsync()
        {
            return await _contexte.Produits
                .AsNoTracking()
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Produit?> GetProduitByIdAsync(int produitId)
        {
            return await _contexte.Produits
                .FirstOrDefaultAsync(p => p.ProduitId == produitId);
        }

        /// <inheritdoc />
        public async Task ModifierProduitAsync(int produitId, ProduitDto produitDto)
        {
            // Map ProduitDto to Produit
            var produit = await _contexte.Produits.FirstOrDefaultAsync(p => p.ProduitId == produitId);
            if (produit == null)
            {
                throw new InvalidOperationException("Produit not found.");
            }

            produit.Nom = produitDto.Nom;
            produit.Categorie = produitDto.Categorie;
            produit.Prix = produitDto.Prix;
            produit.Description = produitDto.Description;

            _contexte.Produits.Update(produit);
            await _contexte.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<List<Produit>> RechercherProduitsAsync(string terme)
        {
            terme = terme?.Trim().ToLower() ?? "";
            return await _contexte.Produits
                .AsNoTracking()
                .Where(p =>
                    p.ProduitId.ToString() == terme ||
                    p.Nom.ToLower().Contains(terme) ||
                    (p.Categorie != null && p.Categorie.ToLower().Contains(terme))
                )
                .ToListAsync();
        }
    }
}
