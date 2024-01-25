using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pocket.Application.Dto;
using Pocket.Application.Interfaces;
using Pocket.Domain;
using Pocket.Domain.Entities;

namespace Pocket.Application.Services;

internal class TransactionService : ITransactionService
{
    private IPocketDbContext _db;
    private UserContext _user;
    private ILogger<TransactionService> _logger;

    public TransactionService(IPocketDbContext db, UserContext user, ILogger<TransactionService> logger)
    {
        _db = db;
        _user = user;
        _logger = logger;
    }

    public IQueryable<TransactionDto> GetTransactions(string? currency = null)
    {
        return _db.Transactions
            .Where(q => q.UserId == _user.Id && (string.IsNullOrEmpty(currency) || q.Currency.Code == currency))
            .OrderByDescending(e => e.TransactionDate)
            .Select(q => new TransactionDto
            {
                Id = q.Id,
                Amount = q.Amount,
                Currency = q.Currency.Code,
                TransactionGroup = q.TransactionGroup.Name,
                Description = q.Description,
                TransactionDate = q.TransactionDate
            });
    }


    public IQueryable<TransactionGroupDto> GetTransactionGroups()
    {
        return _db.TransactionGroups
            .OrderBy(e => e.Name)
            .Select(q => new TransactionGroupDto
            {
                Id = q.Id,
                Name = q.Name
            });
    }


    public async Task<Result> SaveTransaction(TransactionDto dto)
    {
        //check if transaction exists or create
        var transaction = await _db.Transactions
            .Where(q => q.UserId == _user.Id)
            .FirstOrDefaultAsync(q => q.Id == dto.Id);

        if (transaction == null)
        {
            transaction = new Transaction
            {
                UserId = _user.Id
            };
            _db.Transactions.Add(transaction);
        }

        try
        {
            transaction.Amount = dto.Amount;
            transaction.Currency = _db.Currencies.First(q => q.Code == dto.Currency);
            transaction.TransactionGroup = await GetOrCreateTransactionGroup(dto.TransactionGroup);
            transaction.Description = dto.Description;
            transaction.TransactionDate = dto.TransactionDate;

            await _db.SaveChangesAsync();

            return new Result { Success = true };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving transaction");
            return new Result { Error = ex.ToString() };
        }
    }

    public async Task<Result> DeleteTransaction(Guid gid)
    {
        var transaction = _db.Transactions
            .Where(q => q.UserId == _user.Id)
            .FirstOrDefault(q => q.Id == gid);

        if (transaction == null)
        {
            return new Result { Error = "Transaction not found" };
        }

        _db.Transactions.Remove(transaction);
        try
        {
            await _db.SaveChangesAsync();

            return new Result { Success = true };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting transaction");
            return new Result { Error = ex.ToString() };
        }
    }

    public async Task<TransactionGroup> GetOrCreateTransactionGroup(string tgroup)
    {
        //check if transaction exists or create
        var group = await _db.TransactionGroups
            .FirstOrDefaultAsync(q => q.Name.Equals(tgroup, StringComparison.OrdinalIgnoreCase));

        if (group == null)
        {
            group = new TransactionGroup();
            _db.TransactionGroups.Add(group);
        }

        group.Name = tgroup;

        await _db.SaveChangesAsync();

        return group;
    }
}
