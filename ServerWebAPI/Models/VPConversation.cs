using System;
using System.Collections.Generic;

namespace ServerWebAPI.Models;

public partial class VPConversation
{
    public string ConversationId { get; set; } = null!;

    public string MemberId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string? OtherMemberId { get; set; }

    public string? OtherUserId { get; set; }

    public string? Remark { get; set; }

    public long? NewestMessageId { get; set; }

    public int? UnreadCount { get; set; }

    public DateTime CreateTime { get; set; }

    public DateTime UpdateTime { get; set; }
}
