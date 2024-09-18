using System;
using System.Collections.Generic;

namespace ServerWebAPI.Models;

public partial class TFile
{
    public string FileId { get; set; } = null!;

    public string FileName { get; set; } = null!;

    public string FileType { get; set; } = null!;

    public string FileStorageName { get; set; } = null!;

    public string OwnerType { get; set; } = null!;

    public string? OwnerId { get; set; }

    public DateTime CreateTime { get; set; }
}
