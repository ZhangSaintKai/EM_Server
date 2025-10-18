using System;
using System.Collections.Generic;

namespace ServerWebAPI.Models;

public partial class TPrivateMember
{
    public Guid MemberId { get; set; }

    public Guid ConversationId { get; set; }

    public Guid UserId { get; set; }

    public DateTime CreateTime { get; set; }

    public DateTime UpdateTime { get; set; }
}
