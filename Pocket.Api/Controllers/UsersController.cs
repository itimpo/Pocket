using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Pocket.Api.Models;
using Pocket.Application;
using Pocket.Application.Interfaces;

namespace Pocket.Api.Controllers;

/// <summary>
/// Users Controller
/// </summary>
[ApiController]
[Route("[controller]"), AllowAnonymous]
public class UsersController(
    IUserService userRepository,
    IConfiguration configuration) : ControllerBase
{

    /// <summary>
    /// User Login using email and password
    /// </summary>
    /// <response code="200">Successful operation</response>
    /// <response code="400">Invalid post model</response>
    /// <response code="401">User not found</response>
    [ProducesResponseType(typeof(UserToken), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] UserLogin model)
    {
        if (model is null)
        {
            return BadRequest(new Result { Error = "Invalid user request!!!" });
        }
        var user = await userRepository.GetUser(model.Email, model.Password);
        if (user != null)
        {
            var secretString = configuration["JWT:Secret"];
            if (string.IsNullOrEmpty(secretString))
            {
                throw new Exception("JWT:Secret is not configured");
            }
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretString));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim> {
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Sid, user.Id.ToString()),
            };
            var tokenOptions = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddDays(6),
                signingCredentials: signinCredentials);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return Ok(new UserToken
            {
                Token = tokenString
            });
        }
        return Unauthorized();
    }
}