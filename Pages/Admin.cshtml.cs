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
    
    public async Task<IActionResult> OnGetAsync() {
        if (User.Identity == null || !User.Identity.IsAuthenticated) return RedirectToPage("/Authentication");
        if (!User.IsInRole("Admin")) return RedirectToPage("/Index");

        var allCallRequests = await _callRequestRepository.GetAllAsync();
        SeenCallRequests = allCallRequests.Where(cr => cr.Seen);
        UnseenCallRequests = allCallRequests.Where(cr => !cr.Seen);
        return Page();
    }

    public async Task<IActionResult> OnPostSeenUnseenAsync(int id) {
        if (User.Identity == null || !User.Identity.IsAuthenticated) return RedirectToPage("/Authentication");
        if (!User.IsInRole("Admin")) return RedirectToPage("/Index");

        await _callRequestRepository.SetSeenUnseenAsync(id);
        return RedirectToPage("/Admin");
    }
}