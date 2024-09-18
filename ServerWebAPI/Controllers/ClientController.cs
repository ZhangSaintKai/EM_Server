using Microsoft.AspNetCore.Mvc;

namespace ServerWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ClientController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public ClientController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult CheckUpdate()
        {
            try
            {
                var clientVersion = _configuration.GetRequiredSection("ClientVersion");
                var result = new
                {
                    version = clientVersion["version"],
                    description = clientVersion["description"],
                    pkgUrl = clientVersion["pkgUrl"],
                    wgtUrl = clientVersion["wgtUrl"]

                };
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, "检查更新失败：" + e.Message);

            }
        }
    }
}
