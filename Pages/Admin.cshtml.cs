using Servindustria.Models;
using Servindustria.Data.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;

public class AdminModel : PageModel {
    private readonly IUserRepository _userRepository;
    private readonly ICallRequestRepository _callRequestRepository;

    public AdminModel(IUserRepository userRepository, ICallRequestRepository callRequestRepository) {
        _userRepository = userRepository;
        _callRequestRepository = callRequestRepository;
    }

    public IEnumerable<CallRequest> SeenCallRequests { get; set; } = new List<CallRequest>();
    public IEnumerable<CallRequest> UnseenCallRequests { get; set; } = new List<CallRequest>();

    public int TotalUnseenCallRequests { get; set; }
    public int TotalSeenCallRequests { get; set; }
    
    public int UnseenCallRequestsCurrentPage { get; set; }
    public int SeenCallRequestsCurrentPage { get; set; }
    
    public int PageSize { get; set; } = 2;
    
    public async Task<IActionResult> OnGetAsync(int unseenCallRequestsCurrentPage = 1, int seenCallRequestsCurrentPage = 1) {
        if (User.Identity == null || !User.Identity.IsAuthenticated) return RedirectToPage("/Authentication");
        if (!User.IsInRole("Admin")) return RedirectToPage("/Index");
        
        UnseenCallRequestsCurrentPage = unseenCallRequestsCurrentPage;
        SeenCallRequestsCurrentPage = seenCallRequestsCurrentPage;

        var unseenCallRequests = await _callRequestRepository.GetCallRequestsAsync(UnseenCallRequestsCurrentPage, PageSize, false);
        var seenCallRequests = await _callRequestRepository.GetCallRequestsAsync(SeenCallRequestsCurrentPage, PageSize, true);

        TotalUnseenCallRequests = unseenCallRequests.totalCount;
        TotalSeenCallRequests = seenCallRequests.totalCount;
        SeenCallRequests = seenCallRequests.callRequests;
        UnseenCallRequests = unseenCallRequests.callRequests;
        
        return Page();
}

    public async Task<IActionResult> OnPostSeenUnseenAsync(int id) {
        if (User.Identity == null || !User.Identity.IsAuthenticated) return RedirectToPage("/Authentication");
        if (!User.IsInRole("Admin")) return RedirectToPage("/Index");

        await _callRequestRepository.SetSeenUnseenAsync(id);
        return RedirectToPage("/Admin");
    }

    public IActionResult OnPostPage(int SeenCallRequestsCurrentPage, int UnseenCallRequestsCurrentPage) {
        if (User.Identity == null || !User.Identity.IsAuthenticated) return RedirectToPage("/Authentication");
        if (!User.IsInRole("Admin")) return RedirectToPage("/Index");

        return RedirectToPage("/Admin", new { UnseenCallRequestsCurrentPage, SeenCallRequestsCurrentPage });
    }
}