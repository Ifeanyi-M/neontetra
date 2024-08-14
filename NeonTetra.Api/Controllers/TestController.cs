using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NeonTetra.Services.Interfaces;

namespace NeonTetra.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public TestController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllCampaigns()
        {
            var campaigns = await _emailService.GetAllCampaignsAsync();
            return Ok(campaigns);
        }
    }
}
