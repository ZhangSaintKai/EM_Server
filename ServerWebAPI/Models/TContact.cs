using System;
using System.Collections.Generic;

namespace ServerWebAPI.Models;

public partial class TContact
{
    public string ContactId { get; set; } = null!;

    /// <summary>
    /// 此数据所属用户Id
    /// </summary>
    public string UserId { get; set; } = null!;

    /// <summary>
    /// 联系人用户Id
    /// </summary>
    public string ContactUserId { get; set; } = null!;

    public string? Remark { get; set; }

    public DateTime CreateTime { get; set; }

    public DateTime UpdateTime { get; set; }
}
