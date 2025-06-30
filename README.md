# LABORATOIRE 2 — LOG430 | MagasinCentral à 3-couches

## Répo GitHub (public)

- https://github.com/itsahmed98/log430-lab0
- https://github.com/itsahmed98/log430-lab1
- https://github.com/itsahmed98/log430-lab2-mvc

---

## Brève description de l’application

Ce projet est une application Web (.NET 8) qui étend le fonctionnement de plusieurs caisses d’un commerce. Cette application offre une gestion de plusieurs magasin à partir d'un magasin central

L'application est monolithique et suit une architecture en couche 3-tier

---

## Cas d'utilisation du système

| Id  | Fonction                                                         |
| --- | ---------------------------------------------------------------- |
| UC1 | Générer un rapport consolidé des ventes                          |
| UC2 | Consulter le stock central et déclencher un réapprovisionnement  |
| UC3 | Visualiser les performances des magasins dans un tableau de bord |
| UC4 | Mettre à jour les produits depuis la maison mère                 |
| UC6 | Approvisionner un magasin depuis le centre logistique            |

---

## Suite de tests

Le projet contient un dossier `MagasinCentral.Tests` avec des tests unitaires. (Voir Structure du projet).

### Pour les exécuter :

```bash
cd MagasinCentral.Tests
dotnet test

```

## Structure du projet

```plaintext

log430-lab2-mvc/
├── MagasinCentral/
│ ├── Program.cs
│ ├── Models/
│ ├── Data/
│ ├── Services/
│ └── Migrations/
├── client.Tests/
│ ├── ProduitServiceTests.cs
│ ├── VenteServiceTests.cs
│ └── RetourServiceTests.cs
├── docs/
│ ├── ADR/
│ ├── UML/
│ ├── BesoinsDuClient.md
│ └── Cas-utilisations.md
├── Dockerfile
├── docker-compose.yml
├── .github/
│ └── workflows/
│ └── ci.yml
└── README.md
```

---

## Étapes d’installation et d’exécution

### 1. Cloner le dépôt et aller dans le fichier racine

    - git clone https://github.com/itsahmed98/log430-lab2-mvc.git
    - cd log430-lab2-mvc

### 2. Lancer l'application avec docker compose

    - docker compose up --build -d
    L’application va démarrer une instance du WebApp + PostgreSQL

---

## Image Docker Hub

Les images sont disponible ici: https://hub.docker.com/u/ahmedsherif98

pour récupèrer une imgage - docker pull ahmedsherif98/magasincentral-mvc:latest

## 🚀 CI/CD — Pipeline

- https://github.com/itsahmed98/log430-lab2-mvc/actions

Le pipeline CI/CD :

1. Restaure les dépendances
2. Vérifie la mise en forme du code (Linting)
3. Lance les tests unitaires (avec xunit)
4. Construit l’image Docker
5. Publie l’image sur Docker Hub (avec un tag par defaut "latest")

## Auteur

Ahmed Akram Sherif
Étudiant au baccalauréat en génie logiciel
Cours : LOG430 — Été 2025
