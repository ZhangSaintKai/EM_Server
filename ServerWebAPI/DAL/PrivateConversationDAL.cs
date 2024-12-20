using Microsoft.EntityFrameworkCore;
using ServerWebAPI.Models;
using ServerWebAPI.ModelsEx;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ServerWebAPI.DAL
{
    public class PrivateConversationDAL
    {
        private readonly DbEmContext _emContext;

        public PrivateConversationDAL(DbEmContext emContext)
        {
            _emContext = emContext;
        }

        /*
         * 根据自身用户Id和对方用户Id查询是否已有私聊会话成员处于同一会话中
        */
        public async Task<PrivateConversationEx?> GetBy2UserID(string userId, string otherUserId)
        {
            IQueryable<PrivateConversationEx> query =
                from c in _emContext.VPConversations
                where c.UserId == userId && c.OtherUserId == otherUserId
                join u in _emContext.VUserProfiles on c.OtherUserId equals u.UserId into uGroup
                from u in uGroup.DefaultIfEmpty()
                join m in _emContext.VPConversationMessages on c.NewestMessageId equals m.MessageId into mGroup
                from m in mGroup.DefaultIfEmpty()
                select new PrivateConversationEx
                {
                    ConversationId = c.ConversationId,
                    MemberId = c.MemberId,
                    UserId = c.UserId,
                    OtherMemberId = c.OtherMemberId,
                    OtherUser = u ?? null,
                    NewestMessageId = m.MessageId.ToString() ?? null,
                    SenderMemberId = m.MemberId ?? null,
                    MessageType = m.MessageType ?? null,
                    Content = m.Content ?? null,
                    Signature = m.Signature ?? null,
                    Source = m.Source ?? null,
                    ReplyFor = m.ReplyFor ?? null,
                    SendTime = m.SendTime,
                    Read = m.Read,
                    Remark = c.Remark ?? null,
                    UnreadCount = c.UnreadCount,
                    CreateTime = c.CreateTime,
                    UpdateTime = c.UpdateTime,
                };
            return await query.SingleOrDefaultAsync();
        }

        public async Task Create(TPrivateConversation conversation, List<TPrivateMember> privateMembers)
        {
            _emContext.TPrivateConversations.Add(conversation);
            _emContext.TPrivateMembers.AddRange(privateMembers);
            await _emContext.SaveChangesAsync(true);
        }

        public async Task<List<PrivateConversationEx>> GetListByUserID(string userId)
        {
            IQueryable<PrivateConversationEx> query =
                from c in _emContext.VPConversations
                where c.UserId == userId
                join u in _emContext.VUserProfiles on c.OtherUserId equals u.UserId into uGroup
                from u in uGroup.DefaultIfEmpty()
                join m in _emContext.VPConversationMessages on c.NewestMessageId equals m.MessageId into mGroup
                from m in mGroup.DefaultIfEmpty()
                select new PrivateConversationEx
                {
                    ConversationId = c.ConversationId,
                    MemberId = c.MemberId,
                    UserId = c.UserId,
                    OtherMemberId = c.OtherMemberId,
                    OtherUser = u ?? null,
                    NewestMessageId = m.MessageId.ToString() ?? null,
                    SenderMemberId = m.MemberId ?? null,
                    MessageType = m.MessageType ?? null,
                    Content = m.Content ?? null,
                    Signature = m.Signature ?? null,
                    Source = m.Source ?? null,
                    ReplyFor = m.ReplyFor ?? null,
                    SendTime = m.SendTime,
                    Read = m.Read,
                    Remark = c.Remark ?? null,
                    UnreadCount = c.UnreadCount,
                    CreateTime = c.CreateTime,
                    UpdateTime = c.UpdateTime,
                };
            return await query.ToListAsync();

        }

        public async Task<PrivateConversationEx?> GetByIDUserID(string conversationId, string userId)
        {
            IQueryable<PrivateConversationEx> query =
                from c in _emContext.VPConversations
                where c.ConversationId == conversationId && c.UserId == userId
                join u in _emContext.VUserProfiles on c.OtherUserId equals u.UserId into uGroup
                from u in uGroup.DefaultIfEmpty()
                join m in _emContext.VPConversationMessages on c.NewestMessageId equals m.MessageId into mGroup
                from m in mGroup.DefaultIfEmpty()
                select new PrivateConversationEx
                {
                    ConversationId = c.ConversationId,
                    MemberId = c.MemberId,
                    UserId = c.UserId,
                    OtherMemberId = c.OtherMemberId,
                    OtherUser = u ?? null,
                    NewestMessageId = m.MessageId.ToString() ?? null,
                    SenderMemberId = m.MemberId ?? null,
                    MessageType = m.MessageType ?? null,
                    Content = m.Content ?? null,
                    Signature = m.Signature ?? null,
                    Source = m.Source ?? null,
                    ReplyFor = m.ReplyFor ?? null,
                    SendTime = m.SendTime,
                    Read = m.Read,
                    Remark = c.Remark ?? null,
                    UnreadCount = c.UnreadCount,
                    CreateTime = c.CreateTime,
                    UpdateTime = c.UpdateTime,
                };
            return await query.SingleOrDefaultAsync();
        }

    }
}
