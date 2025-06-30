using System;
using System.Collections.Generic;
using MagasinCentral.Models;
using Microsoft.EntityFrameworkCore;

namespace MagasinCentral.Data
{
    /// <summary>
    /// Fournit des données initiales (4 magasins, 4 produits, stocks, ventes) pour la base.
    /// </summary>
    public static class DataSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            var produits = new List<Produit>
            {
                new Produit { ProduitId = 1, Nom = "Stylo",        Categorie = "Papeterie",     Prix = 1.50m, Description = "Stylo à bille bleu" },
                new Produit { ProduitId = 2, Nom = "Carnet",       Categorie = "Papeterie",     Prix = 3.75m, Description = "Carnet de notes A5" },
                new Produit { ProduitId = 3, Nom = "Clé USB 16 Go", Categorie = "Électronique",  Prix = 12.00m, Description = "Clé USB 16 Go avec protection" },
                new Produit { ProduitId = 4, Nom = "Casque Audio",  Categorie = "Électronique",  Prix = 45.00m, Description = "Casque audio sans fil avec réduction de bruit" }
            };
            modelBuilder.Entity<Produit>().HasData(produits);

            var magasins = new List<Magasin>
            {
                new Magasin { MagasinId = 1, Nom = "Magasin Centre-Ville", Adresse = "10 Rue Principale" },
                new Magasin { MagasinId = 2, Nom = "Magasin Université",     Adresse = "5 Avenue des Étudiants" },
                new Magasin { MagasinId = 3, Nom = "Magasin Quartier Nord",  Adresse = "23 Boulevard Nord" },
                new Magasin { MagasinId = 4, Nom = "Magasin Sud-Ouest",      Adresse = "42 Rue du Commerce" }
            };
            modelBuilder.Entity<Magasin>().HasData(magasins);

            var stockLocaux = new List<MagasinStockProduit>();
            foreach (var magasin in magasins)
            {
                foreach (var produit in produits)
                {
                    // Créer des produits en surstock ou en rupture de stock selon les conditions
                    int quantiteInitiale = 50;
                    if (magasin.MagasinId == 1 && produit.ProduitId == 1)
                    {
                        quantiteInitiale = 0;
                    }
                    else if (magasin.MagasinId == 1 && produit.ProduitId == 2)
                    {
                        quantiteInitiale = 150;
                    }

                    stockLocaux.Add(new MagasinStockProduit
                    {
                        MagasinId = magasin.MagasinId,
                        ProduitId = produit.ProduitId,
                        Quantite = quantiteInitiale
                    });
                }
            }
            modelBuilder.Entity<MagasinStockProduit>().HasData(stockLocaux);

            var stocksCentraux = new List<StockCentral>();
            foreach (var produit in produits)
            {
                stocksCentraux.Add(new StockCentral
                {
                    ProduitId = produit.ProduitId,
                    Quantite = 200
                });
            }
            modelBuilder.Entity<StockCentral>().HasData(stocksCentraux);

            var ventes = new List<Vente>
            {
                new Vente { VenteId = 1, MagasinId = 1, Date = DateTime.UtcNow.AddDays(-2) },
                new Vente { VenteId = 2, MagasinId = 2, Date = DateTime.UtcNow.AddDays(-1) },
                new Vente { VenteId = 3, MagasinId = 1, Date = DateTime.UtcNow.AddDays(-1) },
                new Vente { VenteId = 4, MagasinId = 3, Date = DateTime.UtcNow }
            };
            modelBuilder.Entity<Vente>().HasData(ventes);

            var lignesVente = new List<LigneVente>
            {
                new LigneVente { LigneVenteId = 1, VenteId = 1, ProduitId = 1, Quantite = 2, PrixUnitaire = 1.50m },
                new LigneVente { LigneVenteId = 2, VenteId = 1, ProduitId = 2, Quantite = 1, PrixUnitaire = 3.75m },
                new LigneVente { LigneVenteId = 3, VenteId = 2, ProduitId = 3, Quantite = 5, PrixUnitaire = 12.00m },
                new LigneVente { LigneVenteId = 4, VenteId = 3, ProduitId = 4, Quantite = 1, PrixUnitaire = 45.00m },
                new LigneVente { LigneVenteId = 5, VenteId = 4, ProduitId = 1, Quantite = 3, PrixUnitaire = 1.50m }
            };
            modelBuilder.Entity<LigneVente>().HasData(lignesVente);
        }
    }
}
