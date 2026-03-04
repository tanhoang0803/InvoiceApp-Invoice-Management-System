# InvoiceApp — Invoice Management System

A full-stack web application for managing invoices, built with **ASP.NET Core Razor Pages (.NET 10)** and **PostgreSQL**. Supports the complete invoice lifecycle: create, view, edit, soft-delete, search, and paginate — with multi-service billing and client tracking.

> **Live Demo:** [https://YOUR-APP.onrender.com](https://YOUR-APP.onrender.com)

---

## Features

| Feature | Description |
|---------|-------------|
| Invoice List | Paginated table with search by invoice number or client name |
| Create Invoice | Invoice details + client info + dynamic service rows |
| Edit Invoice | Update all fields, add/remove service rows |
| View Invoice | Read-only detail view with full service breakdown |
| Soft Delete | Invoices are flagged as deleted, never hard-removed |
| Payment Status | `Paid` / `Pending` badge on every invoice |
| Real-time Totals | Service row totals and grand total update as you type |
| Delete Modal | Bootstrap 5 confirmation modal before deleting |
| Toast Notifications | Success message after create, edit, or delete |
| Duplicate Guard | Friendly error if Invoice No already exists |
| Auto Migration | Database schema created automatically on first startup |

---

## Tech Stack

| Layer | Technology |
|-------|------------|
| Framework | ASP.NET Core Razor Pages (.NET 10) |
| ORM | Entity Framework Core 10 |
| Database | PostgreSQL (Neon.tech in production) |
| UI | Bootstrap 5 + vanilla JS |
| Hosting | Render (Docker container) |

---

## Database Schema

```
Invoices              Clients               InvoiceServices
────────────────      ────────────────      ────────────────────
Id          PK        Id          PK        Id          PK
InvoiceNo             Name                  InvoiceId   FK → Invoices
PaymentStatus         Email                 ServiceName
IssueDate             Phone                 Quantity
DueDate               Address               UnitPrice
ClientId    FK                              TotalPrice
TotalAmount
CreatedAt
IsDeleted
```

**Business rules:**
```
InvoiceService.TotalPrice = Quantity × UnitPrice
Invoice.TotalAmount       = SUM(all InvoiceService.TotalPrice)
```

---

## Project Structure

```
invoice_App/
├── Dockerfile                          # Multi-stage .NET 10 container build
├── render.yaml                         # Render deployment config
├── invoice_database.slnx
└── invoice_database/
    ├── Data/
    │   └── AppDbContext.cs             # EF Core context, soft-delete filter
    ├── Migrations/                     # PostgreSQL EF migrations
    ├── Models/
    │   ├── Invoice.cs
    │   ├── Client.cs
    │   ├── InvoiceService.cs
    │   ├── PaymentStatus.cs
    │   └── ViewModels/
    │       ├── InvoiceFormViewModel.cs
    │       └── ServiceRowViewModel.cs
    ├── Pages/
    │   ├── Invoices/
    │   │   ├── Index.cshtml(.cs)       # List + search + pagination + delete modal
    │   │   ├── Create.cshtml(.cs)      # Create form
    │   │   ├── Edit.cshtml(.cs)        # Edit form
    │   │   └── Details.cshtml(.cs)    # Read-only detail view
    │   └── Shared/
    │       └── _Layout.cshtml          # Toast notification container
    ├── wwwroot/js/
    │   └── invoice-services.js         # Real-time row/total calculation
    ├── Program.cs
    ├── appsettings.json
    └── invoice_database.csproj
```

---

## Local Development

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- PostgreSQL (local install, Docker, or free [Neon](https://neon.tech) cloud)
- EF Core CLI: `dotnet tool install --global dotnet-ef`

### Setup

```bash
# 1. Clone
git clone https://github.com/tanhoang0803/InvoiceApp-Invoice-Management-System.git
cd InvoiceApp-Invoice-Management-System

# 2. Set your PostgreSQL connection string in invoice_database/appsettings.json
#    "DefaultConnection": "Host=localhost;Port=5432;Database=InvoiceAppDb;Username=postgres;Password=postgres"

# 3. Run (migrations apply automatically on startup)
dotnet run --project invoice_database
```

Open `https://localhost:5001`

---

## Deployment (Render + Neon)

| Service | Purpose |
|---------|---------|
| [Render](https://render.com) | Hosts the .NET app as a Docker container (free tier) |
| [Neon](https://neon.tech) | Serverless PostgreSQL database (free tier) |

Auto-deploys on every push to `main` via Render's native GitHub integration.

```
GitHub push → Render detects change → Docker build → Deploy → Auto-migrate DB
```

---

## Security

- EF Core ORM prevents SQL injection
- DataAnnotations validate all user input
- AntiForgery tokens on every form (built into Razor Pages)
- Soft delete — data is never permanently lost
- SSL enforced on database connection

---

> **Level:** Fresher → Junior — demonstrates CRUD, relational DB design, business logic, Razor Pages architecture, real-time UI, and cloud deployment.
