using Pocket.Application.Dto;

namespace Pocket.Application.Interfaces;

public interface ITransactionService
{
    IQueryable<TransactionDto> GetTransactions(string? currency = null);
    IQueryable<TransactionGroupDto> GetTransactionGroups();
    Task<Result> SaveTransaction(TransactionDto dto);
    Task<Result> DeleteTransaction(Guid gid);
}