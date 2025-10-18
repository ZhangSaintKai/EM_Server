using System;
using System.Collections.Generic;

namespace ServerWebAPI.Models;

public partial class VPConversationMessage
{
    public Guid ConversationId { get; set; }

    public long MessageId { get; set; }

    public Guid MemberId { get; set; }

    public Guid UserId { get; set; }

    public string MessageType { get; set; } = null!;

    public string? Content { get; set; }

    public string? Signature { get; set; }

    public Guid? Source { get; set; }

    public Guid? ReplyFor { get; set; }

    public DateTime SendTime { get; set; }

    public bool Read { get; set; }

    public DateTime? ReadTime { get; set; }
}
