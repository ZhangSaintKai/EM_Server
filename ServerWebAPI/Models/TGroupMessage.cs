using System;
using System.Collections.Generic;

namespace ServerWebAPI.Models;

public partial class TGroupMessage
{
    public long MessageId { get; set; }

    public Guid? MemberId { get; set; }

    public string MessageType { get; set; } = null!;

    public string? Content { get; set; }

    public string? Signature { get; set; }

    public Guid? Source { get; set; }

    public Guid? ReplyFor { get; set; }

    public DateTime SendTime { get; set; }
}
