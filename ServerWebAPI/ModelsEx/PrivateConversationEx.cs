
using ServerWebAPI.Models;

namespace ServerWebAPI.ModelsEx
{
    public class PrivateConversationEx
    {
        public string ConversationId { get; set; } = null!;

        public string MemberId { get; set; } = null!;

        public string UserId { get; set; } = null!;

        // one element member List
        public string? OtherMemberId { get; set; }

        public VUserProfile? OtherUser { get; set; }

        public string? Remark { get; set; }
        // one element member List

        // NewestMessage
        public string? NewestMessageId { get; set; }

        public string? SenderMemberId { get; set; }

        public string? MessageType { get; set; }

        public string? Content { get; set; }

        public string? Source { get; set; }

        public string? ReplyFor { get; set; }

        public DateTime? SendTime { get; set; }

        public bool? Read { get; set; }
        // NewestMessage

        public long? UnreadCount { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
