using Pocket.Application.Dto;

namespace Pocket.Application.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetUser(string email, string password);
}