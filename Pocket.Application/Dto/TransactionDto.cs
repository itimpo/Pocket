using System.ComponentModel.DataAnnotations;

namespace Pocket.Application.Dto;

public record TransactionDto
{
    public Guid? Id { get; set; }
    [Required]
    public decimal Amount { get; set; }
    [Required, StringLength(3, MinimumLength = 3)]
    public string Currency { get; set; } = string.Empty;
    [Required, StringLength(100)]
    public string TransactionGroup { get; set; } = string.Empty;
    [Required, StringLength(250)]
    public string Description { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
}


