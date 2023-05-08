﻿using System;
using System.Collections.Generic;

namespace CI_Platform.Entities.Models;

public partial class ContactU
{
    public long Contactusid { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Subject { get; set; } = null!;

    public string Message { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }
}
