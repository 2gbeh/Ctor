using Ctor.Entities;

namespace Ctor.Models;

public class User : Model
{
  public string? AvatarUrl { get; set; }                  // Avatar image URL
  public string? Name { get; set; }
  public string? Username { get; set; }
  public string? Email { get; set; }
  public string? Password { get; set; }           // Hashed password
  public DateTime? EmailVerifiedAt { get; set; }
  public DateTime? PasswordChangedAt { get; set; }
  public DateTime? LastKnownLogin { get; set; }
  public DeviceEntity? DeviceInfo { get; set; } = new();
}
