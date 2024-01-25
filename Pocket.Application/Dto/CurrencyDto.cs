namespace Pocket.Application.Dto;

public record CurrencyDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
}
