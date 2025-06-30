using MagasinCentral.Data;
using MagasinCentral.Models;
using MagasinCentral.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MagasinCentral.Services
{
    /// <summary>
    /// Implémentation du service métier
    /// </summary>
    public class ReapprovisionnementService : IReapprovisionnementService
    {
        private readonly MagasinDbContext _contexte;

        public ReapprovisionnementService(MagasinDbContext contexte)
        {
            _contexte = contexte ?? throw new ArgumentNullException(nameof(contexte));
        }

        /// <inheritdoc />
        public async Task<List<StockVue>> GetStocksAsync(int magasinId)
        {
            // Récupérer les stocks locaux et centraux pour un magasin donné
            var stocksLocales = await _contexte.MagasinStocksProduits
                .Where(ms => ms.MagasinId == magasinId)
                .Include(ms => ms.Produit)
                .ToListAsync();

            var stocksCentraux = await _contexte.StocksCentraux
                .Include(sc => sc.Produit)
                .ToListAsync();

            // 3. Construire la liste de StockVue
            var listeStocks = new List<StockVue>();

            foreach (var stockLocal in stocksLocales)
            {
                // Trouver le stock central correspondant
                var sc = stocksCentraux
                    .FirstOrDefault(x => x.ProduitId == stockLocal.ProduitId);

                listeStocks.Add(new StockVue
                {
                    ProduitId = stockLocal.ProduitId,
                    NomProduit = stockLocal.Produit.Nom,
                    QuantiteLocale = stockLocal.Quantite,
                    QuantiteCentral = sc?.Quantite ?? 0
                });
            }

            return listeStocks;
        }

        /// <inheritdoc />
        public async Task CreerDemandeReapprovisionnementAsync(int magasinId, int produitId, int quantiteDemande)
        {
            if (quantiteDemande <= 0)
            {
                throw new ArgumentException("La quantité demandée doit être strictement positive.");
            }

            var magasin = await _contexte.Magasins.FindAsync(magasinId);
            if (magasin == null)
            {
                throw new ArgumentException($"Le magasin d’ID={magasinId} n’existe pas.");
            }

            var produit = await _contexte.Produits.FindAsync(produitId);
            if (produit == null)
            {
                throw new ArgumentException($"Le produit d’ID={produitId} n’existe pas.");
            }

            var demande = new DemandeReapprovisionnement
            {
                MagasinId = magasinId,
                ProduitId = produitId,
                QuantiteDemandee = quantiteDemande,
                DateDemande = DateTime.UtcNow,
                Statut = "EnAttente"
            };

            _contexte.DemandesReapprovisionnement.Add(demande);
            await _contexte.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<List<DemandeReapprovisionnement>> GetDemandesReapprovisionnementAsync()
        {
            return await _contexte.DemandesReapprovisionnement
                .Include(d => d.Magasin)
                .Include(d => d.Produit)
                .OrderByDescending(d => d.DateDemande)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<List<DemandeReapprovisionnement>> GetDemandesEnAttenteAsync()
        {
            return await _contexte.DemandesReapprovisionnement
                .Where(d => d.Statut == "EnAttente")
                .Include(d => d.Magasin)
                .Include(d => d.Produit)
                .OrderBy(d => d.DateDemande)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task TraiterDemandeAsync(int demandeId, bool approuver)
        {
            var demande = await _contexte.DemandesReapprovisionnement
                .Include(d => d.Magasin)
                .Include(d => d.Produit)
                .FirstOrDefaultAsync(d => d.DemandeId == demandeId);

            if (demande == null)
                throw new ArgumentException($"La demande d’ID={demandeId} n’existe pas.");

            if (demande.Statut != "EnAttente")
                throw new InvalidOperationException("Cette demande a déjà été traitée.");

            if (!approuver)
            {
                demande.Statut = "Refuse";
                await _contexte.SaveChangesAsync();
                return;
            }

            var stockCentral = await _contexte.StocksCentraux
                .FirstOrDefaultAsync(sc => sc.ProduitId == demande.ProduitId);

            if (stockCentral == null)
                throw new InvalidOperationException("Le stock central pour ce produit n’existe pas.");

            if (stockCentral.Quantite < demande.QuantiteDemandee)
                throw new InvalidOperationException("Stock central insuffisant pour approuver.");

            stockCentral.Quantite -= demande.QuantiteDemandee;

            var stockLocal = await _contexte.MagasinStocksProduits
                .FirstOrDefaultAsync(ms => ms.MagasinId == demande.MagasinId
                                        && ms.ProduitId == demande.ProduitId);

            if (stockLocal == null)
            {
                stockLocal = new MagasinStockProduit
                {
                    MagasinId = demande.MagasinId,
                    ProduitId = demande.ProduitId,
                    Quantite = demande.QuantiteDemandee
                };
                _contexte.MagasinStocksProduits.Add(stockLocal);
            }
            else
            {
                stockLocal.Quantite += demande.QuantiteDemandee;
            }

            demande.Statut = "Approuve";

            await _contexte.SaveChangesAsync();
        }
    }
}
