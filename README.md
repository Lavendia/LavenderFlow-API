# LavenderFlow-API

LavenderFlow est une API de gestion collaborative de tableaux kanban en temps réel, construite avec ASP.NET Core et SignalR.

## Table des matières

- [Prérequis système](#prérequis-système)
- [Installation et configuration](#installation-et-configuration)
- [Migrations EF Core](#migrations-ef-core)
- [Lancement en développement](#lancement-en-développement)
- [Architecture](#architecture)
- [Documentation API](#documentation-api)
- [Authentification JWT](#authentification-jwt)

---

## Prérequis système

### Versions requises

- **.NET 10.0** ou supérieur
- **PostgreSQL 12** ou supérieur
- **Node.js 18+** (pour le frontend)
- **Git**

### Vérifier les versions installées

```bash
dotnet --version
psql --version
node --version
```

### Base de données

- **PostgreSQL**: Assurez-vous que PostgreSQL est installé et en cours d'exécution
- **Connexion**: Configurez une chaîne de connexion PostgreSQL dans votre fichier `.env`

---

## Installation et configuration

### 1. Cloner le dépôt

```bash
git clone <repository-url>
cd LavenderFlow-API
```

### 2. Créer le fichier `.env`

Créez un fichier `.env` à la racine du projet avec les variables d'environnement suivantes:

```env
DB_CONNECTION=User ID=postgres;Password=your_password;Host=localhost;Port=5432;Database=lavenderflow;
Jwt__Key=your-super-secret-key-minimum-32-characters-long
Jwt__Issuer=LavenderFlow
Jwt__Audience=LavenderFlowClient
```

**Variables importantes:**
- `DB_CONNECTION`: Chaîne de connexion PostgreSQL
- `Jwt__Key`: Clé secrète pour signer les tokens JWT (minimum 32 caractères)
- `Jwt__Issuer`: Émetteur du token JWT
- `Jwt__Audience`: Audience cible du token JWT

### 3. Restaurer les dépendances

```bash
dotnet restore
```

### 4. Configurer la base de données

Assurez-vous que PostgreSQL est en cours d'exécution et créez la base de données:

```bash
createdb lavenderflow
```

---

## Migrations EF Core

### Appliquer les migrations

```bash
dotnet ef database update
```

Cette commande va:
1. Créer les tables dans PostgreSQL
2. Appliquer toutes les migrations en attente
3. Initialiser les données de base (rôles, labels)

### Créer une nouvelle migration

Si vous modifiez les entités:

```bash
dotnet ef migrations add NomDeLaMigration
```

### Afficher les migrations appliquées

```bash
dotnet ef migrations list
```

### Revenir à une migration précédente

```bash
dotnet ef database update NomDeLaMigration
```

---

## Lancement en développement

### Mode watch (rechargement automatique)

```bash
dotnet watch run
```

L'application sera accessible sur:
- **API**: https://localhost:5000
- **Swagger/OpenAPI**: https://localhost:5000/swagger

### Mode normal

```bash
dotnet run
```

### Spécifier l'environnement

```bash
dotnet watch run --environment Development
```

---

## Architecture

### Vue d'ensemble

LavenderFlow suit une architecture en **couches** avec séparation des responsabilités:

```
┌─────────────────────────────────────────┐
│      Controllers (API REST)             │
├─────────────────────────────────────────┤
│      Services (Logique métier)          │
├─────────────────────────────────────────┤
│      Repositories (Accès données)       │
├─────────────────────────────────────────┤
│      DbContext (Entity Framework)       │
├─────────────────────────────────────────┤
│      PostgreSQL (Base de données)       │
└─────────────────────────────────────────┘
│      SignalR Hub (Communications RT)    │
└─────────────────────────────────────────┘
```

### Diagramme des entités

```
┌──────────────┐
│    User      │
├──────────────┤
│ Id           │
│ Email        │
│ Password     │
│ CreatedAt    │
│ UpdatedAt    │
└──────────────┘
        │
        ├─────────────────────────┬──────────────────────┐
        │                         │                      │
┌───────▼──────────┐   ┌─────────▼──────────┐  ┌────────▼─────────┐
│  WorkspaceUser   │   │   BoardUser        │  │ CardAssignment   │
├──────────────────┤   ├────────────────────┤  ├──────────────────┤
│ UserId (PK)      │   │ UserId (PK)        │  │ UserId (PK)      │
│ WorkspaceId (PK) │   │ BoardId (PK)       │  │ CardId (PK)      │
│ RoleId           │   │ RoleId             │  └──────────────────┘
└──────────────────┘   └────────────────────┘

┌──────────────────┐
│   Workspace      │
├──────────────────┤
│ Id               │
│ Name             │
│ Public           │
│ CreatedAt        │
└──────────────────┘
        │
        └──────────────┐
                       │
┌──────────────────────▼───┐
│      Board              │
├──────────────────────────┤
│ Id                       │
│ Name                     │
│ Description              │
│ WorkspaceId (FK)         │
└──────────────────────────┘
        │
        └──────────────┐
                       │
┌──────────────────────▼────────┐
│     ListItem (List/Column)    │
├───────────────────────────────┤
│ Id                            │
│ Name                          │
│ Order                         │
│ BoardId (FK)                  │
└───────────────────────────────┘
        │
        └──────────────┐
                       │
┌──────────────────────▼──────┐
│        Card                │
├─────────────────────────────┤
│ Id                          │
│ Name                        │
│ Description                 │
│ Order                       │
│ Deadline                    │
│ Archived                    │
│ ListItemId (FK)             │
└─────────────────────────────┘
        │
        ├──────────────┬──────────────┬──────────────┐
        │              │              │              │
        │      ┌───────▼──────┐ ┌─────▼────┐ ┌──────▼──────┐
        │      │  Checklist   │ │CardLabel │ │ ChatMessage │
        │      ├──────────────┤ ├──────────┤ ├─────────────┤
        │      │ Id           │ │ CardId   │ │ Id          │
        │      │ Name         │ │ LabelId  │ │ Text        │
        │      │ CardId (FK)  │ └──────────┘ │ CardId (FK) │
        │      └──────────────┘              │ UserId (FK) │
        │              │                     └─────────────┘
        │              │
        │      ┌───────▼────────────┐
        │      │  ChecklistItem     │
        │      ├────────────────────┤
        │      │ Id                 │
        │      │ Name               │
        │      │ Finished           │
        │      │ ChecklistId (FK)   │
        │      └────────────────────┘
        │
        └──────────────────────┐
                               │
                      ┌────────▼─────┐
                      │    Label     │
                      ├──────────────┤
                      │ Id           │
                      │ Name         │
                      │ ColorHex     │
                      └──────────────┘
```

### Structure des fichiers

```
LavenderFlow-API/
├── controller/              # Contrôleurs API REST
├── service/                 # Interfaces et implémentations de services
│   ├── impl/               # Implémentations concrètes
│   └── I*.cs              # Interfaces
├── repository/             # Interfaces et implémentations de repositories
│   ├── impl/              # Implémentations concrètes
│   └── I*.cs             # Interfaces
├── entity/                 # Modèles d'entités (EF Core)
├── dto/                    # Data Transfer Objects
│   ├── request/           # DTOs de requête
│   └── response/          # DTOs de réponse
├── dbContext/             # DbContext et Seeder
├── hub/                   # SignalR Hub
├── Migrations/            # Migrations EF Core
├── Program.cs             # Configuration de l'application
├── LavenderFlow-API.csproj
└── README.md
```

---

## Documentation API

### Accéder à Swagger UI

1. Lancez l'application: `dotnet watch run`
2. Ouvrez votre navigateur: https://localhost:5000/swagger
3. Vous verrez la documentation interactive de toutes les endpoints

### Endpoints principaux

#### **Authentification**
- `POST /api/auth/register` - Enregistrer un nouvel utilisateur
- `POST /api/auth/login` - Se connecter

#### **Workspaces**
- `GET /api/workspaces` - Lister tous les workspaces
- `POST /api/workspaces` - Créer un workspace
- `PUT /api/workspaces/{id}` - Mettre à jour un workspace
- `DELETE /api/workspaces/{id}` - Supprimer un workspace

#### **Boards**
- `GET /api/boards` - Lister tous les tableaux
- `POST /api/boards` - Créer un tableau
- `PUT /api/boards/{id}` - Mettre à jour un tableau
- `DELETE /api/boards/{id}` - Supprimer un tableau

#### **Cards**
- `GET /api/cards` - Lister toutes les cartes
- `GET /api/cards/{id}` - Obtenir une carte
- `POST /api/cards` - Créer une carte
- `PUT /api/cards/{id}` - Mettre à jour une carte
- `DELETE /api/cards/{id}` - Supprimer une carte

#### **Labels**
- `GET /api/labels` - Lister tous les labels
- `POST /api/labels` - Créer un label
- `PUT /api/labels/{id}` - Mettre à jour un label
- `DELETE /api/labels/{id}` - Supprimer un label

#### **Card Labels**
- `GET /api/cardlabels/card/{cardId}` - Obtenir les labels d'une carte
- `POST /api/cardlabels` - Ajouter un label à une carte
- `DELETE /api/cardlabels/card/{cardId}/label/{labelId}` - Retirer un label

**Voir Swagger UI pour la documentation complète et interagir avec les endpoints.**

---

## Authentification JWT

### Vue d'ensemble

LavenderFlow utilise **JWT (JSON Web Tokens)** pour l'authentification sans état. Chaque requête authentifiée doit inclure un token valide.

### Flux d'authentification

```
1. Utilisateur s'enregistre/connecte
   └─> Reçoit un JWT token

2. Utilisateur inclut le token dans le header Authorization
   └─> Authorization: Bearer <token>

3. Serveur valide le token
   └─> Si valide → accès accordé
   └─> Si invalide/expiré → 401 Unauthorized
```

### Enregistrement et connexion

#### **1. Enregistrement**

```bash
POST /api/auth/register
Content-Type: application/json

{
  "name": "exemple"
  "email": "user@example.com",
  "password": "password123"
}
```

**Réponse:**
```json
{
  "message": "User registered successfully",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

#### **2. Connexion**

```bash
POST /api/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "password123"
}
```

**Réponse:**
```json
{
  "message": "Login successful",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### Utilisation du token

#### **Inclure le token dans les requêtes**

```bash
GET /api/workspaces
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

#### **Avec curl**

```bash
curl -X GET "https://localhost:5000/api/workspaces" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### Configuration JWT

Les paramètres JWT sont configurés dans `.env`:

```env
Jwt__Key=your-super-secret-key-minimum-32-characters-long
Jwt__Issuer=LavenderFlow
Jwt__Audience=LavenderFlowClient
```

**Explication:**
- `Jwt__Key`: Clé secrète pour signer les tokens (doit être ≥ 32 caractères en production)
- `Jwt__Issuer`: Qui émet le token
- `Jwt__Audience`: À qui est destiné le token

### Durée de vie du token

- Les tokens JWT ont une durée de vie limitée
- Une fois expiré, l'utilisateur doit se reconnecter
- Pour implémenter un refresh token, modifiez `AuthService.cs`

### Sécurité

**Recommandations de sécurité:**

✅ **À faire:**
- Utiliser HTTPS en production
- Stocker les tokens de manière sécurisée côté client (httpOnly cookie de préférence)
- Ne pas mettre la clé JWT dans le code source
- Utiliser une clé JWT forte (32+ caractères) en production
- Renouveler la clé JWT régulièrement

❌ **À ne pas faire:**
- Envoyer le token en clair dans les logs
- Stocker le token en localStorage
- Utiliser une clé JWT faible
- Mettre la clé JWT dans le .env commité

---

## Communication en temps réel avec SignalR

### Connexion au hub

```javascript
const connection = new signalR.HubConnectionBuilder()
  .withUrl("https://localhost:5000/lavenderFlowHub", {
    accessTokenFactory: () => localStorage.getItem("token")
  })
  .withAutomaticReconnect()
  .build();

await connection.start();
```

### Événements disponibles

- `BoardCreated` - Un tableau a été créé
- `BoardUpdated` - Un tableau a été mis à jour
- `BoardDeleted` - Un tableau a été supprimé
- `CardCreated` - Une carte a été créée
- `CardUpdated` - Une carte a été mise à jour
- `CardDeleted` - Une carte a été supprimée
- `LabelAddedToCard` - Un label a été ajouté à une carte
- `LabelRemovedFromCard` - Un label a été retiré d'une carte
- Et bien d'autres...

---

## Support et contribution

Pour toute question ou contribution, veuillez créer une issue ou une pull request sur le dépôt.

## Contributeurs

- Loïc DELPRAT
- Romain BARREAU