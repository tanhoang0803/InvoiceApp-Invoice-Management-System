using invoice_database.Data;
using invoice_database.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace invoice_database.Pages;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;

    public IndexModel(AppDbContext db) => _db = db;

    public int TotalInvoices { get; set; }
    public int PaidCount { get; set; }
    public int PendingCount { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal PendingRevenue { get; set; }
    public List<Invoice> RecentInvoices { get; set; } = new();

    public async Task OnGetAsync()
    {
        TotalInvoices = await _db.Invoices.CountAsync();
        PaidCount = await _db.Invoices.CountAsync(i => i.PaymentStatus == PaymentStatus.Paid);
        PendingCount = await _db.Invoices.CountAsync(i => i.PaymentStatus == PaymentStatus.Pending);
        TotalRevenue = await _db.Invoices.Where(i => i.PaymentStatus == PaymentStatus.Paid).SumAsync(i => (decimal?)i.TotalAmount) ?? 0;
        PendingRevenue = await _db.Invoices.Where(i => i.PaymentStatus == PaymentStatus.Pending).SumAsync(i => (decimal?)i.TotalAmount) ?? 0;

        RecentInvoices = await _db.Invoices
            .Include(i => i.Client)
            .OrderByDescending(i => i.CreatedAt)
            .Take(5)
            .ToListAsync();
    }
}
