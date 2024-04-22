using Microsoft.AspNetCore.Identity;

namespace task4_1.Models;

public class AppUser : IdentityUser
{
    public string? Name { get; set; }
    public DateTimeOffset RegistrationDate { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset LastLoginDate { get; set; } = DateTimeOffset.UtcNow;
    public bool IsBlocked { get; set; } = false;
}