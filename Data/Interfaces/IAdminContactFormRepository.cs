using Servindustria.Models;

namespace Servindustria.Data.Interfaces;
public interface IAdminContactFormRepository {
    Task AddAdminContactFormAsync(AdminContactForm contactForm);
    Task SetSeenUnseenAsync(int id);
    Task<(IEnumerable<AdminContactForm> contactForms, int totalCount)> GetAdminContactFormsAsync(int currentPage, int pageSize, bool seen);
}