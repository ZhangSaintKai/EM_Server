using Microsoft.AspNetCore.Mvc;
using ServerWebAPI.BLL;
using ServerWebAPI.Commons.Algorithm;
using ServerWebAPI.Commons.Enum;
using ServerWebAPI.Models;

namespace ServerWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class FileController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly FileBLL _fileBLL;
        private readonly UseSHA _useSHA;

        public FileController(IConfiguration configuration, IWebHostEnvironment environment, FileBLL fileBLL, UseSHA useSHA)
        {
            _configuration = configuration;
            _environment = environment;
            _fileBLL = fileBLL;
            _useSHA = useSHA;
        }

        [HttpGet]
        public IActionResult GetAllowedMimes()
        {
            try
            {
                List<string> allowedMimes = _configuration.GetRequiredSection("UploadFile").GetRequiredSection("AllowedMimes").Get<List<string>>();
                var allowedMimesEx = _configuration.GetRequiredSection("UploadFile").GetRequiredSection("AllowedMimesEx").Get<Dictionary<string, List<string>>>();
                var result = new { allowedMimes, allowedMimesEx };
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, "获取允许文件类型失败：" + e.Message);
            }
        }

        [HttpPost]
        [RequestSizeLimit(100_000_000)] // 允许的请求正文的最大大小（以字节为单位）
        public async Task<IActionResult> Upload(IFormFile EM_Client_File, string ownerId, OwnerType ownerType)
        {
            if (EM_Client_File == null || EM_Client_File.Length == 0)
                return UnprocessableEntity("文件不能为空");
            if (string.IsNullOrWhiteSpace(ownerId))
                return UnprocessableEntity("文件归属ID不能为空");
            try
            {
                Stream? filestream = EM_Client_File.OpenReadStream();

                // 按文件类型分类保存的文件夹
                string uploadFolder = GetFileCategoryFolder(EM_Client_File.ContentType);
                // 重命名文件
                string[]? splitName = EM_Client_File.FileName.Split('.');
                string fileId = Guid.NewGuid().ToString();
                if (splitName.Length < 2)
                    throw new Exception("文件没有后缀名");
                string extension = splitName.Last();
                string fileStorageName = $"{fileId}.{extension}";
                // 拼接保存目录与文件存储名
                string filePath = Path.Combine(uploadFolder, fileStorageName);
                // 保存，使用using确保FileStream在操作完成后自动关闭
                using (FileStream stream = new(filePath, FileMode.Create))
                {
                    await EM_Client_File.CopyToAsync(stream);
                }
                await _fileBLL.SaveFileAndRefer(fileId, EM_Client_File.FileName, EM_Client_File.ContentType, fileStorageName, ownerType.ToString(), ownerId);
                //return Ok($"文件 {EM_Client_File.FileName} 上传成功");
                return Ok(new { FileId = fileId });
            }
            catch (Exception e)
            {
                return StatusCode(500, $"上传失败: {e.Message}");
            }
        }

        private string GetFileCategoryFolder(string contentType)
        {
            string fileType = "other";
            var uploadFileConfiguration = _configuration.GetRequiredSection("UploadFile");
            List<string> allowedMimes = uploadFileConfiguration.GetRequiredSection("AllowedMimes").Get<List<string>>();
            string[] contentTypeArr = contentType.Split('/');
            string mimeType = contentTypeArr.Length > 0 ? contentTypeArr[0] : "";
            //文件类型在AllowedMimes中
            if (allowedMimes.Contains(mimeType))
                fileType = mimeType;
            else
            {
                var allowedMimesEx = uploadFileConfiguration.GetRequiredSection("AllowedMimesEx").Get<Dictionary<string, List<string>>>();
                // 文件类型在AllowedMimesEx下某个分类的数组中
                KeyValuePair<string, List<string>> cate = allowedMimesEx.FirstOrDefault(cate => cate.Value.Any(mime => mime == contentType));
                if (cate.Key != null)
                    fileType = cate.Key;
                else
                    throw new Exception("不允许的文件类型");
            }
            string rootPath = uploadFileConfiguration.GetValue<string>("RootPath");
            string uploadFolder = Path.Combine(_environment.ContentRootPath, rootPath, fileType);
            // 如果目录不存在，创建目录
            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);
            return uploadFolder;
        }

        [HttpGet]
        public async Task<IActionResult> GetFile(string fileId, string? fileToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileId)) throw new Exception("文件资源ID不能为空");
                TFile? file = await _fileBLL.GetById(fileId);
                if (file == null) throw new Exception("资源ID不存在");
                if (file.OwnerType != OwnerType.Public.ToString())
                {
                    bool permission = await _fileBLL.CheckFilePermission(file, fileToken);
                    if (!permission) return StatusCode(403, "没有权限");
                }
                // 构建文件的完整路径
                string uploadFolder = GetFileCategoryFolder(file.FileType);
                string fullPath = Path.Combine(uploadFolder, file.FileStorageName);
                // 返回文件
                if (System.IO.File.Exists(fullPath))
                    return PhysicalFile(fullPath, file.FileType, file.FileName, true);
                else
                    throw new Exception("文件不存在");
            }
            catch (Exception e)
            {
                return StatusCode(500, $"加载失败: {e.Message}");
            }

        }

        [HttpGet]
        public async Task<IActionResult> ReadFile(string fileId, string? fileToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileId)) throw new Exception("文件资源ID不能为空");
                TFile? file = await _fileBLL.GetById(fileId);
                if (file == null) throw new Exception("资源ID不存在");
                if (file.OwnerType == OwnerType.Public.ToString())
                {
                    throw new Exception("公共文件不可删除!!!");
                }
                else
                {
                    bool permission = await _fileBLL.CheckFilePermission(file, fileToken);
                    if (!permission) return StatusCode(403, "没有权限");
                }
                await _fileBLL.Delete(file);
                // 构建文件的完整路径
                string uploadFolder = GetFileCategoryFolder(file.FileType);
                string fullPath = Path.Combine(uploadFolder, file.FileStorageName);
                // 删除文件
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                    return Ok($"文件“{file.FileName}”已删除");
                }
                else throw new Exception("文件不存在");
            }
            catch (Exception e)
            {
                return StatusCode(500, $"阅后即焚失败: {e.Message}");
            }

        }


    }
}
