namespace Pocket.Api.Models;

/// <summary>
/// User Login Model
/// </summary>
public record UserLogin
{
    /// <summary>
    /// User Email
    /// </summary>
    public required string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// User Password
    /// </summary>
    public required string Password { get; set; } = string.Empty;
}

/// <summary>
/// User Token Model
/// </summary>
public record UserToken
{
    /// <summary>
    /// User Token
    /// </summary>
    public required string Token { get; set; } = string.Empty;
}
