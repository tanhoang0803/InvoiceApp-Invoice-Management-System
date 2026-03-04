using invoice_database.Data;
using invoice_database.Models;
using invoice_database.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace invoice_database.Pages.Invoices;

public class EditModel : PageModel
{
    private readonly AppDbContext _db;

    public EditModel(AppDbContext db) => _db = db;

    [BindProperty]
    public InvoiceFormViewModel Input { get; set; } = new();

    [TempData]
    public string? StatusMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var invoice = await _db.Invoices
            .Include(i => i.Client)
            .Include(i => i.Services)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (invoice == null)
            return NotFound();

        Input = new InvoiceFormViewModel
        {
            InvoiceNo = invoice.InvoiceNo,
            PaymentStatus = invoice.PaymentStatus,
            IssueDate = invoice.IssueDate,
            DueDate = invoice.DueDate,
            ClientName = invoice.Client.Name,
            ClientEmail = invoice.Client.Email,
            ClientPhone = invoice.Client.Phone,
            ClientAddress = invoice.Client.Address,
            Services = invoice.Services.Select(s => new ServiceRowViewModel
            {
                Id = s.Id,
                ServiceName = s.ServiceName,
                Quantity = s.Quantity,
                UnitPrice = s.UnitPrice,
                TotalPrice = s.TotalPrice
            }).ToList()
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid)
            return Page();

        var invoice = await _db.Invoices
            .Include(i => i.Client)
            .Include(i => i.Services)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (invoice == null)
            return NotFound();

        // Update invoice fields
        invoice.InvoiceNo = Input.InvoiceNo;
        invoice.PaymentStatus = Input.PaymentStatus;
        invoice.IssueDate = Input.IssueDate;
        invoice.DueDate = Input.DueDate;

        // Update client
        invoice.Client.Name = Input.ClientName;
        invoice.Client.Email = Input.ClientEmail;
        invoice.Client.Phone = Input.ClientPhone;
        invoice.Client.Address = Input.ClientAddress;

        // Replace services
        _db.InvoiceServices.RemoveRange(invoice.Services);
        invoice.Services.Clear();

        foreach (var row in Input.Services)
        {
            var service = new InvoiceService
            {
                ServiceName = row.ServiceName,
                Quantity = row.Quantity,
                UnitPrice = row.UnitPrice,
                TotalPrice = row.Quantity * row.UnitPrice
            };
            invoice.Services.Add(service);
        }

        invoice.TotalAmount = invoice.Services.Sum(s => s.TotalPrice);

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

        StatusMessage = $"Invoice {invoice.InvoiceNo} updated successfully.";
        return RedirectToPage("Index");
    }
}
