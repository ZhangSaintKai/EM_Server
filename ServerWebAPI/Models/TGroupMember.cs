using System;
using System.Collections.Generic;

namespace ServerWebAPI.Models;

public partial class TGroupMember
{
    public string MemberId { get; set; } = null!;

    public string ConversationId { get; set; } = null!;

    public string? UserId { get; set; }

    public int IsAdmin { get; set; }

    public string? UserRemark { get; set; }

    public string? GroupRemark { get; set; }

    public DateTime CreateTime { get; set; }

    public DateTime UpdateTime { get; set; }
}
