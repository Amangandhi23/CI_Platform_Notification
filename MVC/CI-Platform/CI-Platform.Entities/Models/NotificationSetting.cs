using System;
using System.Collections.Generic;

namespace CI_Platform.Entities.Models;

public partial class NotificationSetting
{
    public long NotificationSettingid { get; set; }

    public long Userid { get; set; }

    public bool? RecommandedMission { get; set; }

    public bool? RecommandedStory { get; set; }

    public bool? Mission { get; set; }

    public bool? Story { get; set; }

    public bool? Timesheet { get; set; }

    public bool? Comments { get; set; }

    public bool? MissionApplication { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
