# Séparation des responsabilités : Présentation et Persistance

## Status

Acceptée

## Contexte

L'application doit rester maintenable, claire, et évolutive. Pour cela, il est essentiel de bien séparer la couche présentaion, logique métier et le serveur qui contient les données essentielles de l'application. Une architecture N-tier à 3 couches est donc un bon choix

## Décision

J'ai choisi de structurer l’application en 3 couches :

- **Présentation** : Les pages web dont l'utilisateur utilise pour intéragir avec l'application (Dossier: Controllers & Views)
- **Logique du Métier** : Contient la logique pour faire les opérations necessaires (Dossier: Services, Models, et Data)
- **Persistance** : accès aux données via l’ORM Entity Framework

## Conséquences

- Facilite les tests unitaires indépendants de la console ou de la base de données.
- Meilleure lisibilité et modularité du code.
- Prépare l’architecture à une éventuelle migration vers une API ou une interface graphique.
