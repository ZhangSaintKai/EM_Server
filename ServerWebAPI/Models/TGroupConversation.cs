using System;
using System.Collections.Generic;

namespace ServerWebAPI.Models;

public partial class TGroupConversation
{
    public string ConversationId { get; set; } = null!;

    public string? Owner { get; set; }

    public string? Description { get; set; }

    public string Avatar { get; set; } = null!;

    public DateTime CreateTime { get; set; }

    public DateTime UpdateTime { get; set; }
}
