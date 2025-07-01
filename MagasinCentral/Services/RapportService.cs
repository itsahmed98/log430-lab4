using MagasinCentral.Data;
using MagasinCentral.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MagasinCentral.Services
{
    /// <summary>
    /// Implémentation du service métier pour générer le rapport consolidé.
    /// </summary>
    public class RapportService : IRapportService
    {
        private readonly MagasinDbContext _contexte;
        private readonly IMemoryCache _cache;

        /// <summary>
        /// Constructeur de <see cref="MagasinDbContext"/>.
        /// </summary>
        /// <param name="contexte">Contexte EF Core.</param>
        public RapportService(MagasinDbContext context, IMemoryCache cache)
        {
            _contexte = context;
            _cache = cache;
        }

        /// <inheritdoc />
        public async Task<List<RapportDto>> ObtenirRapportConsolideAsync()
        {
            var cacheKey = "rapport_consolide";
            if (_cache.TryGetValue(cacheKey, out List<RapportDto> rapports))
            {
                return rapports;
            }

            // Charger tous les magasins, leurs ventes et les lignes de ventes + produits
            var listeMagasins = await _contexte.Magasins
                .Include(m => m.Ventes)
                    .ThenInclude(v => v.Lignes)
                        .ThenInclude(l => l.Produit)
                .Include(m => m.StocksProduits)
                    .ThenInclude(sp => sp.Produit)
                .ToListAsync();

            rapports = new List<RapportDto>();

            foreach (var magasin in listeMagasins)
            {
                // Calcul du CA = somme de toutes les lignes de toutes les ventes
                decimal chiffreAffaires = magasin.Ventes
                    .SelectMany(v => v.Lignes)
                    .Sum(l => l.Quantite * l.PrixUnitaire);

                // Top 3 produits par quantité vendue
                var topProduits = magasin.Ventes
                    .SelectMany(v => v.Lignes)
                    .GroupBy(l => l.Produit)
                    .Select(g => new InfosVenteProduit
                    {
                        NomProduit = g.Key.Nom,
                        QuantiteVendue = g.Sum(x => x.Quantite),
                        TotalVentes = g.Sum(x => x.Quantite * x.PrixUnitaire)
                    })
                    .OrderByDescending(info => info.QuantiteVendue)
                    .Take(3)
                    .ToList();

                // Stocks restants local
                var stocksRestants = magasin.StocksProduits
                    .Select(sp => new InfosStockProduit
                    {
                        NomProduit = sp.Produit.Nom,
                        QuantiteRestante = sp.Quantite
                    })
                    .ToList();

                rapports.Add(new RapportDto
                {
                    NomMagasin = magasin.Nom,
                    ChiffreAffairesTotal = chiffreAffaires,
                    TopProduits = topProduits,
                    StocksRestants = stocksRestants
                });
            }

            var listeStockCentral = await _contexte.StocksCentraux
                .Include(sc => sc.Produit)
                .ToListAsync();

            rapports.Add(new RapportDto
            {
                NomMagasin = "Stock Central",
                ChiffreAffairesTotal = 0m,
                TopProduits = new List<InfosVenteProduit>(),
                StocksRestants = listeStockCentral
                    .Select(sc => new InfosStockProduit
                    {
                        NomProduit = sc.Produit.Nom,
                        QuantiteRestante = sc.Quantite
                    })
                    .ToList()
            });

            var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            _cache.Set(cacheKey, rapports, cacheOptions);
            return rapports;
        }
    }
}
