using Servindustria.Models;
using Servindustria.Data.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

public class ContactModel : PageModel {
    private readonly ICallRequestRepository _repository;

    public ContactModel(ICallRequestRepository repository) {
        _repository = repository;
    }

    [FromForm]
    public CallRequest CallRequest { get; set; } = null!;

    public async Task<IActionResult> OnPostCallRequestAsync() {
        int phoneLength = Math.Abs(CallRequest.Phone).ToString().Length;
            if (phoneLength != 9 && phoneLength != 0)
                return new JsonResult(new {success = false, error = "O número de telefone / telemóvel deve ter 9 dígitos"});

        await _repository.AddCallRequestAsync(CallRequest);
        return new JsonResult(new {success = true});
    }
}