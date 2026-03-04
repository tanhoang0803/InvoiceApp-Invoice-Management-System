using invoice_database.Data;
using invoice_database.Models;
using invoice_database.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace invoice_database.Pages.Invoices;

public class CreateModel : PageModel
{
    private readonly AppDbContext _db;

    public CreateModel(AppDbContext db) => _db = db;

    [BindProperty]
    public InvoiceFormViewModel Input { get; set; } = new();

    [TempData]
    public string? StatusMessage { get; set; }

    public void OnGet()
    {
        Input.IssueDate = DateTime.Today;
        Input.DueDate = DateTime.Today.AddDays(30);
        Input.PaymentStatus = PaymentStatus.Pending;
        Input.Services.Add(new ServiceRowViewModel());
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var client = new Client
        {
            Name = Input.ClientName,
            Email = Input.ClientEmail,
            Phone = Input.ClientPhone,
            Address = Input.ClientAddress
        };
        _db.Clients.Add(client);
        await _db.SaveChangesAsync();

        var invoice = new Invoice
        {
            InvoiceNo = Input.InvoiceNo,
            PaymentStatus = Input.PaymentStatus,
            IssueDate = Input.IssueDate,
            DueDate = Input.DueDate,
            ClientId = client.Id,
            CreatedAt = DateTime.UtcNow
        };

        foreach (var row in Input.Services)
        {
            row.TotalPrice = row.Quantity * row.UnitPrice;
            invoice.Services.Add(new InvoiceService
            {
                ServiceName = row.ServiceName,
                Quantity = row.Quantity,
                UnitPrice = row.UnitPrice,
                TotalPrice = row.TotalPrice
            });
        }

        invoice.TotalAmount = invoice.Services.Sum(s => s.TotalPrice);

        _db.Invoices.Add(invoice);

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("unique") == true ||
                                            ex.InnerException?.Message.Contains("duplicate") == true ||
                                            ex.InnerException?.Message.Contains("23505") == true)
        {
            ModelState.AddModelError("Input.InvoiceNo", "Invoice number already exists. Please use a unique invoice number.");
            return Page();
        }

        StatusMessage = $"Invoice {invoice.InvoiceNo} created successfully.";
        return RedirectToPage("Index");
    }
}
