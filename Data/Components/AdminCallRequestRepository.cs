using Servindustria.Models;
using Servindustria.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Servindustria.Data.Components;

public class AdminCallRequestRepository : IAdminCallRequestRepository {
    private readonly ServindustriaDBContext _context;

    public AdminCallRequestRepository(ServindustriaDBContext context) {
        _context = context;
    }

    public async Task AddAdminCallRequestAsync(AdminCallRequest callRequest) {
        await _context.AdminCallRequests.AddAsync(callRequest);
        await _context.SaveChangesAsync();
    }

    public async Task SetSeenUnseenAsync(int id) {
        var callRequest = await _context.AdminCallRequests.FindAsync(id);
        if (callRequest != null) callRequest.Seen = !callRequest.Seen;
        await _context.SaveChangesAsync();
    }

    public async Task<(IEnumerable<AdminCallRequest> callRequests, int totalCount)> GetAdminCallRequestsAsync(int currentPage, int pageSize, bool seen) {
        var callRequests = _context.AdminCallRequests.Where(cr => cr.Seen == seen)
            .OrderByDescending(cr => cr.Date)
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize);
            
        int totalCount = _context.AdminCallRequests.Count(cr => cr.Seen == seen);
        
        return (await callRequests.ToListAsync(), totalCount);
    }

    public async Task<int> GetTotalUnseenAdminCallRequestsAsync() {
        return await _context.AdminCallRequests.CountAsync(cr => !cr.Seen);
    }
}