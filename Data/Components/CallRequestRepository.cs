using Servindustria.Models;
using Servindustria.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Servindustria.Data.Components;

public class CallRequestRepository : ICallRequestRepository {
    private readonly ServindustriaDBContext _context;

    public CallRequestRepository(ServindustriaDBContext context) {
        _context = context;
    }

    public async Task AddCallRequestAsync(CallRequest callRequest) {
        await _context.CallRequests.AddAsync(callRequest);
        await _context.SaveChangesAsync();
    }

    public async Task SetSeenUnseenAsync(int id) {
        var callRequest = await _context.CallRequests.FindAsync(id);
        if (callRequest != null) callRequest.Seen = !callRequest.Seen;
        await _context.SaveChangesAsync();
    }

    public async Task<(IEnumerable<CallRequest> callRequests, int totalCount)> GetCallRequestsAsync(int currentPage, int pageSize, bool seen) {
        var callRequests = _context.CallRequests.Where(cr => cr.Seen == seen)
            .OrderByDescending(cr => cr.Date)
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize);
            
        int totalCount = _context.CallRequests.Count(cr => cr.Seen == seen);
        
        return (await callRequests.ToListAsync(), totalCount);
    }
}