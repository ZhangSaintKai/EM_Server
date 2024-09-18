using System;
using System.Collections.Generic;

namespace ServerWebAPI.Models;

public partial class TPrivateConversation
{
    public string ConversationId { get; set; } = null!;

    public DateTime CreateTime { get; set; }

    public DateTime UpdateTime { get; set; }
}
