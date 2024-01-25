using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pocket.Application;
using Pocket.Application.Dto;
using Pocket.Application.Interfaces;
using System.Net;

namespace Pocket.Api.Controllers;

/// <summary>
/// Currencies Controller
/// </summary>
[ApiController]
[Route("[controller]"), Authorize]
public class CurrenciesController(ICurrencyService currencyRepository) : ControllerBase
{

    /// <summary>
    /// Gets currencies list
    /// </summary>
    /// <response code="200">Successful operation</response>
    [ProducesResponseType(typeof(CurrencyDto[]), (int)HttpStatusCode.OK)]
    [HttpGet("")]
    public IActionResult Currencies()
    {
        return Ok(currencyRepository.GetCurrencies());
    }

    /// <summary>
    /// Gets currency by id
    /// </summary>
    /// <param name="id">Currency Id</param>
    /// <response code="200">Successful operation</response>
    [ProducesResponseType(typeof(CurrencyDto[]), (int)HttpStatusCode.OK)]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Currency(int id)
    {
        return Ok(await currencyRepository.GetCurrencies().FirstOrDefaultAsync(q => q.Id == id));
    }

    /// <summary>
    /// Creates new currency
    /// </summary>
    /// <response code="200">Successful operation</response>
    /// <response code="400">Invalid post model</response>
    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [HttpPost("")]
    public async Task<IActionResult> CurrencyPost([FromBody] CurrencyDto dto)
    {
        if (dto is null)
        {
            return BadRequest(new Result { Error = "Currency model is empty" });
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await currencyRepository.SaveCurrency(dto);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Updates currency
    /// </summary>
    /// <param name="id">Currency Id</param>
    /// <param name="dto">Currency object</param>
    /// <response code="200">Successful operation</response>
    /// <response code="400">Invalid post model</response>
    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> CurrencyPut(int id, [FromBody] CurrencyDto dto)
    {
        if (dto is null)
        {
            return BadRequest(new Result { Error = "Currency model is empty" });
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        dto.Id = id;

        var result = await currencyRepository.SaveCurrency(dto);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Delete currency by id
    /// </summary>
    /// <param name="id">Currency Id</param>
    /// <response code="200">Successful operation</response>
    [ProducesResponseType(typeof(CurrencyDto[]), (int)HttpStatusCode.OK)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> CurrencyDelete(int id)
    {
        return Ok(await currencyRepository.DeleteCurrency(id));
    }
}