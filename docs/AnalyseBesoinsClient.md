# Besoins du client

## Exigences Fonctionnelles

| Id   | Spécification de l’exigence fonctionnelle                                                                                            |
| :--- | :----------------------------------------------------------------------------------------------------------------------------------- |
| EF01 | L’application doit permettre au gestionnaire de générer un rapport détaillé des magasins.                                            |
| EF02 | L’application doit permettre à l’employé d’un magasin de consulter le stock disponible.                                              |
| EF03 | L’application doit permettre à l’employé de faire une demande de réapprovisionnement lorsque le stock de son magasin est en rupture. |
| EF04 | L’application doit permettre au gestionnaire de visualiser les performances des magasins dans un tableau de bord.                    |
| EF05 | L’application doit permettre au responsable de modifier un produit depuis la maison mère.                                            |
| EF06 | L’application doit permettre au responsable de valider une demande de réapprovisionnement.                                           |

## Exigences non-fonctionnelles

1. **Performance** : Les opérations (consultation du stock, enregistrement des ventes) doivent s’exécuter en moins de 2 secondes.

2. **Robustesse** : Le système doit gérer correctement les erreurs de saisie et les accès invalides à la base de données.

3. **Cohérence des données** : Les écritures dans la base de données doivent être atomiques et respecter les transactions pour éviter les incohérences (surtout en cas d'accès concurrent par plusieurs caisses).

4. **Fiabilité** : Le système doit garantir l’intégrité des données même en cas d’interruption du processus (plantage ou arrêt inattendu).

5. **Portabilité** : L’application doit être exécutable dans un conteneur Docker pour assurer une installation uniforme sur toute machine.

6. **Facilité de déploiement** : Le système doit pouvoir être lancé avec une seule commande (`docker compose up`) pour simplifier le test et l’installation.

7. **Maintenabilité** : Le code doit être structuré en couches séparées (présentation, logique métier, persistance) pour faciliter l'évolution future du projet.
