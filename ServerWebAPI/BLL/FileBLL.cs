using ServerWebAPI.DAL;
using ServerWebAPI.Models;

namespace ServerWebAPI.BLL
{
    public class FileBLL
    {
        private readonly FileDAL _fileDAL;

        public FileBLL(FileDAL fileDAL)
        {
            _fileDAL = fileDAL;
        }

        public async Task<bool> IsExist(string fileId)
        {
            return await _fileDAL.IsExist(fileId);
        }

        public async Task SaveFileAndRefer(string fileId, string fileName, string fileType, string fileStorageName, string ownerType, string ownerId)
        {
            TFile file = new()
            {
                FileId = fileId,
                FileName = fileName,
                FileType = fileType,
                FileStorageName = fileStorageName,
                OwnerType = ownerType,
                OwnerId = ownerId,
                CreateTime = DateTime.Now
            };
            await _fileDAL.Create(file);
        }

        public async Task<TFile?> GetById(string fileId)
        {
            return await _fileDAL.GetById(fileId);
        }
    }
}
