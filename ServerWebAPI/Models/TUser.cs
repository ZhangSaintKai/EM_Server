using System;
using System.Collections.Generic;

namespace ServerWebAPI.Models;

public partial class TUser
{
    public Guid UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Emid { get; set; } = null!;

    public string NickName { get; set; } = null!;

    public string PublicKey { get; set; } = null!;

    public Guid Avatar { get; set; }

    /// <summary>
    /// 只是表示在设计表时规定最大长度为255个字符，但实际存储的数据长度可以超过这个限制
    /// </summary>
    public string? Token { get; set; }

    public string? FileToken { get; set; }

    public DateTime CreateTime { get; set; }

    public DateTime UpdateTime { get; set; }
}
