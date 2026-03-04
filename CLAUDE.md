# InvoiceApp – CLAUDE.md

## Project Overview
Invoice Management System built with ASP.NET Core Razor Pages (.NET 10) and PostgreSQL (Npgsql).
Deployed on Render (Docker) with Neon.tech as the cloud PostgreSQL provider.
Supports full invoice lifecycle: create, view, edit, soft-delete, paginate, search, and track payment status.

## Tech Stack
- **Framework**: ASP.NET Core Razor Pages (.NET 10)
- **ORM**: Entity Framework Core 10 (Npgsql provider)
- **Database**: PostgreSQL (Neon.tech in production, local Postgres in dev)
- **UI**: Razor Pages + Bootstrap 5
- **Hosting**: Render (Docker container)
- **Solution file**: `invoice_database.slnx`
- **Project folder**: `invoice_database/`

## Project Structure
```
invoice_App/
├── Dockerfile                          # Multi-stage .NET 10 build for Render
├── render.yaml                         # Render deployment blueprint
├── invoice_database.slnx
└── invoice_database/
    ├── Data/
    │   └── AppDbContext.cs             # EF Core context, global soft-delete filter, cascade config
    ├── Migrations/                     # PostgreSQL-compatible EF migrations
    ├── Models/
    │   ├── Invoice.cs                  # Has IsDeleted for soft delete
    │   ├── Client.cs
    │   ├── InvoiceService.cs
    │   ├── PaymentStatus.cs            # Enum: Pending / Paid
    │   └── ViewModels/
    │       ├── InvoiceFormViewModel.cs # Shared Create/Edit form model
    │       └── ServiceRowViewModel.cs
    ├── Pages/
    │   ├── Invoices/
    │   │   ├── Index.cshtml(.cs)       # List + search + pagination + Bootstrap delete modal
    │   │   ├── Create.cshtml(.cs)      # Create invoice form
    │   │   ├── Edit.cshtml(.cs)        # Edit invoice form
    │   │   └── Details.cshtml(.cs)    # Read-only invoice detail view
    │   └── Shared/
    │       └── _Layout.cshtml          # Toast notification container (TempData-driven)
    ├── wwwroot/js/
    │   └── invoice-services.js         # Real-time service row totals + grand total
    ├── Program.cs                      # Startup: PORT binding, URI→ADO.NET converter, auto-migrate
    ├── appsettings.json
    └── invoice_database.csproj
```

## Database Schema
```
Invoices         Clients          InvoiceServices
--------         -------          ---------------
Id (PK)          Id (PK)          Id (PK)
InvoiceNo        Name             InvoiceId (FK)
PaymentStatus    Email            ServiceName
IssueDate        Phone            Quantity
DueDate          Address          UnitPrice
ClientId (FK)                     TotalPrice
TotalAmount
CreatedAt
IsDeleted
```

## Key Business Rules
- `InvoiceService.TotalPrice = Quantity * UnitPrice`
- `Invoice.TotalAmount = SUM(all InvoiceService.TotalPrice)`
- PaymentStatus values: `Paid` / `Pending`
- Deleting an invoice sets `IsDeleted = true` (soft delete) — cascade deletes InvoiceServices via EF
- InvoiceNo must be unique (DB index) — duplicate caught with `DbUpdateException` → ModelState error
- Global query filter on Invoice: `!IsDeleted` — soft-deleted records invisible to all queries

## Implemented Features (Complete)
- ✅ Invoice CRUD: Create, Read (Details), Edit, soft-Delete
- ✅ Invoice list with search (by InvoiceNo or client name) and pagination (10/page)
- ✅ Payment status badge (Paid / Pending)
- ✅ Dynamic service rows with real-time totals (invoice-services.js)
- ✅ Bootstrap 5 modal for delete confirmation
- ✅ TempData toast notifications after create / edit / delete
- ✅ Duplicate InvoiceNo validation (catches DbUpdateException)
- ✅ Auto-apply EF migrations at startup (db.Database.Migrate())
- ✅ Render deployment (Docker + PORT env var binding)
- ✅ Neon PostgreSQL connection (URI → ADO.NET converter in Program.cs)

## Coding Conventions
- Razor Pages pattern: each page has `.cshtml` + `.cshtml.cs` (PageModel)
- Use EF Core for all DB access — no raw SQL
- Validate all user input using DataAnnotations on model classes
- Keep business logic in PageModel handlers (`OnGet`, `OnPost`, etc.)
- Use `async/await` throughout
- `[TempData]` for cross-redirect success messages

## Key Notes for AI
- Connection string from Neon is in `postgresql://...` URI format — `ParseConnectionString()` in Program.cs converts it to Npgsql ADO.NET format
- App binds to `PORT` env variable (set by Render) via `builder.WebHost.UseUrls()`
- HTTPS redirect is disabled in production (Render handles SSL at edge)
- `FindAsync()` bypasses the global soft-delete query filter — correct behavior for delete handler

## Commands
```bash
# Run the application
dotnet run --project invoice_database

# Add EF migration (use Release to avoid locked exe)
dotnet ef migrations add <MigrationName> --project invoice_database --configuration Release

# Apply migrations locally
dotnet ef database update --project invoice_database

# Build
dotnet build
```

## Planned Improvements
- Repository Pattern + Unit of Work
- DTO Mapping with AutoMapper
- JWT Authentication
- Role-based access (Admin / Staff)
```
