using Servindustria.Models;
using Servindustria.Data.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;

public class AdminCallRequestsModel : PageModel {
    private readonly IUserRepository _userRepository;
    private readonly IAdminCallRequestRepository _callRequestRepository;

    public AdminCallRequestsModel(IUserRepository userRepository, IAdminCallRequestRepository callRequestRepository) {
        _userRepository = userRepository;
        _callRequestRepository = callRequestRepository;
    }

    public IEnumerable<AdminCallRequest> SeenAdminCallRequests { get; set; } = new List<AdminCallRequest>();
    public IEnumerable<AdminCallRequest> UnseenAdminCallRequests { get; set; } = new List<AdminCallRequest>();

    public int TotalUnseenAdminCallRequests { get; set; }
    public int TotalSeenAdminCallRequests { get; set; }
    
    public int UnseenAdminCallRequestsCurrentPage { get; set; }
    public int SeenAdminCallRequestsCurrentPage { get; set; }
    
    public int PageSize { get; set; } = 2;
    
    public async Task<IActionResult> OnGetAsync(int unseenAdminCallRequestsCurrentPage = 1, int seenAdminCallRequestsCurrentPage = 1) {
        if (User.Identity == null || !User.Identity.IsAuthenticated) return RedirectToPage("/Authentication");
        if (!User.IsInRole("Admin")) return RedirectToPage("/Index");
        
        UnseenAdminCallRequestsCurrentPage = unseenAdminCallRequestsCurrentPage;
        SeenAdminCallRequestsCurrentPage = seenAdminCallRequestsCurrentPage;

        // If the page is less than 0, set it to 1
        if (UnseenAdminCallRequestsCurrentPage <= 0) {
            UnseenAdminCallRequestsCurrentPage = 1;
            return RedirectToPage("/AdminCallRequests", new { UnseenAdminCallRequestsCurrentPage, SeenAdminCallRequestsCurrentPage });
        }

        if (SeenAdminCallRequestsCurrentPage <= 0) {
            SeenAdminCallRequestsCurrentPage = 1;
            return RedirectToPage("/AdminCallRequests", new { UnseenAdminCallRequestsCurrentPage, SeenAdminCallRequestsCurrentPage });
        }

        var unseenAdminCallRequests = await _callRequestRepository.GetAdminCallRequestsAsync(UnseenAdminCallRequestsCurrentPage, PageSize, false);
        var seenAdminCallRequests = await _callRequestRepository.GetAdminCallRequestsAsync(SeenAdminCallRequestsCurrentPage, PageSize, true);

        TotalUnseenAdminCallRequests = unseenAdminCallRequests.totalCount;
        TotalSeenAdminCallRequests = seenAdminCallRequests.totalCount;
        SeenAdminCallRequests = seenAdminCallRequests.callRequests;
        UnseenAdminCallRequests = unseenAdminCallRequests.callRequests;

        if (UnseenAdminCallRequestsCurrentPage > (TotalUnseenAdminCallRequests / PageSize + 1)) {
            UnseenAdminCallRequestsCurrentPage = (int)Math.Ceiling((double)TotalUnseenAdminCallRequests / PageSize * 100) / 100;
            return RedirectToPage("/AdminCallRequests", new { UnseenAdminCallRequestsCurrentPage, SeenAdminCallRequestsCurrentPage });
        }

        if (SeenAdminCallRequestsCurrentPage > (TotalSeenAdminCallRequests / PageSize + 1)) {
            SeenAdminCallRequestsCurrentPage = (int)Math.Ceiling((double)TotalSeenAdminCallRequests / PageSize * 100) / 100;
            return RedirectToPage("/AdminCallRequests", new { UnseenAdminCallRequestsCurrentPage, SeenAdminCallRequestsCurrentPage });
        }

        if (UnseenAdminCallRequestsCurrentPage > 1 && UnseenAdminCallRequests.Count() == 0) {
            UnseenAdminCallRequestsCurrentPage--;
            return RedirectToPage("/AdminCallRequests", new { UnseenAdminCallRequestsCurrentPage, SeenAdminCallRequestsCurrentPage });
        }

        if (SeenAdminCallRequestsCurrentPage > 1 && SeenAdminCallRequests.Count() == 0) {
            SeenAdminCallRequestsCurrentPage--;
            return RedirectToPage("/AdminCallRequests", new { UnseenAdminCallRequestsCurrentPage, SeenAdminCallRequestsCurrentPage });
        }
        
        return Page();
}

    public async Task<IActionResult> OnPostSeenUnseenAsync(int id, int UnseenAdminCallRequestsCurrentPage, int SeenAdminCallRequestsCurrentPage) {
        if (User.Identity == null || !User.Identity.IsAuthenticated) return RedirectToPage("/Authentication");
        if (!User.IsInRole("Admin")) return RedirectToPage("/Index");

        await _callRequestRepository.SetSeenUnseenAsync(id);
        return RedirectToPage("/AdminCallRequests");
    }

    public IActionResult OnPostPage(int UnseenAdminCallRequestsCurrentPage, int SeenAdminCallRequestsCurrentPage) {
        if (User.Identity == null || !User.Identity.IsAuthenticated) return RedirectToPage("/Authentication");
        if (!User.IsInRole("Admin")) return RedirectToPage("/Index");

        return RedirectToPage("/AdminCallRequests", new { UnseenAdminCallRequestsCurrentPage, SeenAdminCallRequestsCurrentPage });
    }
}