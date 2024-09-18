using System;
using System.Collections.Generic;

namespace ServerWebAPI.Models;

public partial class VUserProfile
{
    public string UserId { get; set; } = null!;

    public string Emid { get; set; } = null!;

    public string NickName { get; set; } = null!;

    public string PublicKey { get; set; } = null!;

    public string Avatar { get; set; } = null!;

    public DateTime CreateTime { get; set; }

    public DateTime UpdateTime { get; set; }
}
