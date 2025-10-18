using System;
using System.Collections.Generic;

namespace ServerWebAPI.Models;

public partial class TPrivateConversation
{
    public Guid ConversationId { get; set; }

    public DateTime CreateTime { get; set; }

    public DateTime UpdateTime { get; set; }
}
