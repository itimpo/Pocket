using System.ComponentModel.DataAnnotations;

namespace Pocket.Application.Dto;

public record TransactionGroupDto
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
}


