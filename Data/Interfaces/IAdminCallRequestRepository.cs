using Servindustria.Models;

namespace Servindustria.Data.Interfaces;
public interface IAdminCallRequestRepository {
    Task AddAdminCallRequestAsync(AdminCallRequest callRequest);
    Task SetSeenUnseenAsync(int id);
    Task<(IEnumerable<AdminCallRequest> callRequests, int totalCount)> GetAdminCallRequestsAsync(int currentPage, int pageSize, bool seen);
    Task<int> GetTotalUnseenAdminCallRequestsAsync();
}