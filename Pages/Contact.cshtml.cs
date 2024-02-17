using Servindustria.Models;
using Servindustria.Data.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Globalization;

public class ContactModel : PageModel {
    private readonly IAdminCallRequestRepository _callRequestRepository;
    private readonly IAdminContactFormRepository _contactFormRepository;

    public ContactModel(IAdminCallRequestRepository callRequestRepository, IAdminContactFormRepository contactFormRepository) {
        _callRequestRepository = callRequestRepository;
        _contactFormRepository = contactFormRepository;
    }

    [FromForm]
    public AdminCallRequest? AdminCallRequest { get; set; }

    [FromForm]
    public AdminContactForm? ContactForm { get; set; }

    public async Task<IActionResult> OnPostAdminCallRequestAsync() {
        if (AdminCallRequest == null) return new JsonResult(new {success = false, error = "Unknown Error"});

        int phoneLength = Math.Abs(AdminCallRequest.Phone).ToString().Length;
            if (phoneLength != 9 && phoneLength != 0)
                return new JsonResult(new {success = false, error = "O número de telefone / telemóvel deve ter 9 dígitos"});

        await _callRequestRepository.AddAdminCallRequestAsync(AdminCallRequest);
        return new JsonResult(new {success = true});
    }

    public async Task<IActionResult> OnPostAdminContactFormAsync() {
        if (ContactForm == null) return new JsonResult(new {success = false, error = "Unknown Error"});

        
        if (ContactForm.Phone == null) ContactForm.Phone = 0;
        int phoneLength = Math.Abs(ContactForm.Phone.Value).ToString().Length;
            if (phoneLength != 9 && phoneLength != 0 && ContactForm.Phone != 0)
                return new JsonResult(new {success = false, error = "O número de telefone / telemóvel deve ter 9 dígitos"});
        
        if (!IsValidEmail(ContactForm.Email)) return new JsonResult(new {success = false, error = "O email não é válido"});

        await _contactFormRepository.AddAdminContactFormAsync(ContactForm);
        return new JsonResult(new {success = true});
    }


    private bool IsValidEmail(string email) {
        if (string.IsNullOrWhiteSpace(email)) return false;

        try {
            email = Regex.Replace(email, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));

            string DomainMapper(Match match) {
                var idn = new IdnMapping();
                string domainName = idn.GetAscii(match.Groups[2].Value);
                return match.Groups[1].Value + domainName;
            }
        } catch (RegexMatchTimeoutException) {
            return false;
        } catch (ArgumentException) {
            return false;
        }

        try {
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        } catch (RegexMatchTimeoutException) {
            return false;
        }
    }
}