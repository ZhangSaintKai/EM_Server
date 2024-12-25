using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using ServerWebAPI.Schemas;

namespace ServerWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ClientController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public ClientController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult CheckUpdate(string version)
        {
            try
            {
                var newestClientVersion = _configuration.GetRequiredSection("ClientVersion");
                var result = new
                {
                    version = newestClientVersion["version"],
                    description = newestClientVersion["description"],
                    update = newestClientVersion["version"] != version,

                };
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, "检查更新失败：" + e.Message);
            }
        }

        [HttpGet]
        public IActionResult GetPkgWgt()
        {
            try
            {
                var newestClientVersion = _configuration.GetRequiredSection("ClientVersion");
                string? pkgUrl = newestClientVersion["pkgUrl"];
                //var wgtUrl = newestClientVersion["wgtUrl"];
                if (string.IsNullOrEmpty(pkgUrl)) throw new Exception("pkgUrl为空");
                string fullPath = Path.Combine(_environment.ContentRootPath, pkgUrl);
                if (!System.IO.File.Exists(fullPath)) throw new Exception("文件不存在");
                string pkgName = pkgUrl.Split('/').Last();
                return PhysicalFile(fullPath, "application/vnd.android.package-archive", pkgName, true);
            }
            catch (Exception e)
            {
                return StatusCode(500, "获取安装包失败：" + e.Message);
            }
        }
    }
}
