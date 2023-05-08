using System;
using System.Collections.Generic;

namespace CI_Platform.Entities.Models;

public partial class PasswordReset
{
    public string Email { get; set; } = null!;

    public string Token { get; set; } = null!;

    public DateTime CreateAt { get; set; }
}
