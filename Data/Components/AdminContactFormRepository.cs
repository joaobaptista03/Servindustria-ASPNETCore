using Servindustria.Models;
using Servindustria.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Servindustria.Data.Components;

public class AdminContactFormRepository : IAdminContactFormRepository {
    private readonly ServindustriaDBContext _context;

    public AdminContactFormRepository(ServindustriaDBContext context) {
        _context = context;
    }

    public async Task AddAdminContactFormAsync(AdminContactForm contactForm) {
        await _context.ContactForms.AddAsync(contactForm);
        await _context.SaveChangesAsync();
    }

    public async Task SetSeenUnseenAsync(int id) {
        var contactForm = await _context.ContactForms.FindAsync(id);
        if (contactForm != null) contactForm.Seen = !contactForm.Seen;
        await _context.SaveChangesAsync();
    }

    public async Task<(IEnumerable<AdminContactForm> contactForms, int totalCount)> GetAdminContactFormsAsync(int currentPage, int pageSize, bool seen) {
        var contactForms = _context.ContactForms.Where(cf => cf.Seen == seen)
            .OrderByDescending(cf => cf.Date)
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize);
            
        int totalCount = _context.ContactForms.Count(cf => cf.Seen == seen);
        
        return (await contactForms.ToListAsync(), totalCount);
    }

    public async Task<int> GetTotalUnseenAdminContactFormsAsync() {
        return await _context.ContactForms.CountAsync(cf => !cf.Seen);
    }
}