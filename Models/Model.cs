namespace Ctor.Models;

public abstract class Model
{
  public int Id { get; set; }                          // Internal PK
  public Guid Uuid { get; set; } = Guid.NewGuid();     // Public ID
  public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
  public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
  public DateTime? DeletedAt { get; set; }             // For soft deletes
}