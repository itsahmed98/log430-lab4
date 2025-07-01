using MagasinCentral.Data;
using MagasinCentral.Models;
using MagasinCentral.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MagasinCentral.Services
{
    /// <summary>
    /// Service pour gérer les performances du tableau de bord.
    /// </summary>
    public class PerformancesService : IPerformancesService
    {
        private readonly MagasinDbContext _contexte;
        private readonly IMemoryCache _cache;
        private const int seuilSurstock = 100;

        public PerformancesService(MagasinDbContext contexte, IMemoryCache cache)
        {
            _contexte = contexte ?? throw new ArgumentNullException(nameof(contexte));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<PerformancesViewModel> GetPerformances()
        {
            var cacheKey = "performances";

            if (_cache.TryGetValue(cacheKey, out PerformancesViewModel model))
            {
                return model;
            }

            var viewModel = new PerformancesViewModel();

            var magasins = await _contexte.Magasins
                .AsNoTracking()
                .ToListAsync();

            var ventesParMagasin = await _contexte.LignesVente
                .AsNoTracking()
                .GroupBy(l => l.Vente.MagasinId)
                .Select(g => new
                {
                    MagasinId = g.Key,
                    ChiffreAffaires = g.Sum(l => l.PrixUnitaire * l.Quantite)
                })
                .ToListAsync();

            foreach (var magasin in magasins)
            {
                var infoVente = ventesParMagasin.FirstOrDefault(x => x.MagasinId == magasin.MagasinId);

                viewModel.RevenusParMagasin.Add(new RevenuMagasin
                {
                    MagasinId = magasin.MagasinId,
                    NomMagasin = magasin.Nom,
                    ChiffreAffaires = infoVente?.ChiffreAffaires ?? 0m
                });
            }

            var ruptures = await _contexte.MagasinStocksProduits
                .AsNoTracking()
                .Where(ms => ms.Quantite == 0)
                .Include(ms => ms.Magasin)
                .Include(ms => ms.Produit)
                .ToListAsync();

            foreach (var stock in ruptures)
            {
                viewModel.ProduitsRupture.Add(new StockProduitLocal
                {
                    MagasinId = stock.MagasinId,
                    NomMagasin = stock.Magasin.Nom,
                    ProduitId = stock.ProduitId,
                    NomProduit = stock.Produit.Nom,
                    QuantiteLocale = stock.Quantite
                });
            }

            var surstocks = await _contexte.MagasinStocksProduits
                .AsNoTracking()
                .Where(ms => ms.Quantite > seuilSurstock)
                .Include(ms => ms.Magasin)
                .Include(ms => ms.Produit)
                .ToListAsync();

            foreach (var stock in surstocks)
            {
                viewModel.ProduitsSurstock.Add(new StockProduitLocal
                {
                    MagasinId = stock.MagasinId,
                    NomMagasin = stock.Magasin.Nom,
                    ProduitId = stock.ProduitId,
                    NomProduit = stock.Produit.Nom,
                    QuantiteLocale = stock.Quantite
                });
            }

            DateTime aujourdHui = DateTime.UtcNow.Date;
            DateTime semainePasse = aujourdHui.AddDays(-6);

            var ventesDerniereSemaine = await _contexte.LignesVente
                .AsNoTracking()
                .Include(l => l.Vente)
                .Where(l => l.Vente.Date >= semainePasse && l.Vente.Date < aujourdHui.AddDays(1))
                .ToListAsync();

            var regroupement = ventesDerniereSemaine
                .GroupBy(l => new
                {
                    PharmacyId = l.Vente.MagasinId,
                    Jour = l.Vente.Date.Date
                })
                .Select(g => new
                {
                    MagasinId = g.Key.PharmacyId,
                    Jour = g.Key.Jour,
                    MontantTotal = g.Sum(l => l.PrixUnitaire * l.Quantite)
                })
                .ToList();

            foreach (var magasin in magasins)
            {
                var listeVentesJournalières = new List<VentesQuotidiennes>();

                for (int offset = 0; offset < 7; offset++)
                {
                    var dateCible = semainePasse.AddDays(offset);
                    var element = regroupement
                        .FirstOrDefault(x => x.MagasinId == magasin.MagasinId && x.Jour == dateCible);

                    decimal montant = element?.MontantTotal ?? 0m;

                    listeVentesJournalières.Add(new VentesQuotidiennes
                    {
                        Date = dateCible,
                        MontantVentes = montant
                    });
                }

                viewModel.TendancesHebdomadairesParMagasin.Add(
                    magasin.MagasinId,
                    listeVentesJournalières);
            }

            var options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
            _cache.Set(cacheKey, viewModel, options);

            return viewModel;
        }
    }
}
