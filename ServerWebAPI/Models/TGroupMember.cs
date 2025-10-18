using System;
using System.Collections.Generic;

namespace ServerWebAPI.Models;

public partial class TGroupMember
{
    public Guid MemberId { get; set; }
    public Guid ConversationId { get; set; }

    public Guid? UserId { get; set; }

    public bool IsAdmin { get; set; }

    public string? UserRemark { get; set; }

    public string? GroupRemark { get; set; }

    public DateTime CreateTime { get; set; }

    public DateTime UpdateTime { get; set; }
}
