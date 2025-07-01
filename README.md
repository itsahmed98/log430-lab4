# LOG430 - Laboratoire 4 : Monitoring et Cache

Ce projet met en place une application .NET Core composée de plusieurs microservices (Magasin, Produits, Ventes, etc.) monitorée avec Prometheus, Grafana, et avec mise en cache locale (MemoryCache).

## 📁 Cloner le projet

```bash
git clone https://github.com/itsahmed98/log430-lab4.git
cd log430-lab4
```

---

## ⚙️ Lancer l'application (mode production via Docker Compose)

1. **Vérifiez que Docker est installé et en cours d'exécution.**
2. **Lancer tous les services (app, base de données, Prometheus, Grafana, Redis, etc.)**

```bash
docker compose up --build
```

Cela lancera les services suivants :

- API de l’application (`app1`, `app2`, …)
- PostgreSQL
- Prometheus (monitoring)
- Grafana (dashboard de visualisation)
- Node Exporter (métriques systèmes)
- Redis (optionnel si utilisé)

---

## 🔗 Accès aux interfaces

| Service            | URL                                      |
| ------------------ | ---------------------------------------- |
| Application        | http://localhost (port 80 exposé)        |
| Prometheus         | http://localhost:9090                    |
| Prometheus Targets | http://localhost:9090/targets            |
| Grafana            | http://localhost:3000                    |
| Grafana Login      | `admin` / `admin` (changer au 1er login) |

---

## 📈 Monitoring avec Prometheus & Grafana

### 1. Configuration de Prometheus

Prometheus est configuré pour scrapper :

- L'application (`/metrics` via port 80)
- `node-exporter` (`:9100`)

Fichier `prometheus.yml` déjà configuré dans le repo.

### 2. Configuration Grafana

- Lancer Grafana et ajouter Prometheus comme source de données.
- Importer les dashboards fournis (ou créer vos propres panels avec les requêtes PromQL).

Exemples de requêtes utiles :

```promql
rate(http_requests_received_total[1m]) by (code)
histogram_quantile(0.95, rate(http_request_duration_seconds_bucket[1m]))
```

---

## ⚖️ Test des Stratégies de Load Balancing

Configuration dans `nginx.conf` (Docker) avec plusieurs stratégies :

```nginx
upstream magasin_api {
    least_conn;
    server app1:80 resolve;
    server app2:80 resolve;
    ...
}
```

Pour tester une stratégie différente :

1. Décommentez la section souhaitée (least_conn, round robin, ip_hash...)
2. Rebuild avec `docker compose up --build`

---

## 🔁 Cache mémoire local

Le cache est implémenté dans les services suivants :

- RapportService
- PerformancesService
- ProduitService

Le cache utilise `IMemoryCache` avec une expiration de 5 à 10 minutes selon le service. Cela permet de réduire la charge sur la base de données.

---

## 🧪 Lancer en local (hors Docker)

1. S’assurer que PostgreSQL est en cours d’exécution localement.
2. Modifier `appsettings.Development.json` avec votre chaîne de connexion locale.
3. Lancer l’app depuis Visual Studio ou via la CLI :

```bash
dotnet run --project MagasinCentral
```

L’URL locale sera typiquement : `https://localhost:7230`

⚠️ Pour le cache local, aucune configuration supplémentaire n’est nécessaire.

---

## 🧯 Test de tolérance aux pannes

1. Lancer plusieurs instances (`app1`, `app2`, etc.)
2. Arrêter une instance avec :

```bash
docker stop app1
```

3. Observer via Grafana que le service continue (le load balancer redirige vers les autres instances).

---

## 🧼 Nettoyage

```bash
docker compose down -v
```

---

## Auteur

Projet réalisé par **Ahmed Sherif** dans le cadre du cours **LOG430** à l’ÉTS.
