using invoice_database.Data;
using invoice_database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace invoice_database.Pages.Invoices;

public class DetailsModel : PageModel
{
    private readonly AppDbContext _db;

    public DetailsModel(AppDbContext db) => _db = db;

    public Invoice Invoice { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var invoice = await _db.Invoices
            .Include(i => i.Client)
            .Include(i => i.Services)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (invoice == null)
            return NotFound();

        Invoice = invoice;
        return Page();
    }
}
