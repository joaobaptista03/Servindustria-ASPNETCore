using BCrypt.Net;
using Servindustria.Models;
using Servindustria.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Servindustria.Data.Components;

public class UserRepository : IUserRepository {
    private readonly ServindustriaDBContext _context;
    private IEnumerable<User> _Users = new List<User>();

    public UserRepository(ServindustriaDBContext context) {
        _context = context;
    }
    
    public async Task<bool> EmailExistsAsync(string email)
        => (await _context.Users.FirstOrDefaultAsync(u => u.Email == email)) != null;

    public async Task<bool> NifExistsAsync(int nif)
        => (await _context.Users.FirstOrDefaultAsync(u => u.Nif == nif)) != null;

    public async Task<bool> LoginAsync(string email, string password) {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null) return false;
        return BCrypt.Net.BCrypt.Verify(password, user.Password);
    }

    public async void AddUser(User user) {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}