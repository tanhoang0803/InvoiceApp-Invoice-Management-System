using invoice_database.Data;
using invoice_database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace invoice_database.Pages.Invoices;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;
    private const int PageSize = 10;

    public IndexModel(AppDbContext db) => _db = db;

    public List<Invoice> Invoices { get; set; } = new();
    public string? Search { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }

    public async Task OnGetAsync(string? search, int page = 1)
    {
        Search = search;
        CurrentPage = page;

        var query = _db.Invoices
            .Include(i => i.Client)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var lower = search.ToLower();
            query = query.Where(i =>
                i.InvoiceNo.ToLower().Contains(lower) ||
                i.Client.Name.ToLower().Contains(lower));
        }

        var total = await query.CountAsync();
        TotalPages = (int)Math.Ceiling(total / (double)PageSize);
        CurrentPage = Math.Max(1, Math.Min(page, TotalPages == 0 ? 1 : TotalPages));

        Invoices = await query
            .OrderByDescending(i => i.CreatedAt)
            .Skip((CurrentPage - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();
    }

    [TempData]
    public string? StatusMessage { get; set; }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var invoice = await _db.Invoices.FindAsync(id);
        if (invoice != null)
        {
            invoice.IsDeleted = true;
            await _db.SaveChangesAsync();
            StatusMessage = $"Invoice {invoice.InvoiceNo} deleted.";
        }
        return RedirectToPage();
    }
}
