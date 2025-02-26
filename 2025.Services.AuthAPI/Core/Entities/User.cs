using _2025.Services.AuthAPI.Core.Enum;
using System;
using System.Collections.Generic;

namespace _2025.Services.AuthAPI.Core.Entities;

public partial class User
{
    public int Id { get; set; }

    public string? FullName { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string PasswordSalt { get; set; }

    public string? Title { get; set; }

    public UserStatusEnum Status { get; set; }

    public RoleEnum Role { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public bool Delete { get; set; }
}
