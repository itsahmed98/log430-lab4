# Choix des technologies : C#, PostgreSQL, Entity Framework Core

## Status

Acceptée

## Contexte

Je dois développer une application de caisse avec une interface console, une persistance robuste, et une compatibilité avec CI/CD et Docker. Il est nécessaire de choisir un langage, une base de données, et une méthode d’accès aux données (ORM) adaptée.

## Décision

J'ai décidé d’utiliser :

- Le langage **C#** avec le framework .NET 8, car l’étudiant est déja familier avec ces technologies
- La base de données relationnelle **PostgreSQL**, car elle est open-source, robuste, compatible avec Docker, et bien supportée par Entity Framework. De plus, l'étudiant possède seulement de l'éxperience avec les bases de données relationnelles en PostgreSQL
- L’ORM **Entity Framework Core**, car il s’intègre naturellement avec C#/.NET, simplifie les requêtes, gère les migrations, et prend en charge PostgreSQL via le provider `Npgsql`.

## Conséquences

- Plus facile à démarrer grâce à l’expérience existante avec C# et .NET.
- Intégration fluide dans Docker et dans GitHub Actions.
- Meilleure productivité grâce à l’automatisation des migrations EF Core.
