using Servindustria.Models;
using Servindustria.Data.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;

namespace Servindustria.Pages;

public class AdminContactFormsModel : PageModel {
    private readonly IUserRepository _userRepository;
    private readonly IAdminContactFormRepository _contactFormRepository;

    public AdminContactFormsModel(IUserRepository userRepository, IAdminContactFormRepository contactFormRepository) {
        _userRepository = userRepository;
        _contactFormRepository = contactFormRepository;
    }

    public IEnumerable<AdminContactForm> SeenAdminContactForms { get; set; } = new List<AdminContactForm>();
    public IEnumerable<AdminContactForm> UnseenAdminContactForms { get; set; } = new List<AdminContactForm>();

    public int TotalUnseenAdminContactForms { get; set; }
    public int TotalSeenAdminContactForms { get; set; }
    
    public int UnseenAdminContactFormsCurrentPage { get; set; }
    public int SeenAdminContactFormsCurrentPage { get; set; }
    
    public int PageSize { get; set; } = 2;
    
    public async Task<IActionResult> OnGetAsync(int unseenAdminContactFormsCurrentPage = 1, int seenAdminContactFormsCurrentPage = 1) {
        if (User.Identity == null || !User.Identity.IsAuthenticated) return RedirectToPage("/Authentication");
        if (!User.IsInRole("Admin")) return RedirectToPage("/Index");
        
        UnseenAdminContactFormsCurrentPage = unseenAdminContactFormsCurrentPage;
        SeenAdminContactFormsCurrentPage = seenAdminContactFormsCurrentPage;

        // If the page is less than 0, set it to 1
        if (UnseenAdminContactFormsCurrentPage <= 0) {
            UnseenAdminContactFormsCurrentPage = 1;
            return RedirectToPage("/AdminContactForms", new { UnseenAdminContactFormsCurrentPage, SeenAdminContactFormsCurrentPage });
        }

        if (SeenAdminContactFormsCurrentPage <= 0) {
            SeenAdminContactFormsCurrentPage = 1;
            return RedirectToPage("/AdminContactForms", new { UnseenAdminContactFormsCurrentPage, SeenAdminContactFormsCurrentPage });
        }

        var unseenAdminContactForms = await _contactFormRepository.GetAdminContactFormsAsync(UnseenAdminContactFormsCurrentPage, PageSize, false);
        var seenAdminContactForms = await _contactFormRepository.GetAdminContactFormsAsync(SeenAdminContactFormsCurrentPage, PageSize, true);

        TotalUnseenAdminContactForms = unseenAdminContactForms.totalCount;
        TotalSeenAdminContactForms = seenAdminContactForms.totalCount;
        SeenAdminContactForms = seenAdminContactForms.contactForms;
        UnseenAdminContactForms = unseenAdminContactForms.contactForms;

        if (UnseenAdminContactFormsCurrentPage > (TotalUnseenAdminContactForms / PageSize + 1)) {
            UnseenAdminContactFormsCurrentPage = (int)Math.Ceiling((double)TotalUnseenAdminContactForms / PageSize * 100) / 100;
            return RedirectToPage("/AdminContactForms", new { UnseenAdminContactFormsCurrentPage, SeenAdminContactFormsCurrentPage });
        }

        if (SeenAdminContactFormsCurrentPage > (TotalSeenAdminContactForms / PageSize + 1)) {
            SeenAdminContactFormsCurrentPage = (int)Math.Ceiling((double)TotalSeenAdminContactForms / PageSize * 100) / 100;
            return RedirectToPage("/AdminContactForms", new { UnseenAdminContactFormsCurrentPage, SeenAdminContactFormsCurrentPage });
        }

        if (UnseenAdminContactFormsCurrentPage > 1 && UnseenAdminContactForms.Count() == 0) {
            UnseenAdminContactFormsCurrentPage--;
            return RedirectToPage("/AdminContactForms", new { UnseenAdminContactFormsCurrentPage, SeenAdminContactFormsCurrentPage });
        }

        if (SeenAdminContactFormsCurrentPage > 1 && SeenAdminContactForms.Count() == 0) {
            SeenAdminContactFormsCurrentPage--;
            return RedirectToPage("/AdminContactForms", new { UnseenAdminContactFormsCurrentPage, SeenAdminContactFormsCurrentPage });
        }
        
        return Page();
}

    public async Task<IActionResult> OnPostSeenUnseenAsync(int id, int UnseenAdminContactFormsCurrentPage, int SeenAdminContactFormsCurrentPage) {
        if (User.Identity == null || !User.Identity.IsAuthenticated) return RedirectToPage("/Authentication");
        if (!User.IsInRole("Admin")) return RedirectToPage("/Index");

        await _contactFormRepository.SetSeenUnseenAsync(id);
        return RedirectToPage("/AdminContactForms");
    }

    public IActionResult OnPostPage(int UnseenAdminContactFormsCurrentPage, int SeenAdminContactFormsCurrentPage) {
        if (User.Identity == null || !User.Identity.IsAuthenticated) return RedirectToPage("/Authentication");
        if (!User.IsInRole("Admin")) return RedirectToPage("/Index");

        return RedirectToPage("/AdminContactForms", new { UnseenAdminContactFormsCurrentPage, SeenAdminContactFormsCurrentPage });
    }

    public async Task<IActionResult> OnPostNrContactForms() {
        if (User.Identity == null || !User.Identity.IsAuthenticated) return new JsonResult(new {Result = 0});
        if (!User.IsInRole("Admin")) return new JsonResult(new {Result = 0});

        TotalUnseenAdminContactForms = await _contactFormRepository.GetTotalUnseenAdminContactFormsAsync();

        return new JsonResult(new {Result = TotalUnseenAdminContactForms});
    }
}