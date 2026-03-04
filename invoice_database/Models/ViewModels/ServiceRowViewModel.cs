using System.ComponentModel.DataAnnotations;

namespace invoice_database.Models.ViewModels;

public class ServiceRowViewModel
{
    public int? Id { get; set; }

    [Required]
    public string ServiceName { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; } = 1;

    [Range(0, double.MaxValue)]
    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }
}
