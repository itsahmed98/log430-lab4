using MagasinCentral.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MagasinCentral.Data
{
    /// <summary>
    /// Contexte EF Core pour MagasinCentral (PostgreSQL).
    /// </summary>
    public class MagasinDbContext : IdentityDbContext<IdentityUser>
    {
        public MagasinDbContext(DbContextOptions<MagasinDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Table des magasins.
        /// </summary>
        public DbSet<Magasin> Magasins { get; set; } = null!;

        /// <summary>
        /// Table des produits.
        /// </summary>
        public DbSet<Produit> Produits { get; set; } = null!;

        /// <summary>
        /// Table des stocks locaux.
        /// </summary>
        public DbSet<MagasinStockProduit> MagasinStocksProduits { get; set; } = null!;

        /// <summary>
        /// Table du stock central (represent un entrepôt global).
        /// </summary>
        public DbSet<StockCentral> StocksCentraux { get; set; } = null!;

        /// <summary>
        /// Table des ventes.
        /// </summary>
        public DbSet<Vente> Ventes { get; set; } = null!;

        /// <summary>
        /// Table des demandes de réapprovisionnement.
        /// </summary>
        public DbSet<DemandeReapprovisionnement> DemandesReapprovisionnement { get; set; } = null!;

        /// <summary>
        /// Table des lignes de vente (détails des produits vendus dans chaque vente).
        /// </summary>
        public DbSet<LigneVente> LignesVente { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MagasinStockProduit>()
                .HasKey(ms => new { ms.MagasinId, ms.ProduitId });

            modelBuilder.Entity<StockCentral>()
                .HasKey(sc => sc.ProduitId);

            modelBuilder.Entity<StockCentral>()
                .HasOne(sc => sc.Produit)
                .WithOne(p => p.StockCentral)
                .HasForeignKey<StockCentral>(sc => sc.ProduitId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DemandeReapprovisionnement>()
                .HasOne(d => d.Magasin)
                .WithMany(m => m.DemandesReapprovisionnement)
                .HasForeignKey(d => d.MagasinId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DemandeReapprovisionnement>()
                .HasOne(d => d.Produit)
                .WithMany(p => p.DemandesReapprovisionnement)
                .HasForeignKey(d => d.ProduitId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LigneVente>()
                .HasOne(l => l.Vente)
                .WithMany(v => v.Lignes)
                .HasForeignKey(l => l.VenteId);

            DataSeeder.Seed(modelBuilder);
        }
    }
}
