# InvoiceApp – Invoice Management System

A web-based Invoice Management System built with **ASP.NET Core Razor Pages (.NET 10)** and **SQL Server**. Supports full invoice lifecycle management including multi-service billing, client tracking, and payment status.

## Features

### Invoice List (Dashboard)
- Paginated data table of all invoices
- Search by client name or invoice number
- Status badge: `Paid` / `Pending`
- Edit and delete actions

### Create Invoice
- Invoice details: Invoice No, Payment Status, Issue Date, Due Date
- Client details: Name, Email, Phone, Address
- Service rows: Service Name, Quantity, Unit Price — auto-calculates Total Price

### Edit Invoice
- Load and update all invoice, client, and service data
- Add or remove service rows dynamically
- Recalculates invoice total on save

## Business Logic
```
Service Total  = Quantity × Unit Price
Invoice Total  = SUM(all Service Totals)
```

## Database Schema

| Table | Key Columns |
|---|---|
| `Invoices` | Id, InvoiceNo, PaymentStatus, IssueDate, DueDate, ClientId (FK), TotalAmount, CreatedAt |
| `Clients` | Id, Name, Email, Phone, Address |
| `InvoiceServices` | Id, InvoiceId (FK), ServiceName, Quantity, UnitPrice, TotalPrice |

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core Razor Pages (.NET 10) |
| ORM | Entity Framework Core |
| Database | SQL Server |
| UI | Bootstrap 5 |

## Getting Started

### Prerequisites
- .NET 10 SDK
- SQL Server (local or remote)
- EF Core CLI: `dotnet tool install --global dotnet-ef`

### Setup
```bash
# 1. Clone the repo
git clone <repo-url>
cd invoice_App

# 2. Configure connection string in invoice_database/appsettings.json
# "ConnectionStrings": { "DefaultConnection": "Server=...;Database=InvoiceDb;..." }

# 3. Apply migrations
dotnet ef database update --project invoice_database

# 4. Run
dotnet run --project invoice_database
```

## Project Structure
```
invoice_App/
├── invoice_database/
│   ├── Data/               # AppDbContext
│   ├── Models/             # Invoice, Client, InvoiceService
│   ├── Pages/
│   │   └── Invoices/       # Index, Create, Edit Razor Pages
│   ├── Program.cs
│   └── appsettings.json
└── invoice_database.slnx
```

## Security
- EF Core ORM prevents SQL injection
- Input validation via DataAnnotations
- CSRF protection built into Razor Pages (AntiForgery tokens)
- HTTPS enforced

## Complexity Level
This project demonstrates: CRUD mastery, relational DB design, business logic handling, MVC/Razor Pages architecture, and UI + backend integration — targeting **Fresher to Junior** level.
