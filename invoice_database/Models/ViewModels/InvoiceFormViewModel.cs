using System.ComponentModel.DataAnnotations;
using invoice_database.Models;

namespace invoice_database.Models.ViewModels;

public class InvoiceFormViewModel
{
    [Required]
    [Display(Name = "Invoice No")]
    public string InvoiceNo { get; set; } = string.Empty;

    [Display(Name = "Payment Status")]
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

    [Display(Name = "Issue Date")]
    [DataType(DataType.Date)]
    public DateTime IssueDate { get; set; } = DateTime.Today;

    [Display(Name = "Due Date")]
    [DataType(DataType.Date)]
    public DateTime DueDate { get; set; } = DateTime.Today.AddDays(30);

    // Client fields
    [Required]
    [Display(Name = "Client Name")]
    public string ClientName { get; set; } = string.Empty;

    [Display(Name = "Client Email")]
    [EmailAddress]
    public string? ClientEmail { get; set; }

    [Display(Name = "Client Phone")]
    public string? ClientPhone { get; set; }

    [Display(Name = "Client Address")]
    public string? ClientAddress { get; set; }

    // Services
    public List<ServiceRowViewModel> Services { get; set; } = new List<ServiceRowViewModel>();
}
