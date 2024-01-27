using Servindustria.Models;

namespace Servindustria.Data.Interfaces;
public interface IUserRepository {
    Task<bool> LoginAsync(string email, string password);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> NifExistsAsync(int nif);
    Task AddUserAsync(User user);
    Task<bool> IsAdmin(string email);
}