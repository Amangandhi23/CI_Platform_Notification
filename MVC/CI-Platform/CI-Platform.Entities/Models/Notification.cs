using System;
using System.Collections.Generic;

namespace CI_Platform.Entities.Models;

public partial class Notification
{
    public long Notificationid { get; set; }

    public long Userid { get; set; }

    public long Relatedid { get; set; }

    public string Notificationtype { get; set; } = null!;

    public string Notificationtext { get; set; } = null!;

    public int Isread { get; set; }

    public int IsClear { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? Avtar { get; set; }

    public virtual User User { get; set; } = null!;
}
