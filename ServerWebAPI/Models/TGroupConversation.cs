using System;
using System.Collections.Generic;

namespace ServerWebAPI.Models;

public partial class TGroupConversation
{
    public Guid ConversationId { get; set; }

    public Guid? Owner { get; set; }

    public string? Description { get; set; }

    public Guid Avatar { get; set; }

    public DateTime CreateTime { get; set; }

    public DateTime UpdateTime { get; set; }
}
