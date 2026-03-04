# InvoiceApp – CLAUDE.md

## Project Overview
Invoice Management System built with ASP.NET Core Razor Pages (.NET 10) and SQL Server.
Supports full invoice lifecycle: create, edit, delete, paginate, search, and track payment status.

## Tech Stack
- **Framework**: ASP.NET Core Razor Pages (.NET 10)
- **ORM**: Entity Framework Core
- **Database**: SQL Server
- **UI**: Razor Pages + Bootstrap
- **Solution file**: `invoice_database.slnx`
- **Project folder**: `invoice_database/`

## Project Structure
```
invoice_App/
├── invoice_database/           # Main project
│   ├── Data/
│   │   └── AppDbContext.cs
│   ├── Models/
│   │   ├── Invoice.cs
│   │   ├── Client.cs
│   │   └── InvoiceService.cs
│   ├── Pages/
│   │   ├── Invoices/
│   │   │   ├── Index.cshtml        # Invoice list + search + pagination
│   │   │   ├── Create.cshtml       # Create invoice form
│   │   │   └── Edit.cshtml         # Edit invoice form
│   │   └── Shared/
│   ├── Program.cs
│   ├── appsettings.json
│   └── invoice_database.csproj
└── invoice_database.slnx
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
```

## Key Business Rules
- `InvoiceService.TotalPrice = Quantity * UnitPrice`
- `Invoice.TotalAmount = SUM(all InvoiceService.TotalPrice)`
- PaymentStatus values: `Paid` / `Pending`
- Deleting an invoice must cascade-delete its InvoiceServices
- InvoiceNo must be unique

## Coding Conventions
- Razor Pages pattern: each page has `.cshtml` + `.cshtml.cs` (PageModel)
- Use EF Core for all DB access — no raw SQL
- Validate all user input using DataAnnotations on model classes
- Keep business logic in PageModel handlers (`OnGet`, `OnPost`, etc.)
- Use `async/await` throughout

## Commands
```bash
# Run the application
dotnet run --project invoice_database

# Add EF migration
dotnet ef migrations add <MigrationName> --project invoice_database

# Apply migrations
dotnet ef database update --project invoice_database

# Build
dotnet build
```

## Planned Improvements
- Repository Pattern + Unit of Work
- DTO Mapping with AutoMapper
- JWT Authentication
- Role-based access (Admin / Staff)
- AJAX dynamic service rows with real-time total
- Soft delete instead of hard delete
- Modal confirmation on delete
- Toast notifications
