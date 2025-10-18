using System;
using System.Collections.Generic;

namespace ServerWebAPI.Models;

public partial class TGroupMessageRead
{
    public long MessageId { get; set; }

    public Guid MemberId { get; set; }

    public bool Read { get; set; }

    public DateTime? ReadTime { get; set; }
}
