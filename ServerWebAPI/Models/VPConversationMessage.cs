using System;
using System.Collections.Generic;

namespace ServerWebAPI.Models;

public partial class VPConversationMessage
{
    public string ConversationId { get; set; } = null!;

    public long MessageId { get; set; }

    public string MemberId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string MessageType { get; set; } = null!;

    public string? Content { get; set; }

    public string? Signature { get; set; }

    public string? Source { get; set; }

    public string? ReplyFor { get; set; }

    public DateTime SendTime { get; set; }

    public bool Read { get; set; }

    public DateTime? ReadTime { get; set; }
}
