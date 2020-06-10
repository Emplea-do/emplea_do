using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Web.Controllers
{
    public class SlackController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public SlackController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return Redirect(_configuration["Slack:WorkSpaceUrl"]);
        }
    }
}