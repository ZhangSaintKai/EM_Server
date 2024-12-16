using System;
using System.Collections.Generic;

namespace ServerWebAPI.Models;

public partial class TGroupMessage
{
    public long MessageId { get; set; }

    public string? MemberId { get; set; }

    public string MessageType { get; set; } = null!;

    public string? Content { get; set; }

    public string? Signature { get; set; }

    public string? Source { get; set; }

    public string? ReplyFor { get; set; }

    public DateTime SendTime { get; set; }
}
