using Servindustria.Models;

namespace Servindustria.Data.Interfaces;
public interface ICallRequestRepository {
    Task AddCallRequestAsync(CallRequest callRequest);
}