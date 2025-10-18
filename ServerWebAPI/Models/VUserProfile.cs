using System;
using System.Collections.Generic;

namespace ServerWebAPI.Models;

public partial class VUserProfile
{
    public Guid UserId { get; set; }

    public string Emid { get; set; } = null!;

    public string NickName { get; set; } = null!;

    public string PublicKey { get; set; } = null!;

    public Guid Avatar { get; set; }

    public DateTime CreateTime { get; set; }

    public DateTime UpdateTime { get; set; }
}
