using ServerWebAPI.DAL;
using ServerWebAPI.Models;
using ServerWebAPI.ModelsEx;

namespace ServerWebAPI.BLL
{
    public class PrivateConversationBLL
    {
        private readonly PrivateConversationDAL _conversationDAL;
        private readonly UserBLL _userBLL;

        public PrivateConversationBLL(PrivateConversationDAL conversationDAL, UserBLL userBLL)
        {
            _conversationDAL = conversationDAL;
            _userBLL = userBLL;
        }

        public async Task<List<PrivateConversationEx>> GetListByUserID(Guid userId)
        {
            List<PrivateConversationEx> conversationExList = await _conversationDAL.GetListByUserID(userId);
            return conversationExList;
        }

        public async Task<PrivateConversationEx?> GetByIDUserID(Guid conversationId, Guid userId)
        {
            return await _conversationDAL.GetByIDUserID(conversationId, userId);
        }

        public async Task<PrivateConversationEx?> Create(Guid userId, Guid otherUserId)
        {

            VUserProfile? otherUser = await _userBLL.GetByProfileUserID(otherUserId) ?? throw new Exception("私聊对象用户不存在");
            //排重
            PrivateConversationEx? conversation = await _conversationDAL.GetBy2UserID(userId, otherUserId);
            //不存在私聊会话时，创建新会话
            if (conversation == null)
            {
                Guid conversationId = Guid.NewGuid();
                TPrivateConversation conversationNew = new()
                {
                    ConversationId = conversationId,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                };
                TPrivateMember self = new()
                {
                    MemberId = Guid.NewGuid(),
                    ConversationId = conversationId,
                    UserId = userId,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                };
                TPrivateMember other = new()
                {
                    MemberId = Guid.NewGuid(),
                    ConversationId = conversationId,
                    UserId = otherUserId,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                };
                List<TPrivateMember> privateMembers = new()
                {
                    self,
                    other
                };
                await _conversationDAL.Create(conversationNew, privateMembers);
                conversation = await _conversationDAL.GetByIDUserID(conversationId, userId);
            }
            return conversation;

        }

    }
}
