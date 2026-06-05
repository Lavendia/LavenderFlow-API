# LavenderFlow-API

LavenderFlow est une **API de gestion collaborative de tableaux kanban en temps réel**, construite avec ASP.NET Core et SignalR.

## Présentation du Service

### Objectifs du Service Backend
- Fournir une API REST complète et performante pour la gestion des tableaux kanban
- Gérer la synchronisation temps réel via WebSocket (SignalR)
- Implémenter un système d'authentification sécurisé avec JWT
- Appliquer les règles métier et les contrôles d'accès
- Gérer la persistance des données avec PostgreSQL et Entity Framework Core

### Fonctionnalités Principales
- **API REST** complète avec authentification JWT
- **Synchronisation temps réel** via SignalR WebSocket
- **Gestion multi-niveaux** (Workspaces → Boards → Lists → Cards)
- **Système de permissions** granulaires (Workspace & Board roles)
- **Étiquettes colorées** et assignations d'utilisateurs
- **Checklists** et commentaires sur les cartes
- **Migrations de base de données** automatisées avec EF Core

## Table des matières

- [Présentation du service](#-présentation-du-service)
- [Prérequis système](#prérequis-système)
- [Installation et configuration](#installation-et-configuration)
- [Migrations EF Core](#migrations-ef-core)
- [Lancement en développement](#lancement-en-développement)
- [Architecture](#architecture)
- [Documentation API](#documentation-api)
- [Authentification JWT](#authentification-jwt)
- [Communication SignalR](#communication-en-temps-réel-avec-signalr)

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

LavenderFlow suit une **architecture en couches** avec séparation stricte des responsabilités:

```
┌────────────────────────────────────────────┐
│   Controllers (API REST Endpoints)         │
│  - Gestion des requêtes HTTP               │
│  - Validation des entrées                  │
│  - Formatage des réponses                  │
└────────────┬─────────────────────────────┘
             │
┌────────────▼─────────────────────────────┐
│   Services (Logique Métier)              │
│  - Validations métier                     │
│  - Orchestration des opérations           │
│  - Gestion des permissions                │
└────────────┬─────────────────────────────┘
             │
┌────────────▼──────────────────────────────┐
│   Repositories (Accès aux Données)        │
│  - Requêtes LINQ                          │
│  - Opérations CRUD                        │
│  - Optimisations requêtes                 │
└────────────┬──────────────────────────────┘
             │
┌────────────▼──────────────────────────────┐
│   DbContext (Entity Framework Core)       │
│  - Configuration des entités              │
│  - Gestion des relations                  │
│  - Migrations                             │
└────────────┬──────────────────────────────┘
             │
┌────────────▼──────────────────────────────┐
│   PostgreSQL Database                     │
│  - Persistance des données                │
│  - Transactions ACID                      │
└───────────────────────────────────────────┘

┌────────────────────────────────────────────┐
│   SignalR Hub (Communication RT)           │
│  - WebSocket avec clients                  │
│  - Broadcasting d'événements               │
│  - Groupes par Workspace/Board            │
└───────────────────────────────────────────┘
```

### Services et Leurs Responsabilités

#### **Controllers** 🌐
Gèrent les requêtes HTTP et formatent les réponses:
- `AuthController` - Authentification et enregistrement
- `WorkspacesController` - Gestion des workspaces
- `BoardsController` - Gestion des tableaux
- `CardsController` - Gestion des cartes
- `ListItemsController` - Gestion des listes
- `LabelsController` - Gestion des étiquettes
- `UsersController` - Gestion des utilisateurs
- Et bien d'autres...

#### **Services** 🔧
Contiennent la logique métier:
- Validation des règles de gestion
- Vérification des permissions
- Orchestration des opérations complexes
- Émission des événements SignalR
- Interactions avec les repositories

#### **Repositories** 💾
Gèrent l'accès aux données:
- Requêtes LINQ optimisées
- Opérations CRUD
- Jointures complexes
- Abstraction de l'accès aux données

#### **SignalR Hub** 🔌
Gère la communication temps réel:
- Connexion WebSocket des clients
- Broadcasting des événements
- Groupes par workspace/board
- Notifications temps réel

### Modèle de Données

```
User
├── WorkspaceUser (Many) → Workspace
│   ├── WorkspaceRole
│   └── Workspace
│       ├── Board (Many)
│       │   ├── BoardUser
│       │   ├── BoardRole
│       │   ├── ListItem (Many)
│       │   │   └── Card (Many)
│       │   │       ├── CardAssignment
│       │   │       ├── CardLabel
│       │   │       ├── Checklist
│       │   │       │   └── ChecklistItem (Many)
│       │   │       └── ChatMessage (Many)
│       │   └── Label (Many)
│       └── WorkspaceRole (Many)
├── BoardUser (Many)
├── CardAssignment (Many)
└── ChatMessage (Many)
```

### Flux de Communication

**Scénario: Création d'une nouvelle carte**

```
┌──────────────────────────────────────────────────────────┐
│ 1. Frontend                                              │
│    POST /api/lists/{listId}/cards                        │
│    { name: "Nouvelle tâche", description: "..." }        │
└─────────────────┬──────────────────────────────────────┘
                  │ HTTP
┌─────────────────▼──────────────────────────────────────┐
│ 2. CardsController                                     │
│    - Valide la requête                                 │
│    - Appelle CardService.CreateCard()                  │
└─────────────────┬──────────────────────────────────────┘
                  │
┌─────────────────▼──────────────────────────────────────┐
│ 3. CardService                                         │
│    - Vérifie les permissions de l'utilisateur          │
│    - Valide les règles métier                          │
│    - Crée l'entité Card                                │
│    - Appelle le repository                             │
└─────────────────┬──────────────────────────────────────┘
                  │
┌─────────────────▼──────────────────────────────────────┐
│ 4. CardRepository                                      │
│    - Ajoute la carte à DbContext                       │
│    - Sauvegarde en base de données                     │
└─────────────────┬──────────────────────────────────────┘
                  │
┌─────────────────▼──────────────────────────────────────┐
│ 5. PostgreSQL Database                                 │
│    - INSERT INTO Cards (...)                           │
│    - Retourne la carte créée                           │
└─────────────────┬──────────────────────────────────────┘
                  │
┌─────────────────▼──────────────────────────────────────┐
│ 6. Service → SignalR Hub                               │
│    - Émet événement "CardCreated"                      │
│    - Groupe: Workspace[workspaceId]                    │
└─────────────────┬──────────────────────────────────────┘
                  │ WebSocket
┌─────────────────▼──────────────────────────────────────┐
│ 7. Tous les clients connectés au workspace             │
│    - Reçoivent l'événement "CardCreated"               │
│    - Mettent à jour leur UI localement                 │
│    - Sans rechargement                                 │
└──────────────────────────────────────────────────────────┘
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

Les paramètres JWT sont configurés dans `.env` de l'infrastructure:

```env
JWT_SECRET=your-super-secret-key-minimum-32-characters-long
JWT_ISSUER=http://localhost
JWT_AUDIENCE=http://localhost
```

**Explication:**
- `JWT_SECRET`: Clé secrète pour signer les tokens (doit être ≥ 32 caractères en production)
- `JWT_ISSUER`: Qui émet le token
- `JWT_AUDIENCE`: À qui est destiné le token

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