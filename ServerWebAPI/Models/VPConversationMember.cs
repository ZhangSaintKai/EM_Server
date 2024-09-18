using System;
using System.Collections.Generic;

namespace ServerWebAPI.Models;

public partial class VPConversationMember
{
    public string ConversationId { get; set; } = null!;

    public string MemberId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string? OtherMemberId { get; set; }

    public string? OtherUserId { get; set; }
}
