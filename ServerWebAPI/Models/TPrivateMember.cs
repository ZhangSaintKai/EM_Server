using System;
using System.Collections.Generic;

namespace ServerWebAPI.Models;

public partial class TPrivateMember
{
    public string MemberId { get; set; } = null!;

    public string ConversationId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public DateTime CreateTime { get; set; }

    public DateTime UpdateTime { get; set; }
}
