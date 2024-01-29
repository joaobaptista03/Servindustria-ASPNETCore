using Servindustria.Models;
using Servindustria.Data.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

public class ContactModel : PageModel {
    private readonly IAdminCallRequestRepository _repository;

    public ContactModel(IAdminCallRequestRepository repository) {
        _repository = repository;
    }

    [FromForm]
    public AdminCallRequest AdminCallRequest { get; set; } = null!;

    public async Task<IActionResult> OnPostAdminCallRequestAsync() {
        int phoneLength = Math.Abs(AdminCallRequest.Phone).ToString().Length;
            if (phoneLength != 9 && phoneLength != 0)
                return new JsonResult(new {success = false, error = "O número de telefone / telemóvel deve ter 9 dígitos"});

        await _repository.AddAdminCallRequestAsync(AdminCallRequest);
        return new JsonResult(new {success = true});
    }
}