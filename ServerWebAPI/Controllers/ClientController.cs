using Microsoft.AspNetCore.Mvc;
using ServerWebAPI.Schemas;

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

        [HttpPost]
        public IActionResult CheckUpdate(ClientVersion clientVersion)
        {
            try
            {
                var newestClientVersion = _configuration.GetRequiredSection("ClientVersion");
                var result = new
                {
                    version = newestClientVersion["version"],
                    description = newestClientVersion["description"],
                    pkgUrl = newestClientVersion["pkgUrl"],
                    wgtUrl = newestClientVersion["wgtUrl"],
                    update = newestClientVersion["version"] != clientVersion.Version,

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
