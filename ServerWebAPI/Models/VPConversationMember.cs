using System;
using System.Collections.Generic;

namespace ServerWebAPI.Models;

public partial class VPConversationMember
{
    public Guid ConversationId { get; set; }

    public Guid MemberId { get; set; }

    public Guid UserId { get; set; }

    public Guid? OtherMemberId { get; set; }

    public Guid? OtherUserId { get; set; }
}
