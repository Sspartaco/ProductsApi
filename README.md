# Products API

A .NET 10 Web API for managing **Products** using SQL Server Stored Procedures, following a clean layered architecture. Includes integration with the **GitHub public API** as an external API use case.

## Tech Stack

- **ASP.NET Core 10** (Web API)
- **SQL Server** (Stored Procedures — no EF migrations)
- **Entity Framework Core 10** (as an ORM/query runner, DB-first)
- **AutoMapper 13**
- **Scalar** (interactive API docs at `/scalar/v1`)
- **xUnit + Moq + FluentAssertions** (unit tests)

---

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- SQL Server (local or remote — Express/Developer edition works)
- Git

---

## Database Setup

1. Open **SQL Server Management Studio** (or any SQL client).
2. Connect to your SQL Server instance.
3. Run the script at `sql/setup.sql`.

   This script will:
   - Create the `ProductsDb` database (if it doesn't exist)
   - Create the `Products` table
   - Create all 5 stored procedures (`sp_GetAllProducts`, `sp_GetProductById`, `sp_CreateProduct`, `sp_UpdateProduct`, `sp_DeleteProduct`)
   - Insert 3 sample products

---

## Running Locally

### 1. Clone the repository

```bash
git clone <your-repo-url>
cd ProductsApi
```

### 2. Configure the connection string

Edit `Products.Presentation/appsettings.json` and set your SQL Server connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=ProductsDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Common values for `Server`:
- Local default instance: `localhost` or `.`
- Local named instance: `localhost\SQLEXPRESS`

### 3. Restore & run

```bash
dotnet restore
dotnet run --project Products.Presentation
```

The API starts at `https://localhost:7xxx` (see console output for the exact port).

### 4. Open the interactive docs

Navigate to `https://localhost:<port>/scalar/v1` in your browser.

---

## API Endpoints

### Products (CRUD via Stored Procedures)

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/products` | List all products |
| `GET` | `/api/products/{id}` | Get a product by ID |
| `POST` | `/api/products` | Create a new product |
| `PUT` | `/api/products/{id}` | Update an existing product |
| `DELETE` | `/api/products/{id}` | Delete a product |

**POST / PUT body example:**

```json
{
  "name": "Wireless Headphones",
  "description": "Noise-cancelling over-ear headphones",
  "price": 149.99
}
```

---

### GitHub Integration

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/github/{username}` | Fetch a GitHub user's public profile |

**Example:**

```
GET /api/github/torvalds
```

**Response:**

```json
{
  "login": "torvalds",
  "name": "Linus Torvalds",
  "bio": "Just a simple coder :)",
  "publicRepos": 8,
  "followers": 245000,
  "avatarUrl": "https://avatars.githubusercontent.com/u/1024025",
  "htmlUrl": "https://github.com/torvalds"
}
```

---

## Running Tests

```bash
dotnet test
```

---

## Project Structure

```
ProductsApi/
├── sql/setup.sql                              ← DB + SP creation script
├── Products.Infrastructure.Contracts/     ← Entities + repository interfaces
├── Products.Infrastructure.Implementation/← EF Core DbContext + repository implementations
├── Products.Library.Contracts/            ← DTOs + service interfaces + OperationResult
├── Products.Library.Implementation/       ← Business logic + AutoMapper profiles
├── Products.Presentation/                 ← Web API (controllers, middleware, Program.cs)
└── Products.Tests/                        ← Unit tests (xUnit + Moq + FluentAssertions)
```
