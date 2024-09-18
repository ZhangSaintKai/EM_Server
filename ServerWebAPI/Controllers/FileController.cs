﻿using Microsoft.AspNetCore.Mvc;
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
        private readonly UserBLL _userBLL;

        public FileController(IConfiguration configuration, IWebHostEnvironment environment, FileBLL fileBLL, UseSHA useSHA, UserBLL userBLL)
        {
            _configuration = configuration;
            _environment = environment;
            _fileBLL = fileBLL;
            _useSHA = useSHA;
            _userBLL = userBLL;
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
                // 保存
                FileStream? stream = new(filePath, FileMode.Create);
                await EM_Client_File.CopyToAsync(stream);
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
                if (string.IsNullOrWhiteSpace(fileId))
                    return StatusCode(403, "文件资源ID不能为空");
                TFile? file = await _fileBLL.GetById(fileId);
                if (file == null)
                    return StatusCode(403, "不存在此资源ID的文件");
                if (file.OwnerType != OwnerType.Public.ToString())
                {
                    //放BLL
                    if (string.IsNullOrWhiteSpace(fileToken))
                        return StatusCode(403, "非公共文件的请求令牌不能为空");
                    TUser? reqUser = await _userBLL.GetByFileToken(fileToken);
                    if (reqUser == null)
                        return StatusCode(403, "无效文件令牌");
                    //if(file.OwnerId != reqUser.UserId)
                    //    return StatusCode(403, "此文件不属于请求用户");
                    //放BLL
                }
                // 构建文件的完整路径
                string uploadFolder = GetFileCategoryFolder(file.FileType);
                string fullPath = Path.Combine(uploadFolder, file.FileStorageName);

                if (System.IO.File.Exists(fullPath))
                    return PhysicalFile(fullPath, file.FileType, file.FileName, true);
                else
                    return NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, $"加载失败: {e.Message}");
            }

        }


    }
}
