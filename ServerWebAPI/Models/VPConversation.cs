using System;
using System.Collections.Generic;

namespace ServerWebAPI.Models;

public partial class VPConversation
{
    public Guid ConversationId { get; set; }

    public Guid MemberId { get; set; }

    public Guid UserId { get; set; }

    public Guid? OtherMemberId { get; set; }

    public Guid? OtherUserId { get; set; }

    public string? Remark { get; set; }

    public long? NewestMessageId { get; set; }

    public int? UnreadCount { get; set; }

    public DateTime CreateTime { get; set; }

    public DateTime UpdateTime { get; set; }
}
