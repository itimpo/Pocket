using Microsoft.EntityFrameworkCore;
using Pocket.Application.Dto;
using Pocket.Application.Interfaces;
using Pocket.Domain;

namespace Pocket.Application.Services;

internal class UserService : IUserService
{
    private IPocketDbContext _db;
    public UserService(IPocketDbContext db)
    {
        _db = db;
    }

    public Task<UserDto?> GetUser(string email, string password)
    {
        return _db.Users
            .Where(q => q.Email == email && q.PasswordHash == password) //password hash :)
            .Select(q => new UserDto
            {
                Id = q.Id,
                Name = q.Name,
                Email = email
            })
            .FirstOrDefaultAsync();
    }
}
