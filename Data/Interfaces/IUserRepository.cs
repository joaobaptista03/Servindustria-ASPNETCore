using Servindustria.Models;

namespace Servindustria.Data.Interfaces;
public interface IUserRepository {
    Task<bool> LoginAsync(string email, string password);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> NifExistsAsync(int nif);
    void AddUser(User user);
}