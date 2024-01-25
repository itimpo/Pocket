using Pocket.Application.Dto;

namespace Pocket.Application.Interfaces;

public interface ICurrencyService
{
    IQueryable<CurrencyDto> GetCurrencies();
    Task<Result> SaveCurrency(CurrencyDto dto);
    Task<Result> DeleteCurrency(int id);
}