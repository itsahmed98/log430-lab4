using MagasinCentral.Data;
using MagasinCentral.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace MagasinCentral.Services
{
    /// <summary>
    /// Service pour gérer les opérations liées aux produits.
    /// </summary>
    public class ProduitService : IProduitService
    {
        private readonly MagasinDbContext _contexte;
        private readonly IMemoryCache _cache;

        public ProduitService(MagasinDbContext contexte, IMemoryCache cache)
        {
            _contexte = contexte ?? throw new ArgumentNullException(nameof(contexte));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
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
            string cacheKey = $"produit_{produitId}";

            if (_cache.TryGetValue(cacheKey, out Produit? produit))
            {
                return produit;
            }

            produit = await _contexte.Produits.FirstOrDefaultAsync(p => p.ProduitId == produitId);

            if (produit != null)
            {
                var options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
                _cache.Set(cacheKey, produit, options);
            }

            return produit;
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
