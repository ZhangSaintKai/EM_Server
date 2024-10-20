using Microsoft.EntityFrameworkCore;
using ServerWebAPI.Models;
using ServerWebAPI.ModelsEx;

namespace ServerWebAPI.DAL
{
    public class PrivateMessageDAL
    {
        private readonly DbEmContext _emContext;

        public PrivateMessageDAL(DbEmContext emContext)
        {
            _emContext = emContext;
        }

        public async Task Create(TPrivateMessage privateMessage)
        {
            _emContext.TPrivateMessages.Add(privateMessage);
            await _emContext.SaveChangesAsync();
        }

        public async Task<List<PrivateMessageEx>> GetListExByConversationID(string conversationId)
        {
            IQueryable<PrivateMessageEx> query =
                _emContext.VPConversationMessages.Where(m => m.ConversationId == conversationId)
                .Select(m => new PrivateMessageEx
                {
                    MessageId = m.MessageId.ToString(),
                    MemberId = m.MemberId,
                    MessageType = m.MessageType,
                    Content = m.Content ?? null,
                    Source = m.Source ?? null,
                    ReplyFor = m.ReplyFor ?? null,
                    SendTime = m.SendTime,
                    Read = m.Read,
                    ReadTime = m.ReadTime ?? null,
                });

            return await query.ToListAsync();
        }

        public async Task<List<TPrivateMessage>> GetListByConversationID(string conversationId)
        {
            IQueryable<TPrivateMessage> query =
                _emContext.VPConversationMessages.Where(m => m.ConversationId == conversationId)
                .Select(m => new TPrivateMessage
                {
                    MessageId = m.MessageId,
                    MemberId = m.MemberId,
                    MessageType = m.MessageType,
                    Content = m.Content ?? null,
                    Source = m.Source ?? null,
                    ReplyFor = m.ReplyFor ?? null,
                    SendTime = m.SendTime,
                    Read = m.Read,
                    ReadTime = m.ReadTime ?? null,
                });

            return await query.ToListAsync();
        }

        public async Task Update(IEnumerable<TPrivateMessage> privateMessages)
        {
            //检查实体的状态
            foreach (var entry in _emContext.ChangeTracker.Entries<TPrivateMessage>())
            {
                Console.WriteLine($"{entry.Entity.MemberId} - {entry.State}");
            }
            //检查实体的状态
            _emContext.TPrivateMessages.UpdateRange(privateMessages);
            await _emContext.SaveChangesAsync(true);
        }

    }
}
