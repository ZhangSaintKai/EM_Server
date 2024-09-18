using System;
using System.Collections.Generic;

namespace ServerWebAPI.Models;

public partial class TGroupMessageRead
{
    public string MessageId { get; set; } = null!;

    public string MemberId { get; set; } = null!;

    public bool Read { get; set; }

    public DateTime? ReadTime { get; set; }
}
