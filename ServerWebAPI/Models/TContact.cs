using System;
using System.Collections.Generic;

namespace ServerWebAPI.Models;

public partial class TContact
{
    public Guid ContactId { get; set; }

    /// <summary>
    /// 此数据所属用户Id
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 联系人用户Id
    /// </summary>
    public Guid ContactUserId { get; set; }

    public string? Remark { get; set; }

    public DateTime CreateTime { get; set; }

    public DateTime UpdateTime { get; set; }
}
