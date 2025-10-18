
using ServerWebAPI.Models;

namespace ServerWebAPI.ModelsEx
{
    public class PrivateConversationEx
    {
        public Guid ConversationId { get; set; }

        public Guid MemberId { get; set; }
        public Guid UserId { get; set; }

        // one element member List
        public Guid? OtherMemberId { get; set; }

        public VUserProfile? OtherUser { get; set; }

        public string? Remark { get; set; }
        // one element member List

        // NewestMessage
        public string? NewestMessageId { get; set; }

        public Guid? SenderMemberId { get; set; }

        public string? MessageType { get; set; }

        public string? Content { get; set; }

        public string? Signature { get; set; }

        public Guid? Source { get; set; }

        public Guid? ReplyFor { get; set; }

        public DateTime? SendTime { get; set; }

        public bool? Read { get; set; }
        // NewestMessage

        public int? UnreadCount { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
