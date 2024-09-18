using ServerWebAPI.DAL;
using ServerWebAPI.Models;
using ServerWebAPI.ModelsEx;
using ServerWebAPI.Services;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace ServerWebAPI.BLL
{
    public class PrivateMessageBLL
    {
        private readonly PrivateMessageDAL _messageDAL;
        private readonly PrivateMemberBLL _memberBLL;
        private readonly PrivateConversationBLL _conversationBLL;
        private readonly WebSocketService _wss;

        public PrivateMessageBLL(PrivateMessageDAL messageDAL, PrivateMemberBLL memberBLL, WebSocketService webSocketService, PrivateConversationBLL conversationBLL)
        {
            _messageDAL = messageDAL;
            _memberBLL = memberBLL;
            _conversationBLL = conversationBLL;
            _wss = webSocketService;
        }

        public async Task<List<PrivateMessageEx>> GetList(string userId, string conversationId)
        {
            //PrivateConversationEx conversation = await _conversationBLL.GetByIDUserID(conversationId, userId) ?? throw new Exception("当前登录用户不是该会话中的成员");
            List<PrivateMessageEx> messageExList = await _messageDAL.GetList(conversationId);
            //读不是发送者的消息
            //messageExList.ForEach(msg =>
            //{
            //    if (msg.MemberId != conversation.MemberId)
            //    {
            //        msg.Read = true;
            //        msg.ReadTime = DateTime.Now;
            //    }
            //});
            //IEnumerable<TPrivateMessage> messageReadList = messageExList.Where(msg => msg.MemberId != conversation.MemberId);
            //await _messageDAL.Update(messageReadList);
            //读不是发送者的消息
            return messageExList;
        }

        public async Task Send(string userId, string conversationId, string messageType, string content, string? source, string? replyFor)
        {
            PrivateConversationEx conversation = await _conversationBLL.GetByIDUserID(conversationId, userId) ?? throw new Exception("当前登录用户不是该会话中的成员");
            TPrivateMessage message = new()
            {
                MemberId = conversation.MemberId,
                MessageType = messageType,
                Content = content,
                Source = source,
                ReplyFor = replyFor,
                SendTime = DateTime.Now,
                Read = false,
                ReadTime = null,
            };
            await _messageDAL.Create(message);
            //
            if (conversation.OtherUser == null || string.IsNullOrWhiteSpace(conversation.OtherUser.UserId)) throw new Exception("找不到此成员所属会话的其他成员");
            WebSocket? webSocket = _wss.GetWS(Guid.Parse(conversation.OtherUser.UserId));
            if (webSocket != null)
            {
                // 将消息转换为字节数组
                byte[]? buffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
                // 发送消息到指定WebSocket实例
                await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            //else对方离线

        }

        public async Task Read(string userId, string conversationId)
        {
            PrivateConversationEx conversation = await _conversationBLL.GetByIDUserID(conversationId, userId) ?? throw new Exception("当前登录用户不是该会话中的成员");
            List<PrivateMessageEx> messageExList = await _messageDAL.GetList(conversationId);
            //读不是发送者的消息
            messageExList.ForEach(msg =>
            {
                if (msg.MemberId != conversation.MemberId)
                {
                    msg.Read = true;
                    msg.ReadTime = DateTime.Now;
                }
            });
            IEnumerable<TPrivateMessage> messageReadList = messageExList.Where(msg => msg.MemberId != conversation.MemberId);  // 子类转父类（隐式）
            await _messageDAL.Update(messageReadList);
            //读不是发送者的消息
        }
    }
}
