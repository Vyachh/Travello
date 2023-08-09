using Microsoft.AspNetCore.Mvc;
using TravelloApi.Dto;
using TravelloApi.Interfaces;

namespace TravelloApi.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class EmailController : ControllerBase
  {
    private readonly IEmailService _emailService;

    public EmailController(IEmailService emailService)
    {
      _emailService = emailService;
    }

    [HttpPost("sendemail")]
    public async Task<IActionResult> SendEmail([FromForm] EmailDto request)
    {
      _emailService.SendEmail(request);
      return Ok();
    }
  }
}
