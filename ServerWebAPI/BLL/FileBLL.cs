using ServerWebAPI.Commons.Enum;
using ServerWebAPI.DAL;
using ServerWebAPI.Models;
using ServerWebAPI.ModelsEx;

namespace ServerWebAPI.BLL
{
    public class FileBLL
    {
        private readonly FileDAL _fileDAL;
        private readonly UserBLL _userBLL;
        private readonly PrivateConversationBLL _privateConversationBLL;

        public FileBLL(FileDAL fileDAL, UserBLL userBLL, PrivateConversationBLL privateConversationBLL)
        {
            _fileDAL = fileDAL;
            _userBLL = userBLL;
            _privateConversationBLL = privateConversationBLL;
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

        public async Task<bool> CheckFilePermission(TFile file, string? fileToken)
        {
            if (string.IsNullOrWhiteSpace(fileToken))
                throw new Exception("非公共文件的请求令牌不能为空");
            TUser? reqUser = await _userBLL.GetByFileToken(fileToken);
            if (reqUser == null) throw new Exception("无效文件令牌");
            if (file.OwnerType == OwnerType.Conversation.ToString())
            {
                List<PrivateConversationEx> conversationExList = await _privateConversationBLL.GetListByUserID(reqUser.UserId);
                PrivateConversationEx? conversationEx = conversationExList.Find(e => e.ConversationId == file.OwnerId);
                //此文件不属于请求用户的会话
                if (conversationEx == null) return false;
            }
            if (file.OwnerType == OwnerType.Member.ToString())
            {
                //此文件不属于请求用户的会话成员
                return false;
            }
            if (file.OwnerType == OwnerType.User.ToString())
            {
                //此文件不属于请求用户
                if (file.OwnerId != reqUser.UserId) return false;
            }
            return true;
        }

        public async Task<TFile?> GetById(string fileId)
        {
            return await _fileDAL.GetById(fileId);
        }
    }
}
