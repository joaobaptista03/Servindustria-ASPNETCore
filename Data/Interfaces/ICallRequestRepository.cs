using Servindustria.Models;

namespace Servindustria.Data.Interfaces;
public interface ICallRequestRepository {
    Task AddCallRequestAsync(CallRequest callRequest);
    Task SetSeenUnseenAsync(int id);
    Task<(IEnumerable<CallRequest> callRequests, int totalCount)> GetCallRequestsAsync(int currentPage, int pageSize, bool seen);
}