using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pocket.Application;
using Pocket.Application.Dto;
using Pocket.Application.Interfaces;
using System.Net;

namespace Pocket.Api.Controllers;

/// <summary>
/// Transactions Controller
/// </summary>
[ApiController]
[Route("[controller]"), Authorize]
public class TransactionsController(ITransactionService transactionRepository) : ControllerBase
{

    /// <summary>
    /// Gets transactions list
    /// </summary>
    /// <response code="200">Successful operation</response>
    [ProducesResponseType(typeof(TransactionDto[]), (int)HttpStatusCode.OK)]
    [HttpGet("")]
    public IActionResult Transactions(string currency, string? search = null)
    {
        var transactions = transactionRepository.GetTransactions(currency);
        if (!string.IsNullOrEmpty(search))
        {
            transactions = transactions
                .Where(q => q.Description.Contains(search) || q.TransactionGroup.Contains(search));
        }

        return Ok(transactions);
    }

    /// <summary>
    /// Gets transaction group list
    /// </summary>
    /// <response code="200">Successful operation</response>
    [ProducesResponseType(typeof(TransactionGroupDto[]), (int)HttpStatusCode.OK)]
    [HttpGet("groups")]
    public IActionResult TransactionGroups()
    {
        return Ok(transactionRepository.GetTransactionGroups());
    }

    /// <summary>
    /// Gets transaction by id
    /// </summary>
    /// <response code="200">Successful operation</response>
    [ProducesResponseType(typeof(TransactionDto[]), (int)HttpStatusCode.OK)]
    [HttpGet("{transactionId:Guid}")]
    public async Task<IActionResult> Transaction(Guid transactionId)
    {
        return Ok(await transactionRepository.GetTransactions().FirstOrDefaultAsync(q => q.Id == transactionId));
    }

    /// <summary>
    /// Creates new transaction
    /// </summary>
    /// <response code="200">Successful operation</response>
    /// <response code="400">Invalid post model</response>
    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [HttpPost("")]
    public async Task<IActionResult> TransactionPost([FromBody] TransactionDto dto)
    {
        if (dto is null)
        {
            return BadRequest(new Result { Error = "Transaction model is empty" });
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await transactionRepository.SaveTransaction(dto);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Updates transaction
    /// </summary>
    /// <param name="transactionId">Transaction Id</param>
    /// <param name="dto">Transaction object</param>
    /// <response code="200">Successful operation</response>
    /// <response code="400">Invalid post model</response>
    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [HttpPut("{transactionId:Guid}")]
    public async Task<IActionResult> TransactionPut(Guid transactionId, [FromBody] TransactionDto dto)
    {
        if (dto is null)
        {
            return BadRequest(new Result { Error = "Event model is empty" });
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        dto.Id = transactionId;

        var result = await transactionRepository.SaveTransaction(dto);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Delete transaction by id
    /// </summary>
    /// <param name="transactionId">Transaction Id</param>
    /// <response code="200">Successful operation</response>
    [ProducesResponseType(typeof(TransactionDto[]), (int)HttpStatusCode.OK)]
    [HttpDelete("{transactionId:Guid}")]
    public async Task<IActionResult> TransactionDelete(Guid transactionId)
    {
        return Ok(await transactionRepository.DeleteTransaction(transactionId));
    }
}