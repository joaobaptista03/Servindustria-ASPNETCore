using Servindustria.Models;
using Servindustria.Data.Interfaces;

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
}