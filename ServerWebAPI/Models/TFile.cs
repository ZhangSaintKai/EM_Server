using System;
using System.Collections.Generic;

namespace ServerWebAPI.Models;

public partial class TFile
{
    public Guid FileId { get; set; }

    public string FileName { get; set; } = null!;

    public string FileType { get; set; } = null!;

    public string FileStorageName { get; set; } = null!;

    public string PermissionType { get; set; } = null!;

    public Guid? OwnerId { get; set; }

    public DateTime CreateTime { get; set; }
}
