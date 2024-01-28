using Servindustria.Models;
using Servindustria.Data.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

public class AuthenticationModel : PageModel {
    private readonly IUserRepository _repository;

    /* REGISTER */
    [FromForm]
    public User? NewUser { get; set; }
    
    [BindProperty]
    public string? ConfirmPassword { get; set; } = "";

    /* LOGIN */
    [BindProperty]
    public string? loginEmail { get; set; } = "";
    
    [BindProperty]
    public string? loginPassword { get; set; } = "";

    public AuthenticationModel(IUserRepository repository) {
        _repository = repository;
    }

    public IActionResult OnGet() {
        if (User.Identity != null && User.Identity.IsAuthenticated) return RedirectToPage("/Index");
        return Page();
    }

    public async Task<IActionResult> OnPostLoginAsync() {
        if (loginEmail == null || loginPassword == null) return new JsonResult(new {success = false, error = "Email e/ou Password incorretos"});

        if (await _repository.LoginAsync(loginEmail, loginPassword)) {
            var Claims = new List<Claim> {
                        new Claim(ClaimTypes.Name, loginEmail),
                        new Claim(ClaimTypes.Role, await _repository.IsAdmin(loginEmail) ? "Admin" : "User")
                    };
            await HttpContext.SignInAsync("AuthCookies", new ClaimsPrincipal(new ClaimsIdentity(Claims, "AuthCookies")));
        
            return new JsonResult(new {success = true});
        } else {
            return new JsonResult(new {success = false, error = "Email ou Password incorretos"});
        }
    }

    public async Task<IActionResult> OnPostRegisterAsync() {
        if (NewUser == null) return new JsonResult(new {success = false, errors = "Unknown Error"});

        if (await _repository.EmailExistsAsync(NewUser.Email)) ModelState.AddModelError("Email", "Já existe um utilizador com este email");
        if (NewUser.Nif.HasValue)
            if (await _repository.NifExistsAsync(NewUser.Nif.Value))
                ModelState.AddModelError("Nif", "Já existe um utilizador com este NIF");
        
        if (!IsValidEmail(NewUser.Email)) ModelState.AddModelError("Email", "O email não é válido");
        if (NewUser.Password != ConfirmPassword) ModelState.AddModelError("Password", "As passwords não coincidem");
        
        if (NewUser.Nif.HasValue) {
            int nifLength = Math.Abs(NewUser.Nif.Value).ToString().Length;
            if (NewUser.Nif == 0) nifLength = 0;
            if (nifLength != 9 && nifLength != 0) ModelState.AddModelError("Nif", "O NIF deve ter 9 dígitos");
        }
        
        if (NewUser.Phone.HasValue) {
            int phoneLength = Math.Abs(NewUser.Phone.Value).ToString().Length;
            if (NewUser.Phone == 0) phoneLength = 0;
                if (phoneLength != 9 && phoneLength != 0) ModelState.AddModelError("Phone", "O número de telefone / telemóvel deve ter 9 dígitos");
        }
        
        var modelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
        if (modelErrors.Count > 0) return new JsonResult(new {success = false, errors = modelErrors});

        var Claims = new List<Claim> {
            new Claim(ClaimTypes.Name, NewUser.Email),
            new Claim(ClaimTypes.Role, await _repository.IsAdmin(NewUser.Email) ? "Admin" : "User")
        };
        await HttpContext.SignInAsync("AuthCookies", new ClaimsPrincipal(new ClaimsIdentity(Claims, "AuthCookies")));
        
        NewUser.Password = BCrypt.Net.BCrypt.HashPassword(NewUser.Password);

        await _repository.AddUserAsync(NewUser);
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
    
    public async Task<IActionResult> OnPostLogoutAsync() {
        await HttpContext.SignOutAsync("AuthCookies");
        return new JsonResult(new {success = true});
    }
}