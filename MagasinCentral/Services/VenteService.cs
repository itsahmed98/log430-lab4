using MagasinCentral.Models;
using MagasinCentral.Data;
using Microsoft.EntityFrameworkCore;

namespace MagasinCentral.Services
{
    /// <summary>
    /// Service pour gérer les opérations liées aux ventes.
    /// </summary>
    public class VenteService : IVenteService
    {
        private readonly MagasinDbContext _contexte;

        public VenteService(MagasinDbContext contexte)
        {
            _contexte = contexte ?? throw new ArgumentNullException(nameof(contexte));
        }

        /// <inheritdoc />
        public async Task<int> CreerVenteAsync(int magasinId, List<(int produitId, int quantite)> lignes)
        {
            if (!lignes.Any())
                throw new ArgumentException("Aucune ligne.");

            var vente = new Vente { MagasinId = magasinId, Date = DateTime.UtcNow };

            foreach (var (pid, q) in lignes.Where(x => x.quantite > 0))
            {
                var prod = await _contexte.Produits.FindAsync(pid);
                var prix = prod?.Prix ?? throw new ArgumentException($"Produit {pid} invalide");

                vente.Lignes.Add(new LigneVente
                {
                    ProduitId = pid,
                    Quantite = q,
                    PrixUnitaire = prix
                });

                MagasinStockProduit stockLocal = await _contexte.MagasinStocksProduits
                    .FirstOrDefaultAsync(ms => ms.MagasinId == magasinId && ms.ProduitId == pid);

                if (stockLocal == null)
                {
                    // Si le stock n'existe pas, on le crée avec une quantité de 0
                    stockLocal = new MagasinStockProduit
                    {
                        MagasinId = magasinId,
                        ProduitId = pid,
                        Quantite = 0
                    };
                    _contexte.MagasinStocksProduits.Add(stockLocal);
                }

                if (stockLocal.Quantite < q)
                    throw new InvalidOperationException($"Stock insuffisant pour le produit {pid} dans le magasin {magasinId}.");

                stockLocal.Quantite -= q; // Décrémente la quantité du stock local

            }
            _contexte.Ventes.Add(vente);
            await _contexte.SaveChangesAsync();
            return vente.VenteId;
        }

        /// <inheritdoc />
        public async Task AnnulerVenteAsync(int venteId)
        {
            var vente = await _contexte.Ventes
                .Include(v => v.Lignes)
                .FirstOrDefaultAsync(v => v.VenteId == venteId)
                ?? throw new ArgumentException("Vente introuvable.");

            // pour chaque ligne, restituer la quantité
            foreach (var l in vente.Lignes)
            {
                var stock = await _contexte.MagasinStocksProduits
                    .FirstOrDefaultAsync(ms =>
                        ms.MagasinId == vente.MagasinId &&
                        ms.ProduitId == l.ProduitId);

                if (stock != null)
                    stock.Quantite += l.Quantite;
            }

            _contexte.Ventes.Remove(vente);
            await _contexte.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<List<Vente>> GetVentesAsync()
        {
            return await _contexte.Ventes
                .Include(v => v.Magasin)
                .Include(v => v.Lignes)
                    .ThenInclude(l => l.Produit)
                .OrderByDescending(v => v.Date)
                .ToListAsync();
        }

    }
}