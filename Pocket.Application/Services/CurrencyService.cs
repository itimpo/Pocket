using Microsoft.EntityFrameworkCore;
using Pocket.Application.Interfaces;
using Pocket.Domain;
using Pocket.Domain.Entities;
using Pocket.Application.Dto;

namespace Pocket.Application.Services;

internal class CurrencyService : ICurrencyService
{
    private IPocketDbContext _db;
    public CurrencyService(IPocketDbContext db)
    {
        _db = db;
    }

    public IQueryable<CurrencyDto> GetCurrencies()
    {
        return _db.Currencies
            .OrderBy(e => e.Code)
            .Select(q => new CurrencyDto
            {
                Id = q.Id,
                Code = q.Code
            });
    }

    public async Task<Result> SaveCurrency(CurrencyDto dto)
    {
        //check if currency exists or create
        var currency = await _db.Currencies.FirstOrDefaultAsync(q => q.Id == dto.Id);
        if (currency == null)
        {
            currency = new Currency();
            _db.Currencies.Add(currency);
        }

        currency.Code = dto.Code;

        try
        {
            await _db.SaveChangesAsync();

            return new Result { Success = true };
        }
        catch (Exception ex)
        {
            return new Result { Error = ex.ToString() };
        }
    }

    public async Task<Result> DeleteCurrency(int id)
    {
        var currency = _db.Currencies.FirstOrDefault(q => q.Id == id);
        if (currency == null)
        {
            return new Result { Error = "Currency not found" };
        }

        try
        {
            _db.Currencies.Remove(currency);

            await _db.SaveChangesAsync();

            return new Result { Success = true };
        }
        catch (Exception ex)
        {
            return new Result { Error = ex.ToString() };
        }
    }
}
